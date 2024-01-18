using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using bookStoreProject.DBEFModels;
using BookStoreProject.Models;
using BookStoreProject.DBEFModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

// Add services to the container.


// Adding a Database Context
builder.Services.AddDbContext<BookDBContext>(options => options.UseSqlServer(
                builder.Configuration.GetConnectionString("BookDbConnection")));




// Identity
// This line adds the IdentityDbContext to the dependency injection container.
builder.Services.AddDbContext<BookStoreProject.DBEFModels.IdentityDbContext>(options => options.UseSqlServer(
                builder.Configuration.GetConnectionString("IdentityDbConnection")));

// AddDefaultIdentity configures the default Identity services
builder.Services.AddDefaultIdentity<ApiUser>()//options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    //  This configures Entity Framework as the storage provider for the Identity system.
    //  It specifies that the IdentityDbContext will be used to store Identity-related data.
    .AddEntityFrameworkStores<BookStoreProject.DBEFModels.IdentityDbContext>();

// Adding Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})

// Adding Jwt Bearer authentication handler
// Specify the configuration for validating JWT tokens.
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    // Specify the parameters for validating the received JWT token
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = configuration["JWT:ValidAudience"],
        ValidIssuer = configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
    };
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter 'Bearer' followed by the token",
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
            Array.Empty<string>()
        }
    });
});



var app = builder.Build();

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
