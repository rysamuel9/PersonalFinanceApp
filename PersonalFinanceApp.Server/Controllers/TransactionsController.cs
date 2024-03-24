using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonalFinanceApp.Server.DTO;
using PersonalFinanceApp.Server.Repositories.IRepository;
using System.IO;
using System.Security.Claims;

namespace PersonalFinanceApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TransactionsController(ITransactionService transactionService, IHttpContextAccessor httpContextAccessor)
        {
            _transactionService = transactionService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> CreateTransaction(TransactionCreateDTO model)
        {
            try
            {
                var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var transaction = await _transactionService.CreateTransactionAsync(userId, model.Amount, model.Type, model.Category, model.Description, model.Date);
                return Ok(transaction);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet, Authorize]
        public async Task<IActionResult> GetTransactions()
        {
            try
            {
                var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var transactions = await _transactionService.GetTransactionsByUserIdAsync(userId);

                if (transactions.Any())
                {
                    return Ok(transactions);
                }
                else
                {
                    return NotFound(new { message = "No transactions found for the current user." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("filter"), Authorize]
        public async Task<IActionResult> GetFilteredTransactions([FromQuery] FilterOptionsDTO filterOptions)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var transactions = await _transactionService.GetFilteredTransactionsAsync(userId, filterOptions.StartDate, filterOptions.EndDate, filterOptions.Type, filterOptions.Category);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("GetAllTransactions"), Authorize]
        public async Task<IActionResult> GetAllTransactions([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var transactions = await _transactionService.GetAllTransactionsAsync(userId, pageNumber, pageSize);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("TotalIncomeExpense"), Authorize]
        public async Task<IActionResult> GetTotalIncomeExpense([FromQuery] int? year = null, [FromQuery] int? month = null)
        {
            try
            {
                if (!year.HasValue)
                {
                    year = DateTime.Now.Year;
                }

                var result = await _transactionService.GetTotalIncomeExpenseAsync(year.Value, month);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
