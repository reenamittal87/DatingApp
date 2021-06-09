using API.Data;
using API.Entities;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using API.Interfaces;
using API.DTOs;
using AutoMapper;

namespace API.Controllers
{
    [Authorize]  
    public class UsersController: BaseApiController
    { 
      private readonly IUserRepository _userRepository;
      private readonly IMapper _mapper;

      public UsersController(IUserRepository userRepository, IMapper mapper)
      {
        _userRepository=userRepository;
        _mapper=mapper;
      }  

    [HttpGet]
    //[AllowAnonymous]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
    {
        //var users = await _userRepository.GetUsersAsync();  
        //var usersToReturn = _mapper.Map<IEnumerable<MemberDto>>(users); 
        // return Ok(usersToReturn);
        var users = await _userRepository.GetMembersAsync();

        return Ok(users);
    }


    //api/users/3
    //[Authorize]
    [HttpGet("{username}")]
    //public ActionResult<AppUser> GetUsers(int id)
    //{
        //var user = _context.Users.Find(id);
        //return user;
      //  return _context.Users.Find(id);
    //}

    public async Task<ActionResult<MemberDto>> GetUser(string username)
    {
        //var user = await _userRepository.GetUserByUsernameAsync(username);
        //return _mapper.Map<MemberDto>(user);
        return await _userRepository.GetMemberAsync(username);
    }

    }
}