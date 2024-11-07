using CityLibrary.Models;
using CityLibrary.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CityLibrary.Controllers
{
    public class BorrowerController : Controller
    {
        private readonly BorrowerService _borrowerService;

        public BorrowerController(BorrowerService borrowerService)
        {
            _borrowerService = borrowerService;
        }

        // Index action to list all borrowers
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var borrowers = await _borrowerService.GetAllBorrowersAsync();
            return View(borrowers);
        }

        // Display the create form
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Handle the creation of a new borrower
        [HttpPost]
        public async Task<IActionResult> Create(Borrower borrower)
        {
            if (ModelState.IsValid)
            {
                await _borrowerService.AddBorrowerAsync(borrower);
                return RedirectToAction("Details", new { partitionKey = borrower.PartitionKey, rowKey = borrower.RowKey });
            }
            return View(borrower);
        }

        // Display details of a borrower
        [HttpGet]
        public async Task<IActionResult> Details(string partitionKey, string rowKey)
        {
            var borrower = await _borrowerService.GetBorrowerAsync(partitionKey, rowKey);
            if (borrower == null)
            {
                return NotFound();
            }
            // Ensure Total is updated
            borrower.UpdateTotal();

            return View(borrower);
        }

        // Handle the deletion of a borrower
        [HttpPost]
        public async Task<IActionResult> Delete(string partitionKey, string rowKey)
        {
            await _borrowerService.DeleteBorrowerAsync(partitionKey, rowKey);
            return RedirectToAction("Index");
        }
    }
}
