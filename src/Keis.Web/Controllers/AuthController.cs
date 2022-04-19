using Keis.Model;
using Keis.Model.Commands;
using Keis.Model.Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Keis.Web.Controllers;

public class AuthController : MethodController
{
    private readonly SignInManager<AuthUser> signInManager;
    private readonly UserManager<AuthUser> userManager;
    private readonly ILogger<AuthController> logger;
    private readonly IMediator mediatr;
    private readonly SecurityKeyOptions securityKeyOptions;

    public AuthController(
        SignInManager<AuthUser> signInManager
        , UserManager<AuthUser> userManager
        , ILogger<AuthController> logger
        , IMediator mediatr
        , IOptions<SecurityKeyOptions> options)
    {
        this.signInManager = signInManager;
        this.userManager = userManager;
        this.logger = logger;
        this.mediatr = mediatr;
        securityKeyOptions = options.Value;
    }

    [AllowAnonymous]
    [HttpPost("auth.login", Name = nameof(Login))]
    public async Task<IActionResult> Login(LoginCommand command)
    {
        var msg = $"{Request.Method}::{nameof(AuthController)}::{nameof(Login)}";
        logger.LogInformation(msg);

        var user = await userManager.FindByNameAsync(command.UserName);
        if (user is null)
        {
            return NotFound(new ErrorResult { Ok = false, Errors = new[] { $"User '{command.UserName}' not found" } });
        }

        var hasValidPassword = await userManager.CheckPasswordAsync(user, command.Password);
        if (!hasValidPassword)
        {
            return BadRequest(new ErrorResult { Ok = false, Errors = new[] { $"Login failed for user '{command.UserName}'" } });
        }

        var claims = new List<Claim>
        {
            new("id", user.Id),
            new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new (JwtRegisteredClaimNames.Name, user.NormalizedUserName),
            new (JwtRegisteredClaimNames.Email, user.NormalizedEmail)
        };
        var bearerKey = Encoding.UTF8.GetBytes(securityKeyOptions.Bearer);
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(3),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(bearerKey),
                SecurityAlgorithms.HmacSha512Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return Ok(new AuthResult
        {
            Ok = true,
            Token = tokenHandler.WriteToken(token),
            Fullname = user.NormalizedUserName
        });
    }
}