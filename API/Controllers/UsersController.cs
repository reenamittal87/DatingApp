using API.Data;
using Microsoft.AspNetCore.Http;
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
using System.Security.Claims;
using API.Extensions;


using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.Extensions.Options;
using API.Helpers;

namespace API.Controllers
{
    //[Authorize]  Commented for the moment as the SSL not working
    public class UsersController: BaseApiController
    { 
      private readonly IUserRepository _userRepository;
      private readonly IPhotoService _photoService;
      private readonly IMapper _mapper;

      public UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService)
      {
        _userRepository = userRepository;
        _photoService = photoService;
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
    [HttpGet("{username}", Name = "GetUser")]
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

    [HttpPost("add-photo")]
    public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
    {
         var user = _userRepository.GetUserByUsernameAsync(User.GetUserName());
         var result = _photoService.AddPhotoAsync(file);

         //if(result.Error != null) return BadRequest(result.Error.Message); Need to fix this

        /*var photo = new Photo 
        {
          Url = result.SecureUrl.AbsoluteUri,
          PublicId = result.PublicId
        };

        if(user.Photos.Count == 0)
        {
          photo.IsMain = true;
        }

        user.Photos.Add(photo);

        if(await _userRepository.SaveAllAsync())
        {
          return CreatedAtRoute("GetUser", new {username = user.UserName}, _mapper.Map<PhotoDto>(photo));
        }  */
        /* if(await _userRepository.SaveAllAsync()){ 
        return _mapper.Map<PhotoDto>(photo);
        } */

         return BadRequest("Problem adding photo"); 
    }

    [HttpDelete("delete-photo/{photoId}")]
    public async Task<ActionResult> DeletePhoto(int photoId){
      var user = _userRepository.GetUserByUsernameAsync(User.GetUserName());
      var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);
      if(photo == null) return NotFound();

      if(photo.IsMain) return BadRequest("You cannot delete your main photo");

      if(photo.PublicId != null){
        var result = await _photoService.DeletePhotoAsync(photo.PublicId);
        if(result.Error!= null) return BadRequest(result.Error.Message);
      }
      user.Photos.Remove(photo);
      if(await _userRepository.SaveAllAsync()) return Ok();
      return BadRequest("Failed to set main photo");
    }



    [HttpPut("set-main-photo/{photoId}")]
    public async Task<ActionResult> SetMainPhoto(int photoId)
    {
      var user = _userRepository.GetUserByUsernameAsync(User.GetUserName());
      var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);
      if(photo.IsMain) return BadRequest("This is already your main photo");

      var currentMain = user.Photos.FirstOrDefault(x =>x.IsMain);
      if(currentMain!= null) currentMain.IsMain = false;
      photo.IsMain = true;

      if(await _userRepository.SaveAllAsync()) return NoContent();

      return BadRequest("Failed to set main photo");
    }

    [HttpPut]
    public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto){
        //var username = User.GetUserName();
        //var user = await _userRepository.GetUserByUsernameAsync(username);
        var user = await _userRepository.GetUserByUsernameAsync(User.GetUserName());

        _mapper.Map(memberUpdateDto, user);
        _userRepository.Update(user);

        if(await _userRepository.SaveAllAsync()) return NoContent();
        return BadRequest("Failed to update user");
    }

    }
}