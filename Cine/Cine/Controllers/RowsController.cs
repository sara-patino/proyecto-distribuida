using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cine.Models;

namespace Cine.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RowsController : ControllerBase
    {
        private readonly RowContext _context;

        public RowsController(RowContext context)
        {
            _context = context;
        }

        // GET: api/Rows
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Row>>> GetRows()
        {
            return await _context.Rows.ToListAsync();
        }

        // GET: api/Rows/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Row>> GetRow(int id)
        {
            var row = await _context.Rows.FindAsync(id);

            if (row == null)
            {
                return NotFound();
            }

            return row;
        }

        // PUT: api/Rows/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRow(int id, Row row)
        {
            if (id != row.Id)
            {
                return BadRequest();
            }

            _context.Entry(row).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RowExists(id))
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

        // POST: api/Rows
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Row>> PostRow(Row row)
        {
            _context.Rows.Add(row);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRow), new { id = row.Id }, row);
        }

        // DELETE: api/Rows/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRow(int id)
        {
            var row = await _context.Rows.FindAsync(id);
            if (row == null)
            {
                return NotFound();
            }

            _context.Rows.Remove(row);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RowExists(int id)
        {
            return _context.Rows.Any(e => e.Id == id);
        }
    }
}
