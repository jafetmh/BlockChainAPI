using BlockChain_DB;
using BlockChainAPI.Interfaces;
using BlockChainAPI.Services;
using BlockChainAPI.Services.Auth;
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

//service inyections
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IMemPoolDocumentService, MemPoolDocumentService>();
builder.Services.AddScoped<IConfigurationService, ConfigurationService>();
builder.Services.AddTransient<IAuthService, AuthService>();

//Add database context for dependencies ijection
builder.Services.AddDbContext<BlockChainContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BlockChain_Connection"))
);



//JWT Config
builder.Services.AddAuthentication("Bearer").AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))

    };
});

//CORS Config
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactPolicy", builder =>
        builder.WithOrigins("http://localhost:3000")//React App URL (https)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());
});

var app = builder.Build();

//Create DB on start this proyect
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<BlockChainContext>();
    context.Database.Migrate();
}


// Configure the HTTP request pipeline.
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
