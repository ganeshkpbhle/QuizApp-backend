using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WEBAPI.Models;
using WEBAPI.Models_;

namespace WEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class userdataController : ControllerBase
    {
        private readonly db_a7c24d_quizdbContext _context;
        private readonly IConfiguration config;
        public userdataController(db_a7c24d_quizdbContext context,IConfiguration Config)
        {
            _context = context;
            config = Config;
        }

        // GET: api/userdata
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<UserDatum>>> GetUserData()
        //{
        //    return await _context.UserData.ToListAsync();
        //}

        // GET: api/userdata/5
        [HttpGet("{id}")]
        public async Task<ActionResult<getUsr>> GetUserDatum(int id)
        {
            var userDatum = await _context.UserData.FindAsync(id);
            if (IsCorrectUser(HttpContext.User.Identity as ClaimsIdentity, id))
            {
                if (userDatum == null)
                {
                    return NotFound();
                }
                
                return new getUsr() {UEmail=userDatum.UEmail,UId=userDatum.UId,Mobnumber=userDatum.Mobnumber };
            }
            return StatusCode(401);
        }

        // PUT: api/userdata/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserDatum(int id, UserDatum userDatum)
        {
            if (IsCorrectUser(HttpContext.User.Identity as ClaimsIdentity, id))
            {
                if (id != userDatum.UId)
                {
                    return BadRequest();
                }

                _context.Entry(userDatum).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserDatumExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return NoContent();
            }

            return StatusCode(401);
        }

        // POST: api/userdata
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<UserDatum>> PostUserDatum(UserDatum userDatum)
        {   
            foreach(var user in _context.UserData)
            {
                if(user.UEmail == userDatum.UEmail)
                {
                    return StatusCode(409, $"User '{user.UEmail}' already exists");
                }
            }
            _context.UserData.Add(userDatum);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserDatum", new { id = userDatum.UId }, userDatum);
        }

        // DELETE: api/userdata/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserDatum(int id)
        {
            var userDatum = await _context.UserData.FindAsync(id);
            if (userDatum == null)
            {
                return NotFound();
            }

            _context.UserData.Remove(userDatum);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(loginModel model)
        {
            IActionResult response = new ForbidResult();
            var user = await _context.UserData.FirstAsync(e => e.UEmail == model.UEmail);
            if(user !=null)
            {
                if (model.Passwd == user.Passwd)
                {
                    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
                    var signCredentials = new SigningCredentials(secretKey,SecurityAlgorithms.HmacSha256);
                    var payload = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub,user.UEmail),
                        new Claim(JwtRegisteredClaimNames.UniqueName,user.UId.ToString())
                    };
                    var token = new JwtSecurityToken(config["Jwt:Issuer"],
                                                    config["Jwt:Issuer"],
                                                    payload,
                                                    expires: DateTime.Now.AddMinutes(10),
                                                    signingCredentials: signCredentials
                                                    );
                    var registeredToken = new JwtSecurityTokenHandler().WriteToken(token);
                    response = Ok(new { token = registeredToken });
                }
            }
            return response;
        }

        private bool UserDatumExists(int id)
        {
            return _context.UserData.Any(e => e.UId == id);
        }
        private bool IsCorrectUser(ClaimsIdentity identity, int id)
        {
            IEnumerable<Claim> claim = identity.Claims;
            int userId = int.Parse(claim.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault().Value);
            if (userId == id)
            {
                return true;
            }
            return false;
        }
    }
}
