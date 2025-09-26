using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using AOL_Portal.Configuration;
using AOL_Portal.Data;
using AOL_Portal.Services;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);
ApiKeyAttribute.Configure(builder.Configuration);

var services = builder.Services;
var culture = CultureInfo.CreateSpecificCulture("en-ZA");
var dateformat = new DateTimeFormatInfo { ShortDatePattern = "yyyy/MM/dd", LongDatePattern = "yyyy/MM/dd hh:mm:ss tt" };
culture.DateTimeFormat = dateformat;
Thread.CurrentThread.CurrentCulture = culture;
Thread.CurrentThread.CurrentUICulture = culture;

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS for Angular client
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularClient", policy =>
    {
        policy.SetIsOriginAllowed(origin => true) // Allow any origin in development
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

services.Configure<ApplicationConfig>(builder.Configuration.GetSection("ApplicationConfig"));
services.Configure<EmailConfig>(builder.Configuration.GetSection("Email"));
services.AddTransient<ApplicationConfigService>();
services.AddDbContext<AOLContext>(option =>
         option.UseSqlServer(builder.Configuration.GetConnectionString("AOLPortalConnection")));
builder.Services.AddScoped<IUserClaimsPrincipalFactory<AolApplicationUser>, ApplicationUserClaimsPrincipalFactory>();

// Register JWT Service
builder.Services.AddScoped<IJwtService, JwtService>();

services.AddIdentity<AolApplicationUser, AolUserRole>(
        options =>
        {
            options.User.AllowedUserNameCharacters =
                  "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            options.User.RequireUniqueEmail = true;
            options.Password.RequiredLength = 8;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireDigit = true;
            options.SignIn.RequireConfirmedAccount = false;
            options.SignIn.RequireConfirmedEmail = false;
            options.SignIn.RequireConfirmedPhoneNumber = false;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;
        }
    ).AddEntityFrameworkStores<AOLContext>()
    .AddDefaultTokenProviders();    

// Configure JWT Authentication
var apiConfig = builder.Configuration.GetSection("ApiService").Get<ApiConfig>();
var jwtSecret = Encoding.ASCII.GetBytes(apiConfig?.JwtSecret ?? "DefaultSecretKeyForDevelopmentOnly");

services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(jwtSecret),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
    
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
            {
                context.Response.Headers.Add("Token-Expired", "true");
            }
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddScoped<IEmailService, EmailService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//using (var scope = app.Services.CreateScope())
//{
//    // Uncomment to create default users - start twice for user to be authenticated
//    var usermanager = scope.ServiceProvider.GetRequiredService<UserManager<AolApplicationUser>>();
//    var usercreations = new TestUserSeed(usermanager);
//    await usercreations.CreateDefaultUser();
//}

app.UseHttpsRedirection();

// Use CORS
app.UseCors("AllowAngularClient");

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

