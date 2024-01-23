using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using MovieSearchAPI.model.Service;

namespace MovieSearchAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet("search/{title}")]
        public async Task<IActionResult> Search(string title)
        {
            try
            {
                var result = await _movieService.SearchMovieAsync(title);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("details/{imdbId}")]
        public async Task<IActionResult> Details(string imdbId)
        {
            try
            {
                var result = await _movieService.GetMovieDetailsAsync(imdbId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }

}
