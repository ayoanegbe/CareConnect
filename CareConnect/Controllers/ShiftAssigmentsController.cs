using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CareConnect.CommonLogic.Data;
using CareConnect.CommonLogic.Models;
using Microsoft.AspNetCore.Authorization;

namespace CareConnect.Controllers
{
    [Authorize]
    public class ShiftAssigmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShiftAssigmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ShiftAssigments
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ShiftAssigments.Include(s => s.ShiftRun);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ShiftAssigments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shiftAssigment = await _context.ShiftAssigments
                .Include(s => s.ShiftRun)
                .FirstOrDefaultAsync(m => m.ShiftAssigmentId == id);
            if (shiftAssigment == null)
            {
                return NotFound();
            }

            return View(shiftAssigment);
        }

        // GET: ShiftAssigments/Create
        public IActionResult Create()
        {
            ViewData["ShiftId"] = new SelectList(_context.Shifts, "ShiftId", "Duration");
            return View();
        }

        // POST: ShiftAssigments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ShiftAssigmentId,ShiftId,AssignedTo,UserName,AssignedDate,IsDeclined,DateDeclined")] ShiftAssigment shiftAssigment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shiftAssigment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ShiftId"] = new SelectList(_context.Shifts, "ShiftId", "Duration", shiftAssigment.ShiftRunId);
            return View(shiftAssigment);
        }

        // GET: ShiftAssigments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shiftAssigment = await _context.ShiftAssigments.FindAsync(id);
            if (shiftAssigment == null)
            {
                return NotFound();
            }
            ViewData["ShiftId"] = new SelectList(_context.Shifts, "ShiftId", "Duration", shiftAssigment.ShiftRunId);
            return View(shiftAssigment);
        }

        // POST: ShiftAssigments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ShiftAssigmentId,ShiftId,AssignedTo,UserName,AssignedDate,IsDeclined,DateDeclined")] ShiftAssigment shiftAssigment)
        {
            if (id != shiftAssigment.ShiftAssigmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shiftAssigment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShiftAssigmentExists(shiftAssigment.ShiftAssigmentId))
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
            ViewData["ShiftId"] = new SelectList(_context.Shifts, "ShiftId", "Duration", shiftAssigment.ShiftRunId);
            return View(shiftAssigment);
        }

        // GET: ShiftAssigments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shiftAssigment = await _context.ShiftAssigments
                .Include(s => s.ShiftRun)
                .FirstOrDefaultAsync(m => m.ShiftAssigmentId == id);
            if (shiftAssigment == null)
            {
                return NotFound();
            }

            return View(shiftAssigment);
        }

        // POST: ShiftAssigments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shiftAssigment = await _context.ShiftAssigments.FindAsync(id);
            _context.ShiftAssigments.Remove(shiftAssigment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShiftAssigmentExists(int id)
        {
            return _context.ShiftAssigments.Any(e => e.ShiftAssigmentId == id);
        }
    }
}
