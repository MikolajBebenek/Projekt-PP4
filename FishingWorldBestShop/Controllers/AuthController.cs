using FishingWorldEShop.Models;
using FishingWorldEShop.Models.Auth;
using FishingWorldEShop.Repositories;
using FishingWorldEShop.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly FishingWorldEShopContext _context;
    private readonly IPasswordHasher<Customer> _passwordHasher;
    private readonly JwtService _jwt;
    public AuthController(
        FishingWorldEShopContext context,
        IPasswordHasher<Customer> passwordHasher,
        JwtService jwt)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _jwt = jwt;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        if (await _context.Customers.AnyAsync(c => c.Email == request.Email))
            return BadRequest("Użytkownik o takim e-mailu już istnieje.");

        var customer = new Customer
        {
            Name = request.Name,
            Surname = request.Surname,
            Email = request.Email,
            Phone = request.Phone,
            Address = request.Address
        };

        customer.PasswordHash = _passwordHasher.HashPassword(customer, request.Password);
        await _context.Customers.AddAsync(customer);
        await _context.SaveChangesAsync();

        return Ok("Zarejestrowano pomyślnie.");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var user = await _context.Customers.SingleOrDefaultAsync(c => c.Email == request.Email);
        if (user == null)
            return Unauthorized("Niepoprawny login lub hasło.");

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if (result == PasswordVerificationResult.Failed)
            return Unauthorized("Niepoprawny login lub hasło.");

        var token = _jwt.GenerateToken(user);

        return Ok(new
        {
            message = "Zalogowano pomyślnie",
            token
        });
    }


    [HttpPost("reset")]
    public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
    {
        var user = await _context.Customers.SingleOrDefaultAsync(c => c.Email == request.Email);
        if (user == null)
            return NotFound("Nie znaleziono użytkownika.");

        user.PasswordHash = _passwordHasher.HashPassword(user, request.NewPassword);
        await _context.SaveChangesAsync();

        return Ok("Hasło zostało zmienione.");
    }


    [HttpGet("me")]
    [Authorize]
    public IActionResult Me()
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        if (identity == null || !identity.IsAuthenticated)
            return Unauthorized("Brak autoryzacji");

        var claims = identity.Claims.Select(c => new { c.Type, c.Value });
        return Ok(claims);
    }

}
