using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using RoadTrip.API.Entities;
using RoadTrip.API.Models;

namespace RoadTrip.API.Controllers
{
    [RoutePrefix("api/Expense")]
    public class ExpenseController : ApiController
    {
        private readonly AuthContext _context;

        public ExpenseController(AuthContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet]
        [Route("GetExpensesForTrip/{roadTripId}")]
        public List<ExpenseDisplayModel> GetExpenses(int roadTripId)
        {
            var username = ClaimsPrincipal.Current.Identity.Name;

            var ownerPerson = _context.Persons.First(x => x.RegisteredUserName == username);

            //Check if the user belongs to the Tirp
            if (_context.TripUserMaps.Any(x => (x.PersonId == ownerPerson.Id && x.TripId == roadTripId)))
            {
                return _context.Expenses.Include("Person").Where(x => x.TripId == roadTripId).Select(x => new ExpenseDisplayModel
                {
                    DollarAmount = x.Amount,
                    ExpenseDate = x.ExpenseDate,
                    ExpenseId = x.Id,
                    FirstName = x.Person.FirstName,
                    LastName = x.Person.LastName,
                    Notes = x.Notes
                }).ToList();
            }
            throw new Exception("You are not authorized to view expenses for this road trip.");
        }

        [Authorize]
        [HttpPost]
        [Route("AddExpenseToTrip")]
        public ExpenseDisplayModel AddExpenseToTrip(ExpenseSaveModel expense)
        {
            var username = ClaimsPrincipal.Current.Identity.Name;

            var ownerPerson = _context.Persons.First(x => x.RegisteredUserName == username);

            //Check if the user belongs to the Tirp
            if (_context.TripUserMaps.Any(x => (x.PersonId == ownerPerson.Id && x.TripId == expense.TripId)))
            {
                var exp = new Expense
                {
                    Amount = expense.dollarAmount,
                    TripId = expense.TripId,
                    ExpenseDate = expense.ExpenseDate,
                    PersonId = ownerPerson.Id,
                    Notes = expense.Notes.Length > 20 ? expense.Notes.Substring(0, 20) : expense.Notes
                };
                _context.Expenses.Add(exp);
                _context.SaveChanges();
                return new ExpenseDisplayModel
                {
                    DollarAmount = exp.Amount,
                    ExpenseDate = exp.ExpenseDate,
                    Notes = exp.Notes,
                    ExpenseId = exp.Id,
                    FirstName = ownerPerson.FirstName,
                    LastName = ownerPerson.LastName
                };
            }
            throw new Exception("You are not authorized to add expense to this road trip");
        }

        [Authorize]
        [HttpPost]
        [Route("UpdateExpense")]
        public ExpenseDisplayModel UpdateExpense(ExpenseDisplayModel expense)
        {
            var username = ClaimsPrincipal.Current.Identity.Name;

            var ownerPerson = _context.Persons.First(x => x.RegisteredUserName == username);

            //Check if the user belongs to the Tirp
            if (_context.TripUserMaps.Any(x => (x.PersonId == ownerPerson.Id && x.TripId == expense.RoadTripId)))
            {
                var expToModify = _context.Expenses.First(x => x.Id == expense.ExpenseId);
                expToModify.Amount = expense.DollarAmount;
                expToModify.Notes = expense.Notes;
                _context.SaveChanges();
                return expense;
            }
            throw new Exception("You are not authorized to modify this expense");
        }
    }
}
