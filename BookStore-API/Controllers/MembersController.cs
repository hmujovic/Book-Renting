using AutoMapper;
using BookStore_API.Contracts;
using BookStore_API.DTOs;
using BookStore_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Bookstore_API.Controllers
{
	/// <summary>
	/// Interacts with the Members Table
	/// </summary>
	[Route("api/[controller]/")]
	[ApiController]
	public class MembersController : ControllerBase
	{
		private readonly IMemberRepository _MemberRepository;
		private readonly IMapper _mapper;

		public MembersController(IMemberRepository MemberRepository,
			IMapper mapper)
		{
			_MemberRepository = MemberRepository;
			_mapper = mapper;
		}

		/// <summary>
		/// Get All Members
		/// </summary>
		/// <returns>A list of Members</returns>
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> GetMembers()
		{
			try
			{
				var Members = await _MemberRepository.FindAll();
				return Ok(Members);
			}
			catch (Exception e)
			{
				return StatusCode(500, e.Message);
			}
		}

		/// <summary>
		/// Gets a Member by Username
		/// </summary>
		/// <param name="username"></param>
		/// <returns>A Member record</returns>
		[HttpGet("{username}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> GetMember(string username)
		{
			try
			{
				var Member = await _MemberRepository.FindByUsername(username);
				if (Member == null)
				{
					return NotFound();
				}
				return Ok(Member);
			}

			catch (Exception e)
			{
				return StatusCode(500, e.Message);
			}
		}

		/// <summary>
		/// Create a Member
		/// </summary>
		/// <param name="Member"></param>
		/// <returns></returns>
		[HttpPost]
		[Authorize(Roles = "Librarian")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Create([FromBody] Member MemberDto)
		{
			try
			{
				if (MemberDto == null)
				{
					return BadRequest(ModelState);
				}
				if (!ModelState.IsValid)
				{
					return BadRequest(ModelState);
				}
				var Member = _mapper.Map<Member>(MemberDto);
				var isSuccess = await _MemberRepository.Create(Member);
				if (!isSuccess)
				{
					return StatusCode(500);
				}
				return Created("Create", new { Member });
			}
			catch (Exception e)
			{
				return StatusCode(500, e.Message);
			}
		}

		/// <summary>
		/// Update an Member
		/// </summary>
		/// <param name="id"></param>
		/// <param name="Member"></param>
		/// <returns></returns>
		[HttpPut("{id}")]
		[Authorize(Roles = "Librarian")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Update(int id, [FromBody] Member MemberDto)
		{
			try
			{
				if (id < 1 || MemberDto == null)
				{
					return BadRequest();
				}
				var isExists = await _MemberRepository.isExists(id);
				if (!isExists)
				{
					return NotFound();
				}
				if (!ModelState.IsValid)
				{
					return BadRequest(ModelState);
				}
				var Member = _mapper.Map<Member>(MemberDto);
				Member.Id = id;
				var isSuccess = await _MemberRepository.Update(Member);
				if (!isSuccess)
				{
					return StatusCode(500);
				}
				return NoContent();
			}
			catch (Exception e)
			{
				return StatusCode(500, e.Message);
			}
		}
	}
}
