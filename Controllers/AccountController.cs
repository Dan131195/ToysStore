using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ToysStore.Data;
using ToysStore.DTOs.Account;
using ToysStore.Models;
using ToysStore.Models.Auth;
using ToysStore.Services;
using ToysStore.Settings;

namespace ToysStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly Jwt _jwtSettings;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly EmailService _emailService; // !!! !!! !!! - To Do : Migliorare grafica delle email di benvenuto e login
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<AccountController> _logger;


        public AccountController(IOptions<Jwt> jwtOptions, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager, EmailService emailService, ApplicationDbContext dbContext, ILogger<AccountController> logger)
        {
            _jwtSettings = jwtOptions.Value;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailService = emailService;
            _dbContext = dbContext;
            _logger = logger;
        }

        // - REGISTRAZIONE
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            try
            {
                var existingUser = await _userManager.FindByEmailAsync(registerRequestDto.Email);
                if (existingUser != null)
                {
                    return BadRequest(new { Message = "Email già registrata." });
                }

                var newUser = new ApplicationUser
                {
                    Email = registerRequestDto.Email,
                    UserName = registerRequestDto.Email,
                    FirstName = registerRequestDto.FirstName,
                    LastName = registerRequestDto.LastName
                };

                var result = await _userManager.CreateAsync(newUser, registerRequestDto.Password);
                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description).ToList();
                    return BadRequest(new { Errors = errors });
                }

                var roleExists = await _roleManager.RoleExistsAsync(registerRequestDto.Ruolo);
                if (!roleExists)
                {
                    return BadRequest(new { Message = $"Il ruolo '{registerRequestDto.Ruolo}' non esiste." });
                }

                await _userManager.AddToRoleAsync(newUser, registerRequestDto.Ruolo);

                Guid? utenteId = null;

                if (registerRequestDto.Ruolo == "User")
                {
                    var utente = new Utente
                    {
                        UtenteId = Guid.NewGuid(),
                        Nickname = registerRequestDto.Email,
                        UserId = newUser.Id
                    };

                    _dbContext.Utenti.Add(utente);

                    await _dbContext.SaveChangesAsync();

                    utenteId = utente.UtenteId;
                }

                // - To Do: EMAIL DI BENVENUTO MIgliorare la grafica dell'email di benvenuto

                var subject = "Benvenuto su ToysStore!";
                var body = $"Ciao {newUser.FirstName},<br><br>Benvenuto nel nostro Marketplace di giocattoli! Siamo felici di averti con noi.";
                await _emailService.SendEmailAsync(newUser.Email, subject, body);

                _logger.LogInformation($"Utente {newUser.Email} registrato con successo.");

                return Ok(new RegisterResponseDto
                {
                    UserId = newUser.Id,
                    Email = newUser.Email,
                    Nome = newUser.FirstName,
                    Cognome = newUser.LastName,
                    Ruolo = registerRequestDto.Ruolo,
                    UtenteId = utenteId
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante la registrazione.");
                return BadRequest(ex.Message);
            }
        }

        // - LOGIN
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequestDto loginRequestDto)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(loginRequestDto.Email);

                if (user == null)
                {
                    return Unauthorized("Invalid email or password.");
                }

                var signInResult = await _signInManager.PasswordSignInAsync(user, loginRequestDto.Password, false, false);

                if (!signInResult.Succeeded)
                {
                    return Unauthorized("Invalid email or password.");
                }

                var roles = await _signInManager.UserManager.GetRolesAsync(user);

                List<Claim> claims = new List<Claim>();

                claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                claims.Add(new Claim(ClaimTypes.Email, user.Email));
                claims.Add(new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"));
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecurityKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var expiry = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiresInMinutes);
                var token = new JwtSecurityToken(_jwtSettings.Issuer, _jwtSettings.Audience, claims, expires: expiry, signingCredentials: creds);

                string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                // - To Do: EMAIL DI LOGIN EFFETTUATO  MIgliorare la grafica dell'email di login

                var loginSubject = "Nuovo accesso al tuo account";
                var loginBody = $"Ciao {user.FirstName}, abbiamo rilevato un nuovo accesso al tuo account ToysStore in data {DateTime.Now}. Se non sei stato tu, cambia subito la password.";
                await _emailService.SendEmailAsync(user.Email, loginSubject, loginBody);

                _logger.LogInformation($"Login dell'utente {user.FirstName} avvenuto con successo.");

                return Ok(new TokenResponse
                {
                    Token = tokenString,
                    Expires = expiry,
                    UserId = user.Id,
                    Email = user.Email,
                    Roles = roles.ToList()
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Errore durante il login.");
                return BadRequest(ex.Message);
            }

        }
    }
}
