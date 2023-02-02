using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VidFluentAI.Models;
using VidFluentAI.Services;

namespace VidFluentAI.Controllers
{
    public class DemoJobsController : Controller
    {
        private readonly DataContext _context;

        public DemoJobsController(DataContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "SuperAdmin")]
        // GET: DemoJobs
        public async Task<IActionResult> Index()
        {
              return View(await _context.DemoJobs.ToListAsync());
        }

        [Authorize(Roles = "SuperAdmin")]
        // GET: DemoJobs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.DemoJobs == null)
            {
                return NotFound();
            }

            var demoJob = await _context.DemoJobs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (demoJob == null)
            {
                return NotFound();
            }

            return View(demoJob);
        }

        // GET: DemoJobs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DemoJobs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Input,Output,EmailAddress")] DemoJob demoJob)
        {
            if (ModelState.IsValid)
            {
                _context.Add(demoJob);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(VideoSubmitted));
            }
            return View(demoJob);
        }

        public IActionResult VideoSubmitted()
        {
            return View();
        }

        [Authorize(Roles = "SuperAdmin")]
        // GET: DemoJobs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DemoJobs == null)
            {
                return NotFound();
            }

            var demoJob = await _context.DemoJobs.FindAsync(id);
            if (demoJob == null)
            {
                return NotFound();
            }
            return View(demoJob);
        }

        [Authorize(Roles = "SuperAdmin")]
        // POST: DemoJobs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Input,Output,JobType,EmailAddress")] DemoJob demoJob)
        {
            if (id != demoJob.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(demoJob);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DemoJobExists(demoJob.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(demoJob);
        }

        [Authorize(Roles = "SuperAdmin")]
        // GET: DemoJobs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.DemoJobs == null)
            {
                return NotFound();
            }

            var demoJob = await _context.DemoJobs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (demoJob == null)
            {
                return NotFound();
            }

            return View(demoJob);
        }

        [Authorize(Roles = "SuperAdmin")]
        // POST: DemoJobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DemoJobs == null)
            {
                return Problem("Entity set 'DataContext.DemoJobs'  is null.");
            }
            var demoJob = await _context.DemoJobs.FindAsync(id);
            if (demoJob != null)
            {
                _context.DemoJobs.Remove(demoJob);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DemoJobExists(int id)
        {
          return _context.DemoJobs.Any(e => e.Id == id);
        }
    }
}
