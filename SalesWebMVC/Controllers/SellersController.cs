using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalesWebMVC.Models;
using SalesWebMVC.Models.ViewModels;
using SalesWebMVC.Services;
using SalesWebMVC.Services.Exceptions;
using System;
using System.Collections.Generic;
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

        public IActionResult Index()
        {
            List<Seller> _sellers = _sellerService.FindAll();

            return View(_sellers);
        }

        public IActionResult Create()
        {
            List<Department> departments = _departmentService.FindAll();
            SellerFormViewModel sellerFormViewModel = new SellerFormViewModel { Departments = departments };

            return View(sellerFormViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Seller seller)
        {
            _sellerService.Insert(seller);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var _seller = _sellerService.FindById(id.Value);

            if (_seller is null)
            {
                return NotFound();
            }

            return View(_seller);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _sellerService.Remove(id);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var _seller = _sellerService.FindById(id.Value);

            if (_seller is null)
            {
                return NotFound();
            }

            return View(_seller);
        }

        public IActionResult Edit(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            Seller _seller = _sellerService.FindById(id.Value);

            if (_seller is null)
            {
                return NotFound();
            }

            List<Department> _departments = _departmentService.FindAll();

            SellerFormViewModel _sellerFormViewModel = new SellerFormViewModel
            {
                Seller = _seller,
                Departments = _departments
            };

            return View(_sellerFormViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Seller seller)
        {
            if (id != seller.Id)
            {
                return BadRequest();
            }

            try
            {
                _sellerService.Update(seller);
                return RedirectToAction(nameof(Index));
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }
        }
    }
}
