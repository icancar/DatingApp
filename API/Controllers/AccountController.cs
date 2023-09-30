using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Http.Connections;
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

        [HttpPost("login")]
        public async Task<ActionResult<AppUser>> Login(LoginDTO loginDTO) {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == loginDTO.username);

            if (user == null) {
                return Unauthorized("Invalid Username!");
            }

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.password));

            for (int i = 0; i < computedHash.Length; i++) {
                if (computedHash[i] != user.PasswordHash[i]) {
                    return Unauthorized("Invalid Password");
                }
            }

            return user;

        }
        
    }

}