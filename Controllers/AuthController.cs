using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyKoloApi.Data;
using MyKoloApi.DTOS;
using MyKoloApi.Features.Security;
using MyKoloApi.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyKoloApi.Controllers
{
    [ApiController]
    public class AuthController:ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;
        private  IClaimsManager _claimsManager;


        public AuthController(IConfiguration configuration , AppDbContext context, IClaimsManager claimsManager)
        {
            _configuration = configuration;
            _context = context;
            _claimsManager = claimsManager;
        }


        [HttpPost]
        [Route("register")]
        public IActionResult Register(RegisterForm registerForm)
        {

            User userToAdd = new User()
            {
                Id= Guid.NewGuid().ToString(),
                Email = registerForm.Email,
                EncryptedPassword = SimpleStringCryptographer.EnryptString(registerForm.Password),
                UserName = registerForm.UserName
            };

            _context.Users.Add(userToAdd);
            _context.SaveChanges();

            return Ok("User Registered Successfully");
        }



        [HttpPost]
        [Route("login")]
        public IActionResult GetAccessToken([FromBody] LoginForm loginForm)
        {
            // for a user centric client , 
            // verify email and password ,
            // once verified , you can then send back a token.
            User foundUser = _context.Users.FirstOrDefault(user => user.Email.ToLower() == loginForm.Email.ToLower());
            if(foundUser != null)
            {
                bool passWordIsValid = SimpleStringCryptographer.EnryptString(loginForm.Password) 
                    == foundUser.EncryptedPassword ? true : false;
                if (!passWordIsValid) return Unauthorized("Invalid Email or Password");
                else
                {
                    List<Claim> claims = new List<Claim>() { 
                        new Claim("Email",foundUser.Email)
                    };
                    JwtSecurityToken token = null;
                    if (_claimsManager.SaveClaimsToDatabase(claims,foundUser.Id) == true)
                    {
                        token = new JwtSecurityToken(
                        issuer: _configuration["JWT:ValidIssuer"],
                        audience: _configuration["JWT:ValidAudience"],
                        expires: DateTime.Now.AddMinutes(5),
                        claims:claims,
                        signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SigningSecret"])), SecurityAlgorithms.HmacSha256Signature)
                    );

                    
                    }
                     return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                    
                
                
                
                }


            }
            else
            {
                return Unauthorized("Invalid Email or Password");
            }

            // we can move this into another class 

        }





    
    }
}
