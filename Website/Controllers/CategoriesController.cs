using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRUD_Application.Data;
using CRUD_Application.Models;
namespace CRUD_Application.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: Categories
        public async Task<IActionResult> Index()
        {
            return View(await _context.Category.ToListAsync());
        }
        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }
        // POST: Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Category.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }
        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _context.Category.FindAsync(id);
            if (category == null) return NotFound();
            return View(category);
        }
        // POST: Categories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if (id != category.Id) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Category.Update(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }
        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var category = await _context.Category
            .FirstOrDefaultAsync(c => c.Id == id);
            if (category == null) return NotFound();
            return View(category);
        }
        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Category
            .FirstOrDefaultAsync(c => c.Id == id);
            if (category == null) return NotFound();
            return View(category);
        }
        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Category.FindAsync(id);
            if (category != null)
            {
                _context.Category.Remove(category);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AjaxDelete(int id)
        {
            var category = await _context.Category.FindAsync(id);
            if (category == null)
                return NotFound(new { success = false, message = "Category not found." });
            _context.Category.Remove(category);
            await _context.SaveChangesAsync();
            return Json(new { success = true, id });
        }
    }
}