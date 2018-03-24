using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OMS.Data.Domain;
using OMS.Model.Customer;
using OMS.Services.Common;
using OMS.Services.Customer;

namespace OMS.Web.Controllers
{
    [UserAuthorize]
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly ICommonService _commonService;
        private readonly IAuthenticationService _authenticationsService;
        public CustomerController(ICustomerService customerService,IAuthenticationService authenticationService,ICommonService commonService)
        {
            _customerService = customerService;
            _authenticationsService = authenticationService;
            _commonService = commonService;
        }
        public  IActionResult Index()
        {
            List<Customers> list = _customerService.GetAllCustomerList();
            return View(list);
        }
        public IActionResult Add()
        {
            ViewBag.CustomerType =new SelectList(_commonService.GetBaseDictionaryList(DictionaryType.CustomerType),"Id","Value");
            ViewBag.PriceType =new SelectList(_commonService.GetBaseDictionaryList(DictionaryType.PriceType),"Id","Value");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Customers customer)
        {
            string name = customer.Name.ToString();
            if (_customerService.GetCountByName(name))
            {
                return RedirectToAction("Add");
            }
            else
            {
                _customerService.Add(customer);
                return RedirectToAction("Index");
            }
        }
        public IActionResult Del(int id)
        {
            _customerService.DelCustomerById(id);
            return RedirectToAction("Index");
        }
        public IActionResult Detail(int id)
        {
            ViewBag.CustomerType = new SelectList(_commonService.GetBaseDictionaryList(DictionaryType.CustomerType), "Id", "Value");
            ViewBag.PriceType = new SelectList(_commonService.GetBaseDictionaryList(DictionaryType.PriceType), "Id", "Value");
            var data=_customerService.GetById(id);
            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Detail(Customers customers)
        {
            if (ModelState.IsValid)
            {
                _customerService.UpdateCustomer(customers);
            }
            return RedirectToAction("Index");
        }
    }
}