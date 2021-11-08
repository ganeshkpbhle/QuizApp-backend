using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEBAPI.Models;

namespace WEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class attemptController : ControllerBase
    {
        private readonly db_a7c24d_quizdbContext _context;

        public attemptController(db_a7c24d_quizdbContext context)
        {
            _context = context;
        }

        // GET: api/attempt
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Attempt>>> GetAttempts()
        //{
        //    return await _context.Attempts.ToListAsync();
        //}

        // GET: api/attempt/5
        [HttpGet("{id}")]
        public IActionResult GetAttempt(int id)
        {   
            var response = Forbid();
            if (IsCorrectUser(HttpContext.User.Identity as ClaimsIdentity,id))
            {
                List<Attempt> attempted = new List<Attempt>();
                foreach (var attmpt in _context.Attempts)
                {
                    if (attmpt.UId == id)
                    {
                        attempted.Add(new Attempt() { UId = id, Score = attmpt.Score, Date = attmpt.Date, EntryId = attmpt.EntryId, TimeSpent = attmpt.TimeSpent });
                    }
                }
                attempted.Sort((x, y) => y.EntryId.CompareTo(x.EntryId));
                return Ok(attempted);
                
            }

            return response;
        }

        // PUT: api/attempt/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutAttempt(int id, Attempt attempt)
        //{
        //    if (id != attempt.EntryId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(attempt).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!AttemptExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/attempt
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Attempt>> PostAttempt(Attempt attempt)
        {
            _context.Attempts.Add(attempt);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAttempt", new { id = attempt.EntryId }, attempt);
        }

        // DELETE: api/attempt/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteAttempt(int id)
        //{
        //    var attempt = await _context.Attempts.FindAsync(id);
        //    if (attempt == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Attempts.Remove(attempt);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        private bool AttemptExists(int id)
        {
            return _context.Attempts.Any(e => e.EntryId == id);
        }

        private bool IsCorrectUser(ClaimsIdentity identity,int id)
        {
            IEnumerable<Claim> claim = identity.Claims;
            int userId = int.Parse(claim.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault().Value);
            if(userId == id)
            {
                return true;
            }
            return false;
        }
    }
}
