using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoList.Models;
using ToDoList.Models.Account;
using CryptoHelper;
using Microsoft.AspNetCore.Authorization;

namespace ToDoList.Controllers
{
    [Produces("application/json")]
    [Route("api/Account/[action]")]
    public class AccountController : Controller
    {
        private readonly todolistContext _context;

        public AccountController(todolistContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Unauthorized(string message)
        {
            return Unauthorized();
        }
        [HttpGet, Authorize]
        public IActionResult getUser()
        {
            var user = new { id = User.FindFirst("Id").Value, name = User.FindFirst("Name").Value, email = User.FindFirst("Email").Value };
            return Ok(user);
        }
        [HttpPost]
        public async Task<IActionResult> Login([FromBody]Login login)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            if (await LoginUser(login.Email, login.Password))
                return Ok();
            return BadRequest("信箱或密碼錯誤!");
        }
        private async Task<bool> LoginUser(string Email, string password)
        {
            User user = await _context.User.SingleOrDefaultAsync(m => m.Email == Email);
            //找不到email或密碼錯誤
            if (user == null || !Crypto.VerifyHashedPassword(user.Password, password))
                return false;

            addClaims(user);

            return true;
        }
        //讓之後已登入的用戶能從加密cookie取資料User.FindFirst("Email").Value
        private async void addClaims(User user){
            var claims = new List<Claim>();
            claims.Add(new Claim("Id", Convert.ToString(user.Id)));
            claims.Add(new Claim("Name", user.Name));
            claims.Add(new Claim("Email", user.Email));

            var identity = new ClaimsIdentity(claims, "login");

            ClaimsPrincipal principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(principal);
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Ok("已登出");
        }
        [HttpPost]
        public async Task<IActionResult> Signup([FromBody] Signup signup)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            User user = await _context.User.SingleOrDefaultAsync(m => m.Email == signup.Email);
            if(user != null)
                return BadRequest("已有相同email註冊!");

            user = new User() { Name=signup.Name, Email=signup.Email };
            user.Password = Crypto.HashPassword(signup.Password);
            _context.User.Add(user);
            await _context.SaveChangesAsync();

            addClaims(user);

            return Ok();
        }
    }
}