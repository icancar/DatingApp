using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")] //https://localhost:5001/api/users
    public class UsersController : ControllerBase
    {
        private readonly DataContext _dataContext;
        public UsersController(DataContext context)
        {
            _dataContext = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            var users = await _dataContext.Users.ToListAsync();
            return users;
        }

        [HttpGet("{id}")] //api/users/2 
        public async Task<ActionResult<AppUser>> GetUser(int id)
        {
            var user = await _dataContext.Users.FindAsync(id);

            return user;
        }
    }
}