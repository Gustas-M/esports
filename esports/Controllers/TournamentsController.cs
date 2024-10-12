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
    [Route("Championships/{championshipId}/Tournaments")]
    public class TournamentsController : Controller
    {
        private readonly EsportsContext _context;

        public TournamentsController(EsportsContext context)
        {
            _context = context;
        }

        // GET: Tournaments
        [HttpGet]
        public async Task<IActionResult> Index(int championshipId)
        {
            var esportsContext = _context.Tournaments.Where(t => t.ChampionshipId == championshipId).Include(t => t.Championship).ToListAsync();

            return Ok(await esportsContext);
        }

        // GET: Tournaments/Details/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int championshipId, int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tournament = await _context.Tournaments
                .Include(t => t.Championship)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (tournament == null)
            {
                return NotFound();
            }

            if (tournament.ChampionshipId != championshipId)
            {
                return NotFound();
            }

            return Ok(tournament);
        }

        [HttpPost]
        public async Task<IActionResult> Create(int championshipId, [FromBody] Tournament tournament)
        {
            var champ = await _context.Championships.FindAsync(championshipId);
            if (champ == null)
            {
                return NotFound();
            }

            tournament.ChampionshipId = championshipId;
            tournament.Championship = champ;

            if (tournament.IsValid())
            {
                _context.Tournaments.Add(tournament);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(Details), new { id = tournament.Id }, tournament);
            }

            return BadRequest(ModelState);
        }

        // GET: Tournaments/Edit/5
        //[HttpPut]
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var tournament = await _context.Tournaments.FindAsync(id);
        //    if (tournament == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["ChampionshipId"] = new SelectList(_context.Championships, "Id", "Id", tournament.ChampionshipId);
        //    return View(tournament);
        //}

        // POST: Tournaments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Number_of_rounds,ChampionshipId")] Tournament tournament)
        {
            if (id != tournament.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tournament);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TournamentExists(tournament.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ChampionshipId"] = new SelectList(_context.Championships, "Id", "Id", tournament.ChampionshipId);
            return View(tournament);
        }

        //// GET: Tournaments/Delete/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var tournament = await _context.Tournaments
        //        .Include(t => t.Championship)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (tournament == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(tournament);
        //}

        //// POST: Tournaments/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var tournament = await _context.Tournaments.FindAsync(id);
        //    if (tournament != null)
        //    {
        //        _context.Tournaments.Remove(tournament);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool TournamentExists(int id)
        {
            return _context.Tournaments.Any(e => e.Id == id);
        }


    }
}
