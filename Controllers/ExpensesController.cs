using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyKoloApi.Data;
using MyKoloApi.DTOS;
using MyKoloApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyKoloApi.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {

        private readonly AppDbContext _context;
        

        public ExpensesController(AppDbContext context)
        {
            _context = context;
        }
     
        [Route("all")]
        [HttpGet]
        public IActionResult GetAll()
        {
            List<GetExpenseInfo> response = new List<GetExpenseInfo>();
            var contextUser = HttpContext.User;

            if (contextUser.HasClaim(claim => claim.Type == "Email"))
            {
                Claim userClaim = contextUser.FindFirst(claim => claim.Type == "Email");
                User user = _context.Set<User>()
                    .Where(user => user.Email == userClaim.Value)
                    .Include(x => x.UserExpenses)
                    .FirstOrDefault();
                foreach (var expense in user.UserExpenses)
                {
                    response.Add(new GetExpenseInfo() { 
                        Amount = expense.Amount,
                        Description = expense.Description,
                        ExpenseId = expense.Id
                    });
                }
                return Ok(response);
            }
             return NotFound();
        }


        [HttpGet]
        [Route("testaccess")]
        public IActionResult TestAccess()
        {
            var user = HttpContext.User;
            Console.WriteLine(user.HasClaim(claim => claim.Type == "Email"));

            return Ok("GRANTED, you have access to this Controller");
        }


        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        public IActionResult AddExpenses([FromBody] List<AddExpense> addExpensesForm)
        {
            var contextUser = HttpContext.User;

            if (contextUser.HasClaim(claim => claim.Type == "Email"))
            {
                Claim userClaim = contextUser.FindFirst(claim => claim.Type == "Email");
                User user = _context.Set<User>()
                    .Where(user => user.Email == userClaim.Value)
                    .Include(x => x.UserExpenses)
                    .FirstOrDefault();

                foreach(var addExpenseForm in addExpensesForm)
                {
                    user.UserExpenses.Add(new Expense() { 
                        Id = Guid.NewGuid().ToString(),
                        Amount = addExpenseForm.Amount,
                        Description = addExpenseForm.Description
                     });
                }

                _context.SaveChanges();

            
            }
               
            return Ok("Success");
        }

        [HttpPut("{id}")]
        public void UpdateExpenses([FromBody] List<GetExpenseInfo> updateExpensesForm)
        {
        }

        
        [HttpDelete]
        public void DeleteExpenses([FromBody] List<string> expenseIdsToDelete)
        {
        }
    }
}
