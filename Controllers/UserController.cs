using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API_Tutorial.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace API_Tutorial.Controllers;

[ApiController]
[Route("[Controller]")]
public class UserController : ControllerBase
{
    private readonly AppSetting setting;

    public UserController(MyDbContext _context,IOptionsMonitor<AppSetting> _setting)
    {
        Context = _context;
        setting = _setting.CurrentValue;
    }

    public MyDbContext Context { get; }

    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginModel user)
    {
        var u = Context.Users.FirstOrDefault(u=>u.UserName==user.UserName && u.PassWord==user.PassWord);
        
        if(u==null)
        {
            return Ok(new ResponseModel{
                Susscess=false,
                Message="inValidate User/Pass"

            });
        }
        //Console.WriteLine($"use name {u.UserName} - {u.Email}");
        //gentoken
        TokenModel token = await GentokenJWT(u);
        //Console.WriteLine($"Token {strtoken}");
        return Ok(new ResponseModel{
            Susscess=true,
            Message="Authenticate Success",
            Data=token
        });
    }

    private async Task<TokenModel> GentokenJWT(UserModel u)
    {
        var jwthandler=new JwtSecurityTokenHandler();
        //Console.WriteLine($"secretkey {setting.SecretKey}");
        IdentityModelEventSource.ShowPII = true;


        var SecretkeyBytes = Encoding.UTF8.GetBytes(setting.SecretKey);
        var tokendescription=new SecurityTokenDescriptor{
            Subject = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Name,u.Fname+"-"+u.Lname),
                new Claim(JwtRegisteredClaimNames.Email,u.Email),
                new Claim(JwtRegisteredClaimNames.Sub,u.Email),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),

                new Claim("username",u.UserName),
                new Claim("Id",u.Id.ToString()),


                //roles
            }),
            Expires=DateTime.UtcNow.AddMinutes(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(SecretkeyBytes),SecurityAlgorithms.HmacSha512Signature)
        };

        var token = jwthandler.CreateToken(tokendescription);
        var accesstoken=jwthandler.WriteToken(token);
        var refreshtoken=GenerateRefreshToken();

        //luu database
        RefreshTokenModel refreshmodel=new RefreshTokenModel{
            Id = Guid.NewGuid(),
            UserId=u.Id,
            Token=refreshtoken,
            JwtId=token.Id,
            IsUsed=false,
            IsRevoked=false,
            IsSuedAt=DateTime.UtcNow,
            ExpireAt=DateTime.UtcNow.AddHours(1),

        };

        await Context.RefreshToken.AddAsync(refreshmodel);
        await Context.SaveChangesAsync();

        return new TokenModel{
            AccessToken=accesstoken,
            RefreshToken=refreshtoken
        };
    }

    private string GenerateRefreshToken()
    {
        var random = new Byte[32]; 
        using(var ran = RandomNumberGenerator.Create())
        {
            ran.GetBytes(random);
            return Convert.ToBase64String(random);
        }
    }

    [HttpPost("RenewToken")]
    public async Task<IActionResult> RenewToken(TokenModel model)
    {

        try
        {
            var tokenhandler=new JwtSecurityTokenHandler();
            var secretkeybytes=Encoding.UTF8.GetBytes(setting.SecretKey);
            var TokenValidationParameters=new TokenValidationParameters{
                        //tự cấp token
                        ValidateIssuer=false,
                        ValidateAudience=false,

                        //ký vào token
                        ValidateIssuerSigningKey=true,
                        IssuerSigningKey = new SymmetricSecurityKey(secretkeybytes),

                        ClockSkew=TimeSpan.Zero,
                        ValidateLifetime=false     // không kiểm tra token hết hạn
                    };

                //1. Checktoken valid format
                var tokeninVerification=tokenhandler.ValidateToken(model.AccessToken,TokenValidationParameters,out var validatedToken);
                //2 check alg
                if(validatedToken is JwtSecurityToken jwttoken)
                {
                    var result=jwttoken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512,StringComparison.InvariantCultureIgnoreCase);
                    if(!result)
                    {
                        return Ok(new ResponseModel{
                            Susscess=false,
                            Message="Invalid Token"
                        });
                    }
                }
                //3. check accesstoken expire
                var utcExpiredDate=long.Parse(tokeninVerification.Claims.FirstOrDefault(x=>x.Type==JwtRegisteredClaimNames.Exp).Value);
                var expiredate=ConvertUnixTimeToDateTime(utcExpiredDate);
                //Console.WriteLine($"epiredate {expiredate} - to now {DateTime.UtcNow}");
                if(expiredate>DateTime.UtcNow)
                {
                    return Ok(new ResponseModel{
                        Susscess=false,
                        Message="Accesstoken don't Expired "
                    });
                }
                //4.Check token db
                var refreshtokenmodel=Context.RefreshToken.FirstOrDefault(r=>r.Token==model.RefreshToken);
                if(refreshtokenmodel==null)
                {
                    return Ok(new ResponseModel{
                        Susscess=false,
                        Message="Token not found "
                    });
                }
            //5.refreshtoken isused/revoked
                if(refreshtokenmodel.IsUsed)
                {
                    return Ok(new ResponseModel{
                        Susscess=false,
                        Message="refreshToken IsUsed "
                    });
                }
                if(refreshtokenmodel.IsRevoked)
                {
                    return Ok(new ResponseModel{
                        Susscess=false,
                        Message="Refreshtoken IsRevoked "
                    });
                }
            //6.Check AccessToken ID == Jwtid in RefreshToken
            var jwti=tokeninVerification.Claims.FirstOrDefault(i=>i.Type==JwtRegisteredClaimNames.Jti).Value;
                if(refreshtokenmodel.JwtId!=jwti)
                {
                    return Ok(new ResponseModel{
                        Susscess=false,
                        Message="Token not found "
                    });
                }

            //7.update token

            refreshtokenmodel.IsUsed=true;
            refreshtokenmodel.IsRevoked=true;
            Context.RefreshToken.Update(refreshtokenmodel);
            await Context.SaveChangesAsync();

            //8.Create new tokne
            var u=Context.Users.FirstOrDefault(u=>u.Id==refreshtokenmodel.UserId);
            TokenModel token = await GentokenJWT(u);
            //Console.WriteLine($"Token {strtoken}");
            return Ok(new ResponseModel{
                Susscess=true,
                Message="Authenticate Success",
                Data=token
            });            
        }
        catch(Exception ex)
        {
            return Ok(new ResponseModel{
                Susscess=false,
                Message="Request Invalid",
            });
        }
    }

    // private DateTime ConvertUnixTimeToDateTime(long utcdate)
    // {
    //     var  DatetimeInterval=new DateTime(1970,1,1,0,0,0,0,DateTimeKind.Utc);
    //     DatetimeInterval.AddSeconds(utcdate).ToUniversalTime();
    //     return DatetimeInterval;
    // }

    private DateTime ConvertUnixTimeToDateTime(long unixTime)
    {
        DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(unixTime);
        return dateTimeOffset.UtcDateTime;
    }

}