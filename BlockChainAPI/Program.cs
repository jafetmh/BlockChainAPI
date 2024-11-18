using BlockChain_DB;
using BlockChainAPI;
using BlockChainAPI.Services.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
    In = ParameterLocation.Header,
    Description = "Ingrese el token JWT en formato 'Bearer {token}'",
    Name = "Authorization",
    Type = SecuritySchemeType.ApiKey
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] {}
            }
    });
});


//database context dependencie ijection
builder.Services.AddDbContext<BlockChainContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BlockChain_Connection"))
);

//service injection
builder.Services.AddDataAccesServices();
builder.Services.AddAppServices();

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
app.UseAuthentication();//needed to accept authentication
app.UseAuthorization();

app.MapControllers();

app.Run();
