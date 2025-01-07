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
            var matches = _context.Matches
                .Include(m => m.FirstTeam)
                .Include(m => m.SecondTeam)
                .Include(m => m.WinningTeam)
                .Where(m => m.TournamentId == tournamentId)
                .ToListAsync();

            //return Ok(matches);
            //var matches = _context.Matches.Where(t => t.TournamentId == tournamentId).Include(t => t.Tournament).ToListAsync();

            return Ok(await matches);
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
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Create(int championshipId, int tournamentId, [FromBody] MatchDto match)
        {
            // Validate the tournamentId
            var tournament = await _context.Tournaments.FindAsync(tournamentId);
            if (tournament == null)
            {
                return NotFound();
            }

            if (!tournament.Number_of_rounds.HasValue)
            {
                return UnprocessableEntity();
            }
                        

            if (ModelState.IsValid)
            {
                if (match.IsValid(tournament.Number_of_rounds.Value))
                {
                    Team? firstTeam = await _context.Teams.FindAsync(match.FirstTeamId);
                    Team? secondTeam = await _context.Teams.FindAsync(match.SecondTeamId);
                    Team? winningTeam = await _context.Teams.FindAsync(match.WinningTeamId);
                    Match matchDbObj = new Match
                    {
                        Round_Number = match.Round_Number,
                        Match_In_Round_Number = match.Match_In_Round_Number,
                        FirstTeamId = match.FirstTeamId,
                        FirstTeam = firstTeam,
                        SecondTeamId = match.SecondTeamId,
                        SecondTeam = secondTeam,
                        WinningTeamId = match.WinningTeamId,
                        WinningTeam = winningTeam,
                        TournamentId = tournamentId,
                        Tournament = tournament
                    };

                    _context.Matches.Add(matchDbObj);
                    await _context.SaveChangesAsync();
                    return CreatedAtAction(nameof(Details), new { championshipId, tournamentId, id = matchDbObj.Id }, matchDbObj);
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
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Edit(int championshipId, int tournamentId, int id, [FromBody] MatchDto match)
        {

            var matchDbObj = await _context.Matches.FindAsync(id);
            if (matchDbObj == null)
            {
                return NotFound();
            }

            

            if (ModelState.IsValid)
            {
                var tournament = await _context.Tournaments.FindAsync(tournamentId);

                if (!tournament.Number_of_rounds.HasValue)
                {
                    return UnprocessableEntity();
                }

                if (matchDbObj.IsValid(tournament.Number_of_rounds.Value))
                {

                    matchDbObj.Round_Number = match.Round_Number;
                    matchDbObj.Match_In_Round_Number = match.Match_In_Round_Number;
                    matchDbObj.FirstTeamId = match.FirstTeamId;
                    Team? firstTeam = await _context.Teams.FindAsync(match.FirstTeamId);
                    Team? secondTeam = await _context.Teams.FindAsync(match.SecondTeamId);
                    Team? winningTeam = await _context.Teams.FindAsync(match.WinningTeamId);
                    matchDbObj.SecondTeamId = match.SecondTeamId;
                    matchDbObj.WinningTeamId = match.WinningTeamId;
                    matchDbObj.TournamentId = tournamentId;
                    matchDbObj.Tournament = tournament;

                    _context.Update(matchDbObj);
                    await _context.SaveChangesAsync();
                    return Ok(matchDbObj);
                }

                return UnprocessableEntity();
            }

            var validation = new ValidationProblemDetails(ModelState);

            return BadRequest(validation);
        }

        /// <summary>
        /// Deletes a match for a specific tournament
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = Roles.Admin)]
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
