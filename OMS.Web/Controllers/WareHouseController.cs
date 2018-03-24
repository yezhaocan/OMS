using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using OMS.Data.Domain;
using OMS.Services;

namespace OMS.Web.Controllers
{
    [UserAuthorize]
    public class WareHouseController : Controller
    {
        private readonly IWareHouseService _wareHouseService;
        //private readonly IWareHouseService _commonService;
        private readonly IAuthenticationService _authenticationsService;
        public WareHouseController(IWareHouseService wareHouseService, IAuthenticationService authenticationService, IWareHouseService commonService)
        {
            _wareHouseService = wareHouseService;
            _authenticationsService = authenticationService;
            //_commonService = commonService;
        }
        public IActionResult Index()
        {
           List<WareHouse> list= _wareHouseService.GetAllWareHouseList();
            return View(list);
        }
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(WareHouse wareHouse)
        {
            string name = wareHouse.Name.ToString();
                if (_wareHouseService.GetCountByName(name))
                {
                    return View(wareHouse);
                }
                else
                {
                    _wareHouseService.Add(wareHouse);
                    return RedirectToAction("Index");
                }
        }
        public IActionResult Detail(int id)
        {
            var data = _wareHouseService.GetById(id);
            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Detail(WareHouse wareHouse)
        {
            if (ModelState.IsValid)
            {
                _wareHouseService.UpdateWareHouse(wareHouse);
            }
            return RedirectToAction("Index");
        }
        public IActionResult Del(int id)
        {
            _wareHouseService.DelWareHouseById(id);
            return RedirectToAction("Index");
        }
    }
}