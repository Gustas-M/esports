using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using esports.Data;
using esports.Models;
using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;

namespace esports.Controllers
{
    [Route("Championships/{championshipId}/Tournaments")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class TournamentsController : Controller
    {
        private readonly EsportsContext _context;

        public TournamentsController(EsportsContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns a list of all tournaments for a specific championship
        /// </summary>
        /// <param name="championshipId"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Index(int championshipId)
        {
            var esportsContext = _context.Tournaments.Where(t => t.ChampionshipId == championshipId).Include(t => t.Championship).ToListAsync();

            return Ok(await esportsContext);
        }

        /// <summary>
        /// Returns a specific tournament for a specific championship
        /// </summary>
        /// <param name="championshipId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Creates a new tournament for a specific championship
        /// </summary>
        /// <param name="championshipId"></param>
        /// <param name="tournament"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(int championshipId, [FromBody] TournamentDto tournament)
        {
            var champ = await _context.Championships.FindAsync(championshipId);
            if (champ == null)
            {
                return NotFound();
            }

            tournament.ChampionshipId = championshipId;


            if (ModelState.IsValid)
            {
                if (tournament.IsValid())
                {
                    Tournament tournamentDbObj = new Tournament
                    {
                        Name = tournament.Name,
                        Number_of_rounds = tournament.Number_of_rounds,
                        Championship = champ,
                        ChampionshipId = championshipId
                    };
                    _context.Tournaments.Add(tournamentDbObj);
                    await _context.SaveChangesAsync();
                    return CreatedAtAction(nameof(Details), new { id = tournamentDbObj.Id }, tournamentDbObj);
                }

                return UnprocessableEntity();
            }

            var validation = new ValidationProblemDetails(ModelState);

            return BadRequest(validation);
        }

        /// <summary>
        /// Edits a specific tournament for a specific championship
        /// </summary>
        /// <param name="championshipId"></param>
        /// <param name="id"></param>
        /// <param name="tournament"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int championshipId, int id, [FromBody] TournamentDto tournament)
        {
            var tournamentDbObj = await _context.Tournaments.FindAsync(id);
            if(tournamentDbObj == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var champ = await _context.Championships.FindAsync(championshipId);

                if (tournament.IsValid())
                {
                    tournamentDbObj.Name = tournament.Name;
                    tournamentDbObj.Number_of_rounds = tournament.Number_of_rounds;
                    tournamentDbObj.ChampionshipId = championshipId;
                    tournamentDbObj.Championship = champ;

                    _context.Update(tournamentDbObj);
                    await _context.SaveChangesAsync();
                    return Ok(tournamentDbObj);
                }

                return UnprocessableEntity();
            }

            var validation = new ValidationProblemDetails(ModelState);

            return BadRequest(validation);
        }

        /// <summary>
        /// Deletes a specific tournament for a specific championship
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[Authorize(Roles = "Admin")]
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
