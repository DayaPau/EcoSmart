using System.Security.Claims;
using EkoTrack.Data;
using EkoTrack.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EkoTrack.Controllers
{
    [Authorize(Roles = "CIUDADANO")]
    public class ReciclajeController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ReciclajeController(ApplicationDbContext db) => _db = db;

        private int CurrentUserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        // GET: /Reciclaje
        public async Task<IActionResult> Index(string? material, DateTime? desde, DateTime? hasta)
        {
            var q = _db.RecyclingRecords
                .Include(r => r.Material)
                .Where(r => r.UserId == CurrentUserId);

            if (!string.IsNullOrWhiteSpace(material))
                q = q.Where(r => r.Material!.Nombre.Contains(material));

            if (desde.HasValue) q = q.Where(r => r.Fecha >= desde.Value.Date);
            if (hasta.HasValue) q = q.Where(r => r.Fecha <= hasta.Value.Date);

            var lista = await q
                .OrderByDescending(r => r.Fecha)
                .ThenByDescending(r => r.Id)
                .ToListAsync();

            // Totales simples para mostrar
            ViewBag.TotalKg = lista.Sum(x => x.Cantidad);
            return View(lista);
        }

        // GET: /Reciclaje/Create
        public IActionResult Create()
        {
            ViewBag.Materiales = new SelectList(_db.Materials.OrderBy(m => m.Nombre), "Id", "Nombre");
            return View(new RecyclingRecord { Fecha = DateTime.Today });
        }

        // POST: /Reciclaje/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RecyclingRecord model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Materiales = new SelectList(_db.Materials.OrderBy(m => m.Nombre), "Id", "Nombre", model.MaterialId);
                return View(model);
            }

            model.UserId = CurrentUserId;
            _db.RecyclingRecords.Add(model);
            await _db.SaveChangesAsync();
            TempData["msg"] = "Registro creado.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Reciclaje/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var rec = await _db.RecyclingRecords.FindAsync(id);
            if (rec == null || rec.UserId != CurrentUserId) return NotFound();

            ViewBag.Materiales = new SelectList(_db.Materials.OrderBy(m => m.Nombre), "Id", "Nombre", rec.MaterialId);
            return View(rec);
        }

        // POST: /Reciclaje/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RecyclingRecord model)
        {
            var rec = await _db.RecyclingRecords.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
            if (rec == null || rec.UserId != CurrentUserId) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.Materiales = new SelectList(_db.Materials.OrderBy(m => m.Nombre), "Id", "Nombre", model.MaterialId);
                return View(model);
            }

            model.UserId = CurrentUserId; // asegurar ownership
            _db.RecyclingRecords.Update(model);
            await _db.SaveChangesAsync();
            TempData["msg"] = "Registro actualizado.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Reciclaje/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var rec = await _db.RecyclingRecords
                .Include(r => r.Material)
                .FirstOrDefaultAsync(r => r.Id == id && r.UserId == CurrentUserId);

            if (rec == null) return NotFound();
            return View(rec);
        }

        // POST: /Reciclaje/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rec = await _db.RecyclingRecords.FirstOrDefaultAsync(r => r.Id == id && r.UserId == CurrentUserId);
            if (rec == null) return NotFound();

            _db.RecyclingRecords.Remove(rec);
            await _db.SaveChangesAsync();
            TempData["msg"] = "Registro eliminado.";
            return RedirectToAction(nameof(Index));
        }
    }
}

