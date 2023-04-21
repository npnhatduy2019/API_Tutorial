using System.Text;
using API_Tutorial.Models;
using API_Tutorial.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<MyDbContext>(option=>{
    option.UseSqlServer(builder.Configuration.GetConnectionString("EntityDB"));
});

builder.Services.AddScoped<ICategoryRepositories,CategoryRepository>();
builder.Services.AddScoped<IProductRepository,ProductRepository>();

builder.Services.Configure<AppSetting>(builder.Configuration.GetSection("AppSettings"));
var SecretKey=builder.Configuration["AppSettings:Secretkey"];
var SecretBytes=Encoding.UTF8.GetBytes(SecretKey);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt=>{
    opt.TokenValidationParameters=new TokenValidationParameters{
        //tự cấp token
        ValidateIssuer=false,
        ValidateAudience=false,

        //ký vào token
        ValidateIssuerSigningKey=true,
        IssuerSigningKey = new SymmetricSecurityKey(SecretBytes),

        ClockSkew=TimeSpan.Zero,
    };
});


var app = builder.Build();

// builder.Services.AddDbContext<APIDBContext>(options=>{
//     options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnectstring"));
// });

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
