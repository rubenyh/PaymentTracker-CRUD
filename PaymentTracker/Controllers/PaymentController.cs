using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class PaymentsController : Controller
{
    private readonly ApplicationDbContext _context;

    public PaymentsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Payments
    public async Task<IActionResult> Index()
    {
        var payments = await _context.Payments.ToListAsync();
        return View(payments);
    }

    // GET: Payments/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Payments/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Payment payment)
    {
        if (!ModelState.IsValid)
            return View(payment);

        try
        {
            // Evitar duplicados solo si Title y Category son iguales
            bool exists = await _context.Payments
                .AnyAsync(p => p.Title == payment.Title &&
                               p.Category == payment.Category);

            if (exists)
            {
                ModelState.AddModelError("", "A payment with the same Title and Category already exists.");
                return View(payment);
            }

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateException ex)
        {
            // Captura errores de SQL
            ModelState.AddModelError("", "Error saving to the database: " + ex.Message);
            return View(payment);
        }
        catch (Exception ex)
        {
            // Otros errores inesperados
            ModelState.AddModelError("", "An unexpected error occurred: " + ex.Message);
            return View(payment);
        }
    }

    // GET: Payments/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var payment = await _context.Payments.FindAsync(id);
        if (payment == null) return NotFound();
        return View(payment);
    }

    // POST: Payments/Edit/5
    [HttpPost]
    public async Task<IActionResult> Edit(Payment payment)
    {
        if (!ModelState.IsValid)
            return View(payment);

        try
        {
            bool exists = await _context.Payments
                    .AnyAsync(p => p.Title == payment.Title &&
                   p.Category == payment.Category);

            if (exists)
            {
                ModelState.AddModelError("", "A payment with the same Title and Category already exists.");
                return View(payment);
            }


            _context.Update(payment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateException ex)
        {
            ModelState.AddModelError("", $"No se puede actualizar: conflicto con un pago existente: {ex.Message}");
            return View(payment);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", $"Error inesperado: {ex.Message}");
            return View(payment);
        }
    }

    // POST: Payments/Delete/5
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment != null)
            {
                _context.Payments.Remove(payment);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"No se pudo eliminar el pago: {ex.Message}";
            return RedirectToAction(nameof(Index));
        }
    }

    // POST: Payments/DeleteSelected
    [HttpPost]
    public async Task<IActionResult> DeleteSelected(int[] selectedIds)
    {
        try
        {
            var payments = _context.Payments.Where(p => selectedIds.Contains(p.Id));
            _context.Payments.RemoveRange(payments);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"No se pudieron eliminar los pagos seleccionados: {ex.Message}";
            return RedirectToAction(nameof(Index));
        }
    }
}
