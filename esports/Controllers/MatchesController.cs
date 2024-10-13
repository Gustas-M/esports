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

namespace esports.Controllers
{
    [Route("Championships/{championshipId}/Tournaments/{tournamentId}/Matches")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class MatchesController : Controller
    {
        private readonly EsportsContext _context;

        public MatchesController(EsportsContext context)
        {
            _context = context;
        }

        // GET: Matches
        /// <summary>
        /// Returns a list of all matches for a specific tournament
        /// </summary>
        /// <param name="championshipId"></param>
        /// <param name="tournamentId"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Index(int championshipId, int tournamentId)
        {
            var matches = await _context.Matches
                .Where(m => m.TournamentId == tournamentId)
                .Include(m => m.Tournament)
                .Select(m => new
                {
                    m.Id,
                    m.Round_Number,
                    m.Match_In_Round_Number,
                    m.FirstTeamId,
                    m.SecondTeamId,
                    m.WinningTeamId,
                    m.TournamentId,
                    Tournament = new
                    {
                        m.Tournament.Id,
                        m.Tournament.Name,
                        m.Tournament.Number_of_rounds,
                        m.Tournament.ChampionshipId
                    }
                })
                .ToListAsync();

            return Ok(matches);
        }

        // GET: Matches/Details/5
        /// <summary>
        /// Returns a specific match for a specific tournament
        /// </summary>
        /// <param name="championshipId"></param>
        /// <param name="tournamentId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Details(int championshipId, int tournamentId, int id)
        {
            var match = await _context.Matches
                .Include(m => m.Tournament)
                .Select(m => new
                {
                    m.Id,
                    m.Round_Number,
                    m.Match_In_Round_Number,
                    m.FirstTeamId,
                    m.SecondTeamId,
                    m.WinningTeamId,
                    m.TournamentId,
                    Tournament = new
                    {
                        m.Tournament.Id,
                        m.Tournament.Name,
                        m.Tournament.Number_of_rounds,
                        m.Tournament.ChampionshipId
                    }
                })
                .FirstOrDefaultAsync(m => m.Id == id && m.TournamentId == tournamentId);
            if (match == null)
            {
                return NotFound();
            }

            return Ok(match);
        }

        /// <summary>
        /// Creates a new match for a specific tournament
        /// </summary>
        /// <param name="championshipId"></param>
        /// <param name="tournamentId"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Create(int championshipId, int tournamentId, [FromBody] Match match)
        {
            // Validate the tournamentId
            var tournament = await _context.Tournaments.FindAsync(tournamentId);
            if (tournament == null || tournament.ChampionshipId != championshipId)
            {
                return NotFound();
            }

            // Set the TournamentId for the match


            // Remove the Tournament property from ModelState validation
            ModelState.Remove(nameof(match.Tournament));

            if (ModelState.IsValid)
            {
                match.TournamentId = tournamentId;
                match.Tournament = tournament;
                int round_count = tournament.Number_of_rounds;
                if (match.IsValid(round_count))
                {
                    _context.Matches.Add(match);
                    await _context.SaveChangesAsync();


                    var createdMatch = await _context.Matches
                    .Include(m => m.Tournament)
                    .Where(m => m.Id == match.Id)
                    .Select(m => new
                    {
                        m.Id,
                        m.Round_Number,
                        m.Match_In_Round_Number,
                        m.FirstTeamId,
                        m.SecondTeamId,
                        m.WinningTeamId,
                        m.TournamentId,
                        Tournament = new
                        {
                            m.Tournament.Id,
                            m.Tournament.Name,
                            m.Tournament.Number_of_rounds,
                            m.Tournament.ChampionshipId
                        }
                    })
                    .FirstOrDefaultAsync();

                    return CreatedAtAction(nameof(Details), new { championshipId, tournamentId, id = match.Id }, createdMatch);
                }

                return UnprocessableEntity();
            }

            return BadRequest(ModelState);
        }


        /// <summary>
        /// Edits a match for a specific tournament
        /// </summary>
        /// <param name="championshipId"></param>
        /// <param name="tournamentId"></param>
        /// <param name="id"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Edit(int championshipId, int tournamentId, int id, [FromBody] Match match)
        {

            var tournament = await _context.Tournaments.FindAsync(tournamentId);
            ModelState.Remove(nameof(match.Tournament));

            if (ModelState.IsValid)
            {
                try
                {
                    match.TournamentId = tournamentId;
                    match.Tournament = tournament;
                    int round_count = tournament.Number_of_rounds;
                    if (match.IsValid(round_count))
                    {
                        _context.Update(match);
                        await _context.SaveChangesAsync();
                        return Ok(match);
                    }

                    return UnprocessableEntity();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MatchExists(match.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }


            }

            
            return BadRequest(ModelState);
        }

        /// <summary>
        /// Deletes a match for a specific tournament
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var match = await _context.Matches
                .Include(match => match.Tournament)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (match == null)
            {
                return NotFound();
            }

            _context.Matches.Remove(match);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool MatchExists(int id)
        {
            return _context.Matches.Any(e => e.Id == id);
        }
    }
}
