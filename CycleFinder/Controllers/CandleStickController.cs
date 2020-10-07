using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CycleFinder.Models;
using CycleFinder.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CycleFinder.Dtos;
using LazyCache;

namespace CycleFinder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandleStickController : ControllerBase
    {
        private readonly ILogger<CandleStickController> _logger;
        private readonly ICandleStickRepository _repository;
        private readonly IAppCache _cache;

        public CandleStickController(
            ILogger<CandleStickController> logger, 
            ICandleStickRepository repository, 
            IAppCache cache)
        {
            _logger = logger;
            _repository = repository;
            _cache = cache;
        }

        private async Task<List<Symbol>> GetSymbols()
        {
            return await _cache.GetOrAddAsync("symbols", () => _repository.ListSymbols());
        }

        [HttpGet("{symbol}")]
        public async Task<ActionResult<IEnumerable<CandleStickDto>> > GetAllData(string symbol)
        {
            if (GetSymbols().Result.FirstOrDefault(_ => _.Name == symbol) == null)
            {
                return NotFound();
            }

            return Ok((await _repository.GetAllData(symbol, TimeFrame.Daily)));
        }
    }
}
