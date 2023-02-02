using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VidFluentAI.Models;
using VidFluentAI.Services;

namespace VidFluentAI.Controllers
{
    [Authorize]
    public class JobsController : Controller
    {
        private readonly DataContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public JobsController(DataContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Jobs
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByNameAsync(User!.Identity!.Name);

            if (!user.IsSubscriptionActive) return RedirectToAction("Index", "Payments");

            return View(await _context.Jobs.Include(u => u.ApplicationUser).Where(j => j.ApplicationUser!.UserName == User!.Identity!.Name).ToListAsync());
        }

        // GET: Jobs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Jobs == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByNameAsync(User!.Identity!.Name);

            if (!user.IsSubscriptionActive) return RedirectToAction("Index", "Payments");

            var job = await _context.Jobs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (job == null)
            {
                return NotFound();
            }

            return View(job);
        }

        // GET: Jobs/Create
        public async Task<IActionResult> Create()
        {
            var user = await _userManager.FindByNameAsync(User!.Identity!.Name);

            if (!user.IsSubscriptionActive) return RedirectToAction("Index", "Payments");

            return View();
        }

        // POST: Jobs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Input,Output,JobType")] Job job)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(User!.Identity!.Name);

                if (!user.IsSubscriptionActive) return RedirectToAction("Index", "Payments");

                job.ApplicationUser = user;
                _context.Add(job);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(job);
        }

        // GET: Jobs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Jobs == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByNameAsync(User!.Identity!.Name);

            if (!user.IsSubscriptionActive) return RedirectToAction("Index", "Payments");

            var job = await _context.Jobs.FindAsync(id);
            if (job == null)
            {
                return NotFound();
            }
            return View(job);
        }

        // POST: Jobs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Input,Output,JobType,CreatedAt")] Job job)
        {
            if (id != job.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(User!.Identity!.Name);

                if (!user.IsSubscriptionActive) return RedirectToAction("Index", "Payments");

                try
                {
                    _context.Update(job);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobExists(job.Id))
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
            return View(job);
        }

        // GET: Jobs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Jobs == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByNameAsync(User!.Identity!.Name);

            if (!user.IsSubscriptionActive) return RedirectToAction("Index", "Payments");

            var job = await _context.Jobs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (job == null)
            {
                return NotFound();
            }

            return View(job);
        }

        // POST: Jobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Jobs == null)
            {
                return Problem("Entity set 'DataContext.Jobs'  is null.");
            }

            var user = await _userManager.FindByNameAsync(User!.Identity!.Name);

            if (!user.IsSubscriptionActive) return RedirectToAction("Index", "Payments");

            var job = await _context.Jobs.FindAsync(id);
            if (job != null)
            {
                _context.Jobs.Remove(job);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JobExists(int id)
        {
          return _context.Jobs.Any(e => e.Id == id);
        }
    }
}
