using API.Data;
using API.Entities;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    public class UsersController: BaseApiController
    { 
      private readonly DataContext _context;

      public UsersController(DataContext context)
      {
        _context=context;
      }  

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
    {
        return await _context.Users.ToListAsync();
    }
//public ActionResult<IEnumerable<AppUser>> GetUsers()
  //  {
    //    return _context.Users.ToList();
    //}



    //api/users/3
    [Authorize]
    [HttpGet("{id}")]
    //public ActionResult<AppUser> GetUsers(int id)
    //{
        //var user = _context.Users.Find(id);
        //return user;
      //  return _context.Users.Find(id);
    //}

    public async Task<ActionResult<AppUser>> GetUsers(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    }
}