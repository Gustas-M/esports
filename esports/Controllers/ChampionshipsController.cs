using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using esports.Data;
using esports.Models;

namespace esports.Controllers
{
    [Route("Championships")]
    public class ChampionshipsController : Controller
    {
        private readonly EsportsContext _context;

        public ChampionshipsController(EsportsContext context)
        {
            _context = context;
        }

        // GET: Championships
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return Ok(await _context.Championships.ToListAsync());
        }

        // GET: Championships/Details/5

        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var championship = await _context.Championships
                .FirstOrDefaultAsync(m => m.Id == id);
            if (championship == null)
            {
                return NotFound();
            }

            return Ok(championship);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Championship championship)
        {
            if (ModelState.IsValid)
            {     
                if (championship.IsValid())
                {
                    _context.Add(championship);
                    await _context.SaveChangesAsync();
                    return CreatedAtAction("POST", new { id = championship.Id }, championship);
                }

                return UnprocessableEntity();

            }
                       
            return BadRequest();
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] Championship championship)
        {
            if (id != championship.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (championship.IsValid())
                    {
                        _context.Update(championship);
                        await _context.SaveChangesAsync();
                        return Ok(championship);
                    }

                    return UnprocessableEntity();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChampionshipExists(championship.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return BadRequest(championship);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var championship = await _context.Championships
                .FirstOrDefaultAsync(m => m.Id == id);
            if (championship == null)
            {
                return NotFound();
            }


            _context.Championships.Remove(championship);     

            await _context.SaveChangesAsync();
            return Ok(championship);
        }


        private bool ChampionshipExists(int id)
        {
            return _context.Championships.Any(e => e.Id == id);
        }
    }
}
