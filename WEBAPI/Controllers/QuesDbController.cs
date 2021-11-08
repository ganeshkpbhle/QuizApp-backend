using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEBAPI.Models;

namespace WEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuesDbController : ControllerBase
    {
        private readonly db_a7c24d_quizdbContext _context;

        public QuesDbController(db_a7c24d_quizdbContext context)
        {
            _context = context;
        }

        // GET: api/QuesDb
        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuesDb>>> GetQuesDbs()
        {
            return await _context.QuesDbs.ToListAsync();
        }

        // GET: api/QuesDb/5
        [HttpGet("{id}")]
        public async Task<ActionResult<QuesDb>> GetQuesDb(int id)
        {
            var quesDb = await _context.QuesDbs.FindAsync(id);

            if (quesDb == null)
            {
                return NotFound();
            }

            return quesDb;
        }

        // PUT: api/QuesDb/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuesDb(int id, QuesDb quesDb)
        {
            if (id != quesDb.QnId)
            {
                return BadRequest();
            }

            _context.Entry(quesDb).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuesDbExists(id))
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

        // POST: api/QuesDb
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<QuesDb>> PostQuesDb(QuesDb quesDb)
        {
            _context.QuesDbs.Add(quesDb);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetQuesDb", new { id = quesDb.QnId }, quesDb);
        }

        // DELETE: api/QuesDb/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuesDb(int id)
        {
            var quesDb = await _context.QuesDbs.FindAsync(id);
            if (quesDb == null)
            {
                return NotFound();
            }

            _context.QuesDbs.Remove(quesDb);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool QuesDbExists(int id)
        {
            return _context.QuesDbs.Any(e => e.QnId == id);
        }
    }
}
