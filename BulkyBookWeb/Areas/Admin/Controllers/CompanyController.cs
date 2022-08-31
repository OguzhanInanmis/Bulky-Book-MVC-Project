using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitofwork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public CompanyController(IUnitOfWork unitofwork)
        {
            _unitofwork = unitofwork;
          
        }

        public IActionResult Index()
        {
            
            return View();
        }
       
        //Get
        public IActionResult Upsert(int? id)
        {
            Company company = new();
            
            if (id == null || id == 0)
            {
                //create product
                //ViewBag.CategoryList = CategoryList;
                //ViewData["CoverTypeList"] = CoverTypeList;
                return View(company);
            }
            else
            {
                //update product
                company =  _unitofwork.Company.GetFirstOrDefault(u => u.Id == id);
                return View(company);
            }


        }
        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company obj, IFormFile? file)
        {
            
            if (ModelState.IsValid)
            {
                               
                if(obj.Id == 0)
                {
                    _unitofwork.Company.Add(obj);
                    TempData["success"] = "Product created successfully";
                }
                else
                {
                    _unitofwork.Company.Update(obj);
                    TempData["success"] = "Product updated successfully";
                }
                _unitofwork.Save();
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var companyList = _unitofwork.Company.GetAll();
            return Json(new { data = companyList });
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var obj = _unitofwork.Company.GetFirstOrDefault(c => c.Id == id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _unitofwork.Company.Remove(obj);
            _unitofwork.Save();
            return Json(new { success = true, message = "Delete succesful" });
        }
        #endregion


    }


}
