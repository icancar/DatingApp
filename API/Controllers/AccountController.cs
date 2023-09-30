using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using SQLitePCL;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        public DataContext _context;
        public AccountController(DataContext context) {
            _context = context;
        }

        [HttpPost("register")] //POST api/account/register
        public async Task<ActionResult<AppUser>> Register(string username, string password) {
            using var hmac = new HMACSHA512(); //will use this as passwordSalt

            var user = new AppUser {
                UserName = username,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)),
                PasswordSalt = hmac.Key
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            
            return user;
        }
        
    }

}