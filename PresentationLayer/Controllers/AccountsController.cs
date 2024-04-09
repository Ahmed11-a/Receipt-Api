using BusinessLayer.DTO;
using BusinessLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace PresentationLayer.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountsController : ControllerBase
	{
		private readonly UserManager<Client> _userManager;
		private readonly IConfiguration _configuration;

		public AccountsController(UserManager<Client> userManager, IConfiguration configuration)
		{
			_userManager = userManager;
			_configuration = configuration;
		}

		// Register a new account
		[HttpPost("[action]")]
		public async Task<IActionResult> Register(RegisterDTO accountDTO)
		{
			// Check if the provided model state is valid
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			// Create a new Client instance with the provided account details
			var client = new Client
			{
				UserName = accountDTO.Name,
				PhoneNumber = accountDTO.Phone,
				CreatedAt = DateTime.Now,
			};

			// Attempt to create the user using the provided information
			var result = await _userManager.CreateAsync(client, accountDTO.Password);
			if (result.Succeeded)
			{
				return Ok($"Successful Registration for Account Id: {client.Id} !");
			}

			// If user creation fails, add errors to model state and return bad request
			foreach (var error in result.Errors)
			{
				ModelState.AddModelError("", error.Description);
			}

			return BadRequest(ModelState);
		}

		// Login to an existing account
		[HttpPost("[action]")]
		public async Task<IActionResult> Login(LoginDTO loginDTO)
		{
			// Check if the provided model state is valid
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			// Find the user by their username
			var client = await _userManager.FindByNameAsync(loginDTO.Name);
			if (client == null)
			{
				ModelState.AddModelError("", "User name is not found");
				return BadRequest(ModelState);
			}

			// Check if the provided password matches the user's password
			if (!await _userManager.CheckPasswordAsync(client, loginDTO.Password))
			{
				return Unauthorized();
			}

			// Create claims for the authenticated user
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, client.UserName),
				new Claim(ClaimTypes.NameIdentifier, client.Id.ToString()),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
			};

			var roles = await _userManager.GetRolesAsync(client);
			foreach (var role in roles)
			{
				claims.Add(new Claim(ClaimTypes.Role, role));
			}

			// Generate JWT token with claims
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecratKey"]));
			var signingCredential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
			var token = new JwtSecurityToken
			(
				claims: claims,
				issuer: _configuration["JWT:Issuer"],
				audience: _configuration["JWT:Audience"],
				expires: DateTime.Now.AddHours(2),
				signingCredentials: signingCredential
			);

			// Generate response token containing JWT token and expiration date
			var responseToken = new
			{
				token = new JwtSecurityTokenHandler().WriteToken(token),
				expires = token.ValidTo
			};

			return Ok(responseToken);
		}
	}
}
