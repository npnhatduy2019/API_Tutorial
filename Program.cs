using API_Tutorial.Models;
using API_Tutorial.Repository;
using Microsoft.EntityFrameworkCore;

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

app.UseAuthorization();

app.MapControllers();

app.Run();
