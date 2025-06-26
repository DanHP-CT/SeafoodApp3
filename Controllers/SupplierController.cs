using Microsoft.AspNetCore.Mvc;
using SeafoodApp.Models.Entities;
using SeafoodApp.Models.ViewModels;
using SeafoodApp.Services;
using System.Linq;
using System.Threading.Tasks;

namespace SeafoodApp.Controllers
{
    public class SupplierController : Controller
    {
        private readonly ISupplierService _supplierService;

        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        public async Task<IActionResult> Index()
        {
            var suppliers = await _supplierService.GetAllSuppliersAsync();
            var viewModel = suppliers.Select(s => new SupplierViewModel
            {
                Id = s.Id,
                Name = s.Name,
                Address = s.Address,
                TaxCode = s.TaxCode,
                ContactPerson = s.ContactPerson,
                Phone = s.Phone
            }).ToList();
            return View(viewModel);
        }

        public IActionResult Create()
        {
            return View(new SupplierViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SupplierViewModel model)
        {
            if (ModelState.IsValid)
            {
                var supplier = new Supplier
                {
                    Name = model.Name,
                    Address = model.Address,
                    TaxCode = model.TaxCode,
                    ContactPerson = model.ContactPerson,
                    Phone = model.Phone
                };
                await _supplierService.CreateSupplierAsync(supplier);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var supplier = await _supplierService.GetSupplierByIdAsync(id);
            if (supplier == null)
            {
                return NotFound();
            }
            var viewModel = new SupplierViewModel
            {
                Id = supplier.Id,
                Name = supplier.Name,
                Address = supplier.Address,
                TaxCode = supplier.TaxCode,
                ContactPerson = supplier.ContactPerson,
                Phone = supplier.Phone
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SupplierViewModel model)
        {
            if (ModelState.IsValid)
            {
                var supplier = await _supplierService.GetSupplierByIdAsync(model.Id);
                if (supplier == null)
                {
                    return NotFound();
                }
                supplier.Name = model.Name;
                supplier.Address = model.Address;
                supplier.TaxCode = model.TaxCode;
                supplier.ContactPerson = model.ContactPerson;
                supplier.Phone = model.Phone;
                await _supplierService.UpdateSupplierAsync(supplier);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var supplier = await _supplierService.GetSupplierByIdAsync(id);
            if (supplier == null)
            {
                return NotFound();
            }
            var viewModel = new SupplierViewModel
            {
                Id = supplier.Id,
                Name = supplier.Name,
                Address = supplier.Address,
                TaxCode = supplier.TaxCode,
                ContactPerson = supplier.ContactPerson,
                Phone = supplier.Phone
            };
            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _supplierService.DeleteSupplierAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}