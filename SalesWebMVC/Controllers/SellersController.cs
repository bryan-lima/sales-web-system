using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalesWebMVC.Models;
using SalesWebMVC.Models.ViewModels;
using SalesWebMVC.Services;
using SalesWebMVC.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMVC.Controllers
{
    public class SellersController : Controller
    {
        private readonly SellerService _sellerService;
        private readonly DepartmentService _departmentService;

        public SellersController(SellerService sellerService, DepartmentService departmentService)
        {
            _sellerService = sellerService;
            _departmentService = departmentService;
        }

        public async Task<IActionResult> Index()
        {
            List<Seller> _sellers = await _sellerService.FindAllAsync();

            return View(_sellers);
        }

        public async Task<IActionResult> Create()
        {
            List<Department> departments = await _departmentService.FindAllAsync();
            SellerFormViewModel sellerFormViewModel = new SellerFormViewModel { Departments = departments };

            return View(sellerFormViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Seller seller)
        {
            if (!ModelState.IsValid)
            {
                List<Department> _departments = await _departmentService.FindAllAsync();
                SellerFormViewModel _sellerFormViewModel = new SellerFormViewModel { Seller = seller, Departments = _departments };

                return View(_sellerFormViewModel);
            }

            await _sellerService.InsertAsync(seller);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            var _seller = await _sellerService.FindByIdAsync(id.Value);

            if (_seller is null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }

            return View(_seller);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _sellerService.RemoveAsync(id);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            var _seller = await _sellerService.FindByIdAsync(id.Value);

            if (_seller is null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }

            return View(_seller);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            Seller _seller = await _sellerService.FindByIdAsync(id.Value);

            if (_seller is null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }

            List<Department> _departments = await _departmentService.FindAllAsync();

            SellerFormViewModel _sellerFormViewModel = new SellerFormViewModel
            {
                Seller = _seller,
                Departments = _departments
            };

            return View(_sellerFormViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Seller seller)
        {
            if (!ModelState.IsValid)
            {
                List<Department> _departments = await _departmentService.FindAllAsync();
                SellerFormViewModel _sellerFormViewModel = new SellerFormViewModel { Seller = seller, Departments = _departments };

                return View(_sellerFormViewModel);
            }

            if (id != seller.Id)
            {
                return RedirectToAction(nameof(Error), new { message = "Id mismatch" });
            }

            try
            {
                await _sellerService.Update(seller);
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException ex)
            {
                return RedirectToAction(nameof(Error), new { message = ex.Message });
            }
        }

        public IActionResult Error(string message)
        {
            ErrorViewModel _errorViewModel = new ErrorViewModel
            {
                Message = message,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };

            return View(_errorViewModel);
        }
    }
}
