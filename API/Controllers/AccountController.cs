using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<ActionResult<AppUser>> Register(RegisterDTO registerDTO) {

            if (await UserExists(registerDTO.Username.ToLower())){
                return BadRequest("Username is taken!");
            }

            using var hmac = new HMACSHA512(); //will use this as passwordSalt

            var user = new AppUser {
                UserName = registerDTO.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.Password)),
                PasswordSalt = hmac.Key
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            
            return user;
        }

        public async Task<bool> UserExists(string username) {
            return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
        
    }

}