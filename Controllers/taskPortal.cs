using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using internalTools.Model;
using internalTools.wwwroot;
using System.Data;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Configuration;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace internalTools.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class taskPortal : Controller
    {
        private IConfiguration _config;

        public taskPortal(IConfiguration config)
        {
            _config = config;
        }
        // GET: api/values
        [HttpGet]
        public JsonResult Get()
        {
            dbHelper helper = new dbHelper();
            return Json(helper.getTasks());
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody]UserModel login)
        {
            IActionResult response = Unauthorized();
            var user = AuthenticateUser(login);

            if (user != null)
            {
                var tokenString = GenerateJSONWebToken(user);
                response = Ok(new { token = tokenString });
            }

            return response;
        }


        // POST api/values


        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        private string GenerateJSONWebToken(UserModel userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              null,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private UserModel AuthenticateUser(UserModel login)
        {
            UserModel user = null;

            //Validate the User Credentials  
            //Demo Purpose, I have Passed HardCoded User Information  
            if (login.Username == "djoshi")
            {
                user = new UserModel { Username = "djoshi", EmailAddress = "djoshi@gmail.com" };
            }
            return user;
        }
    }
}
