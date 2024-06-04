using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Themgico.Authentication;
using Themgico.Entities;
using Themgico.Properties;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Themgico.Service.Interface;
using Themgico.Service;
using Themgico.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = ".NET-Core-UserManagement-API-8.0",
        Version = "v1",
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
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
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                },
                });
});

// Configure AppSettings section
builder.Services.Configure<AppSetting>(builder.Configuration.GetSection("AppSettings"));
builder.Services.AddDbContext<ThemgicoContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString);
});

// Configure JWT authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true, // Enable validation for the issuer
            ValidateAudience = true, // Enable validation for the audience
            ValidAudience = builder.Configuration["JWT:ValidAudience"],
            ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
        };
        options.Events = new ApplicationJwtBearEvents();
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("admin"));
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<INewsService, NewsService>();
builder.Services.AddTransient<ICategoryService, CategoryService>();

var app = builder.Build();

//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI(c =>
//    {
//        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
//        c.RoutePrefix = string.Empty;
//    });
//}
app.MapControllers();

app.Run();