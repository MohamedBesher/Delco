using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Saned.Delco.Admin.Models;
using Saned.Delco.Data.Core.Models;

namespace Saned.Delco.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class SettingController : BasicController
    {
        // GET: Setting
        public ActionResult Index()
        {
          var model=  _UnitOfWork.SettingRepository.All().First();
            var requestModel = Mapper.Map<Setting, SettingViewModel>(model);
            return View(requestModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(SettingViewModel model)
        {
            if (ModelState.IsValid)
            {
                var requestModel = Mapper.Map<SettingViewModel, Setting>(model);
                _UnitOfWork.SettingRepository.Updated(requestModel);
                await  _UnitOfWork.CommitAsync();
            }
            return View(model);
        }

    }
}