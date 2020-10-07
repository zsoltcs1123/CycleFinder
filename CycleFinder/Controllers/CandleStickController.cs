using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CycleFinder.Models;
using CycleFinder.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CycleFinder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandleStickController : ControllerBase
    {
        private readonly ILogger<CandleStickController> _logger;
        private readonly ICandleStickRepository _repository;
        private List<Symbol> _symbols = new List<Symbol>();

        public CandleStickController(ILogger<CandleStickController> logger, ICandleStickRepository repository)
        {
            _logger = logger;
            _repository = repository;
            GetSymbols();
        }

        private async void GetSymbols()
        {
            _symbols = await _repository.ListSymbols();
        }

        [HttpGet("{symbol}")]
        public async Task<ActionResult<IEnumerable<CandleStick>> > GetAllData(string symbol)
        {
            if (_symbols.FirstOrDefault(_ => _.Name == symbol) == null)
            {
                return NotFound();
            }

            return Ok(await _repository.GetAllData(symbol, TimeFrame.Daily));
        }
    }
}
