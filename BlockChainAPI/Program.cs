using BlockChain_DB;
using BlockChainAPI;
using BlockChainAPI.Interfaces;
using BlockChainAPI.Interfaces.IDataService;
using BlockChainAPI.Services;
using BlockChainAPI.Services.Auth;
using BlockChainAPI.Utilities.ResponseMessage;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//this main equals java

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//database context dependencie ijection
builder.Services.AddDbContext<BlockChainContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BlockChain_Connection"))
);

//service injection
builder.Services.AddCRUDServices();
builder.Services.AddHelperServices();

//JWT Config
builder.Services.AddJWTConfig(builder.Configuration);

//CORS Config
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactPolicy", builder =>
         builder.WithOrigins("http://localhost:3000")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());
});

var app = builder.Build();

//Create DB on start this proyect
//using (var scope = app.Services.CreateScope())
//{
//    var context = scope.ServiceProvider.GetRequiredService<BlockChainContext>();
//    context.Database.Migrate();
//}


// Configure the HTTP request pipeline...
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("ReactPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
