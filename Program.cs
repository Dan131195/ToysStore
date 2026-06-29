using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using ToysStore.Data;
using ToysStore.Models;
using ToysStore.Models.Auth;
using ToysStore.Services;
using ToysStore.Settings;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
    .MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Async(a => a.File("Log/log_txt", rollingInterval: RollingInterval.Day))
    .WriteTo.Async(a => a.Console())
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.Configure<Jwt>(builder.Configuration.GetSection(nameof(Jwt)));

    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
    {
        options.SignIn.RequireConfirmedAccount =
            builder.Configuration.GetSection("Identity").GetValue<bool>("RequireConfirmedAccount");

        options.Password.RequiredLength =
            builder.Configuration.GetSection("Identity").GetValue<int>("RequiredLength");

        options.Password.RequireDigit =
            builder.Configuration.GetSection("Identity").GetValue<bool>("RequireDigit");

        options.Password.RequireLowercase =
            builder.Configuration.GetSection("Identity").GetValue<bool>("RequireLowercase");

        options.Password.RequireNonAlphanumeric =
            builder.Configuration.GetSection("Identity").GetValue<bool>("RequireNonAlphanumeric");

        options.Password.RequireUppercase =
            builder.Configuration.GetSection("Identity").GetValue<bool>("RequireUppercase");
    })
           .AddEntityFrameworkStores<ApplicationDbContext>()
           .AddDefaultTokenProviders();

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration.GetSection(nameof(Jwt)).GetValue<string>("Issuer"),
                ValidAudience = builder.Configuration.GetSection(nameof(Jwt)).GetValue<string>("Audience"),
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(builder.Configuration.GetSection(nameof(Jwt)).GetValue<string>("SecurityKey"))
                )
            };
        });
    builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));

    builder.Services.AddScoped<EmailService>();
    builder.Services.AddScoped<ProdottoService>();
    builder.Services.AddScoped<UtenteService>();

    builder.Host.UseSerilog();

    var app = builder.Build();

    using (var scope = app.Services.CreateScope())
    {
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

        string[] ruoliBase = { "User", "Admin" };

        foreach (var ruolo in ruoliBase)
        {
            if (!await roleManager.RoleExistsAsync(ruolo))
            {
                await roleManager.CreateAsync(new ApplicationRole { Name = ruolo });
            }
        }
    }

    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<ApplicationDbContext>();

            context.Database.Migrate();
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "Si è verificato un errore durante l'aggiornamento o il seeding del database.");
        }
    }

    app.UseCors(x =>
          x.AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader()
       );

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseStaticFiles();

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();

}
catch (Exception ex)
{
    Log.Error(ex, "Errore fatale all'avvio");
}
finally
{
    await Log.CloseAndFlushAsync();
}
