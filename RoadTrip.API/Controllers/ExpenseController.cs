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

                    Notes = x.Notes,
                    RoadTripId = x.TripId
                    
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
                    Amount = expense.DollarAmount,
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
                    LastName = ownerPerson.LastName,
                    RoadTripId = exp.TripId

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

        [Authorize]
        [HttpGet]
        [Route("GetCompleteSharedCost/{roadTripId}")]
        public List<SharedCostDisplayModel> GetCompleteSharedCost(int roadTripId)
        {
            var username = ClaimsPrincipal.Current.Identity.Name;

            var ownerPerson = _context.Persons.First(x => x.RegisteredUserName == username);

            //Check if the user belongs to the Tirp
            if (_context.TripUserMaps.Any(x => (x.PersonId == ownerPerson.Id && x.TripId == roadTripId)))
            {

                return GetSharedCostForRoadTrip(roadTripId);

            }
            throw  new Exception("You are not authorized to see the spilt for this road tirp.");
        }

        [Authorize]
        [HttpGet]
        [Route("GetMySharedCost/{roadTripId}")]
        public List<IndiSplit> GetMySharedCost(int roadTripId)
        {
            var username = ClaimsPrincipal.Current.Identity.Name;

            var ownerPerson = _context.Persons.First(x => x.RegisteredUserName == username);

            //Check if the user belongs to the Tirp
            if (_context.TripUserMaps.Any(x => (x.PersonId == ownerPerson.Id && x.TripId == roadTripId)))
            {
                var sharedCost = GetSharedCostForRoadTrip(roadTripId).FirstOrDefault(x => x.PersonId == ownerPerson.Id);
                return sharedCost != null ? sharedCost.Payouts : new List<IndiSplit>();
            }
            throw new Exception("You are not authorized to see the spilt for this road tirp.");
        }

        private List<SharedCostDisplayModel> GetSharedCostForRoadTrip(int roadTripId)
        {
            var users = _context.TripUserMaps.Include("User").Where(x => x.TripId == roadTripId).ToList();
            var expenses = _context.Expenses.Where(x => x.TripId == roadTripId).ToList();
            var totalExpense = expenses.Sum(y => y.Amount);
            var eachPersonShare = totalExpense / users.Count;
            //calculate each persons defecit
            var expGroupedByUser =
                users.Select(
                    x =>
                        new ExpData
                        {
                            PersonId = x.PersonId,
                            AmountSpent =
                                expenses.Any(y => y.PersonId == x.PersonId)
                                    ? expenses.Where(y => y.PersonId == x.PersonId).Sum(g => g.Amount)
                                    : 0,
                            FirstName = x.User.FirstName,
                            LastName = x.User.LastName

                        }).OrderByDescending(k => k.AmountSpent);
            var userWhoSpentMoreThanShare =
                expGroupedByUser.Where(x => x.AmountSpent >= eachPersonShare).OrderByDescending(x => x.AmountSpent);
            var usersWhoSpentLessThanShare =
                expGroupedByUser.Where(x => x.AmountSpent < eachPersonShare).OrderBy(x => x.AmountSpent);
            var uwsLQ = new Queue<ExpData>(usersWhoSpentLessThanShare);
            var sharedCost = new Dictionary<int, List<IndiSplit>>();
            //matching
            if (uwsLQ.Any())
            {
                var popedLessSpender = uwsLQ.Dequeue();
                var pendingShare = eachPersonShare - popedLessSpender.AmountSpent;
                foreach (var moreSpendingUser in userWhoSpentMoreThanShare)
                {
                    var extraAmountSpent = moreSpendingUser.AmountSpent - eachPersonShare;
                    while (extraAmountSpent > 0)
                    {
                        if (pendingShare < extraAmountSpent)
                        {
                            extraAmountSpent -= pendingShare;
                            AddUserToList(ref sharedCost, moreSpendingUser.PersonId, pendingShare, popedLessSpender);
                            AddUserToList(ref sharedCost, popedLessSpender.PersonId, pendingShare*-1, moreSpendingUser);
                            popedLessSpender = uwsLQ.Dequeue();
                            pendingShare = eachPersonShare - popedLessSpender.AmountSpent;
                        }
                        else if (pendingShare > extraAmountSpent)
                        {
                            //extra amount = 0
                            //pendingshare still remains
                            AddUserToList(ref sharedCost, moreSpendingUser.PersonId, extraAmountSpent, popedLessSpender);
                            AddUserToList(ref sharedCost, popedLessSpender.PersonId, extraAmountSpent*-1,
                                moreSpendingUser);
                            pendingShare -= extraAmountSpent;
                            extraAmountSpent = 0;
                        }
                        else
                        {
                            AddUserToList(ref sharedCost, moreSpendingUser.PersonId, extraAmountSpent, popedLessSpender);
                            AddUserToList(ref sharedCost, popedLessSpender.PersonId, extraAmountSpent*-1,
                                moreSpendingUser);
                            extraAmountSpent = 0;
                            if (uwsLQ.Count == 0)
                            {
                                break;
                            }
                            popedLessSpender = uwsLQ.Dequeue();
                            pendingShare = eachPersonShare - popedLessSpender.AmountSpent;
                        }
                    }
                }
            }
            return sharedCost.Select(kvPair =>
            {
                var user = users.First(x => x.PersonId == kvPair.Key);
                return new SharedCostDisplayModel
                {
                    FirstName = user.User.FirstName,
                    LastName = user.User.LastName,
                    Payouts = kvPair.Value,
                    PersonId = user.PersonId
                };
            }).ToList();
        } 

        private void AddUserToList(ref Dictionary<int,List<IndiSplit>> sharedCost, int personToAddFor, double pendingShare, ExpData personAdded )
        {
            if (sharedCost.ContainsKey(personToAddFor))
            {
                sharedCost[personToAddFor].Add(new IndiSplit
                {
                    Amount = pendingShare,
                    FirstName = personAdded.FirstName,
                    LastName = personAdded.LastName,
                    PersonId = personAdded.PersonId
                });
            }
            else
            {
                sharedCost[personToAddFor] = new List<IndiSplit>
                                {
                                    new IndiSplit
                                    {
                                        Amount = pendingShare,
                                        FirstName = personAdded.FirstName,
                                        LastName = personAdded.LastName,
                                        PersonId = personAdded.PersonId
                                    }
                                };
            }
        }

    }

    class ExpData
    {
        public int PersonId { get; set; }
        public double AmountSpent { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
