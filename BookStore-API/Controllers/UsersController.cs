using BookStore_API.Contracts;
using BookStore_API.DTOs;
using BookStore_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BookStore_API.Controllers
{
	[Route("api/[controller]/")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly SignInManager<IdentityUser> _signInManager;
		private readonly UserManager<IdentityUser> _userManager;
		private readonly IConfiguration _config;
		private readonly IMemberRepository _memberRepository;

		public UsersController(SignInManager<IdentityUser> signInManager,
			UserManager<IdentityUser> userManager,
			IConfiguration config,
			IMemberRepository memberRepository)
		{
			_signInManager = signInManager;
			_userManager = userManager;
			_config = config;
			_memberRepository = memberRepository;
		}

		/// <summary>
		/// User Register Endpoint
		/// </summary>
		/// <param name="userDto"></param>
		/// <returns></returns>
		[Route("register")]
		[HttpPost]
		public async Task<IActionResult> Register([FromBody] UserDto userDto)
		{
			try
			{
				var username = userDto.EmailAddress;
				var password = userDto.Password;
				var user = new IdentityUser { Email = username, UserName = username };
				var result = await _userManager.CreateAsync(user, password);

				if (!result.Succeeded)
				{
					return StatusCode(500, "Something went wrong. Please contact the Administrator.");
				}
				await _userManager.AddToRoleAsync(user, "Member");
				var member = new Member();
				member.Name = username;

				await _memberRepository.Create(member);

				return Created("login", new { result.Succeeded });
			}
			catch (Exception e)
			{
				return StatusCode(500, "Something went wrong. Please contact the Administrator.");
			}
		}

		/// <summary>
		/// User Login Endpoint
		/// </summary>
		/// <param name="userDto"></param>
		/// <returns></returns>
		[Route("login")]
		[AllowAnonymous]
		[HttpPost]
		public async Task<IActionResult> Login([FromBody] UserDto userDto)
		{
			try
			{
				var username = userDto.EmailAddress;
				var password = userDto.Password;
				var result = await _signInManager.PasswordSignInAsync(username, password, false, false);

				if (result != null)
				{
					var user = await _userManager.FindByEmailAsync(username);
					var tokenString = await GenerateJSONWebToken(user);
					return Ok(new { token = tokenString });
				}
				return Unauthorized(userDto);
			}
			catch (Exception e)
			{
				return StatusCode(500, "Something went wrong. Please contact the Administrator.");
			}
		}

		private async Task<string> GenerateJSONWebToken(IdentityUser user)
		{
			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
			var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
			var claims = new List<Claim>
			{
				new Claim(JwtRegisteredClaimNames.Sub, user.Email),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim(ClaimTypes.NameIdentifier, user.Id)
			};
			var roles = await _userManager.GetRolesAsync(user);
			claims.AddRange(roles.Select(r => new Claim(ClaimsIdentity.DefaultRoleClaimType, r)));

			var token = new JwtSecurityToken(_config["Jwt:Issuer"]
				, _config["Jwt:Issuer"],
				claims,
				null,
				expires: DateTime.Now.AddHours(5),
				signingCredentials: credentials
			);
			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}