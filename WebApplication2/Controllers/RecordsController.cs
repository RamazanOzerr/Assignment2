using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class RecordsController : Controller
    {
        private readonly WebApplication2Context _context;

        public RecordsController(WebApplication2Context context)
        {
            _context = context;
        }

        // GET: Records
        public async Task<IActionResult> Index()
        {
            return View(await _context.Record.ToListAsync());
        }

        // GET: Records/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Records/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Surname,Age")] Record @record)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@record);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(@record);
        }


        // GET: Records/DeleteByName
        public IActionResult Delete()
        {
            return View();
        }

        // POST: Records/DeleteByName
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteByName([Bind("Name")] Record record)
        {
            if (ModelState.IsValid)
            {
                var recordToDelete = await _context.Record.FirstOrDefaultAsync(r => r.Name == record.Name);
                if (recordToDelete != null)
                {
                    _context.Record.Remove(recordToDelete);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Record not found.");
                }
            }
            return View(record);
        }

        // GET: Records/SearchByName
        public IActionResult Search()
        {
            return View(new List<Record>());
        }

        // POST: Records/SearchByName
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SearchByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                ModelState.AddModelError("", "Please enter a name.");
                return View();
            }

            var records = await _context.Record
                .Where(r => r.Name.Contains(name))
                .ToListAsync();

            if (records == null || records.Count == 0)
            {
                ModelState.AddModelError("", "No records found.");
            }

            return View("SearchResults", records);
            
        }


        private bool RecordExists(int id)
        {
            return _context.Record.Any(e => e.Id == id);
        }
    }
}
