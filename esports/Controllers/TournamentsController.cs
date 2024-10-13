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

        [HttpGet]
        public async Task<IActionResult> Index(int championshipId)
        {
            var esportsContext = _context.Tournaments.Where(t => t.ChampionshipId == championshipId).Include(t => t.Championship).ToListAsync();

            return Ok(await esportsContext);
        }

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



            ModelState.Remove(nameof(tournament.Championship));

            if (ModelState.IsValid)
            {
                tournament.ChampionshipId = championshipId;
                tournament.Championship = champ;
                if (tournament.IsValid())
                {
                    _context.Tournaments.Add(tournament);
                    await _context.SaveChangesAsync();
                    return CreatedAtAction(nameof(Details), new { id = tournament.Id }, tournament);
                }

                return UnprocessableEntity();
            }

            return BadRequest(ModelState);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int championshipId, int id, [FromBody] Tournament tournament)
        {
            ModelState.Remove(nameof(tournament.Championship));
            if (ModelState.IsValid)
            {
                if (id != tournament.Id)
                {
                    return NotFound();
                }

                var champ = await _context.Championships.FindAsync(tournament.ChampionshipId);



                try
                {
                    tournament.Championship = champ;
                    tournament.ChampionshipId = championshipId;

                    if (tournament.IsValid())
                    {
                        _context.Update(tournament);
                        await _context.SaveChangesAsync();
                        return Ok(tournament);
                    }

                    return UnprocessableEntity();

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
            }

            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int? id)
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

            _context.Tournaments.Remove(tournament);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        private bool TournamentExists(int id)
        {
            return _context.Tournaments.Any(e => e.Id == id);
        }


    }
}
