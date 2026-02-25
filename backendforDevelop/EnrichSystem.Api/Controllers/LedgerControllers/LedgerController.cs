using EnrichSystem.Domain.Ledgers;
using EnrichSystem.Usecase.Interfaces.UseCase.LedgerInterface;
using Microsoft.AspNetCore.Mvc;

namespace EnrichSystem.Api.Controllers.LedgerControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LedgerController : ControllerBase
    {
        private readonly ILedgerUsecase _ledgerUsecase;
        public LedgerController(ILedgerUsecase ledgerUsecase)
        {
            _ledgerUsecase = ledgerUsecase;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllLedgers(int id)
        {
            var result = await _ledgerUsecase.GetAllLedgers(id);
            return Ok(result);
        }

        [HttpGet("all/{id:int}")]
        public async Task<IActionResult> GetLedgerById(int id)
        {
            var result = await _ledgerUsecase.GetLedgerById(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateLedger([FromBody] Ledger createObj)
        {
            var result = await _ledgerUsecase.CreateLedger(createObj);
            return Ok(result);
        }
        
    }
}
