﻿using System;
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
    [Route("Championships")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    //[ApiController]
    public class ChampionshipsController : Controller
    {
        private readonly EsportsContext _context;

        public ChampionshipsController(EsportsContext context)
        {
            _context = context;
        }

        // GET: Championships
        /// <summary>
        /// Returns a list of all championships
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Index()
        {
            return Ok(await _context.Championships.ToListAsync());
        }

        // GET: Championships/Details/5
        /// <summary>
        /// Returns a specific championship
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Creates a new championship with specified parameters
        /// </summary>
        /// <param name="championship"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Create([FromBody] ChampionshipDto championship)
        {
            var tmp = _context.Championships.FirstOrDefault(c => c.Name == championship.Name && c.Year == championship.Year);
            if (tmp != null)
            {
                return UnprocessableEntity();
            }

            if (ModelState.IsValid)
            {     
                if (championship.IsValid())
                {
                    Championship champDbObj = new Championship
                    {
                        Name = championship.Name,
                        Year = championship.Year
                    };
                    _context.Add(champDbObj);
                    await _context.SaveChangesAsync();
                    return CreatedAtAction("POST", new { id = champDbObj.Id }, champDbObj);
                }

                return UnprocessableEntity();

            }

            var validation = new ValidationProblemDetails(ModelState);
                       
            return BadRequest(validation);
        }

        /// <summary>
        /// Edits a championship with specified parameters
        /// </summary>
        /// <param name="id"></param>
        /// <param name="championship"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Edit(int id, [FromBody] ChampionshipDto championship)
        {
            var champDbObj = await _context.Championships.FindAsync(id);
            if (champDbObj == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (championship.IsValid())
                {
                    champDbObj.Name = championship.Name;
                    champDbObj.Year = championship.Year;
                    _context.Update(champDbObj);
                    await _context.SaveChangesAsync();
                    return Ok(championship);
                }

                return UnprocessableEntity();
            }

            var validation = new ValidationProblemDetails(ModelState);

            return BadRequest(validation);
        }

        /// <summary>
        /// Deletes a championship with specified ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = Roles.Admin)]
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
            return NoContent();
        }


        private bool ChampionshipExists(int id)
        {
            return _context.Championships.Any(e => e.Id == id);
        }
    }
}
