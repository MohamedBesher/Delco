using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Saned.Delco.Data.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Saned.Delco.Admin.Extensions;
using Saned.Delco.Admin.Models;
using PagedList;

namespace Saned.Delco.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class CityController : BasicController
    {
        // GET: City
        public ActionResult Index(Pager model)
        {
            try
            {
                ViewBag.Keyword = model.Keyword;
                var cities = _UnitOfWork.CityRepository.All();

               var result= cities.Where(x => string.IsNullOrEmpty(model.Keyword) ||
                           x.Name.Contains(model.Keyword) || x.Longitude.Contains(model.Keyword) || x.Latitude.Contains(model.Keyword)).ToList();

                return View(result.ToPagedList(model.Page, model.PageSize));
            }
            catch (Exception ex)
            {

                return View();
            }
           
        }




        public PartialViewResult Search(PathSearchModel model)
        {

            ViewBag.Keyword = model.Keyword;
            var cities = _UnitOfWork.CityRepository.All();

            var paths = cities.Where(x => string.IsNullOrEmpty(model.Keyword) ||
                         x.Name.Contains(model.Keyword) || x.Longitude.Contains(model.Keyword) || x.Latitude.Contains(model.Keyword)).ToList();

          

            ViewBag.ResultCount = paths.Count();

            int result = (paths.Count() / model.PageSize) + (paths.Count() % model.PageSize > 0 ? 1 : 0);
            if (model.Page > 1 && result < model.Page)
            {
                ViewBag.Page = model.Page - 1;
                return PartialView(paths.ToPagedList(model.Page - 1, model.PageSize));
            }
            else
                return PartialView(paths.ToPagedList(model.Page, model.PageSize));

        }


        public  ActionResult Add()
        {


            return View();
        }

       [HttpPost]
        public async Task<ActionResult> Add(CityViewModel city)
        {
            if (city != null && ModelState.IsValid)
            {
                var model = Mapper.Map<CityViewModel, City>(city);
                _UnitOfWork.CityRepository.Create(model);
                await _UnitOfWork.CommitAsync();
                var message = "تم اضافة المدينة بنجاح .";
                return RedirectToAction("Index", "City").Success(message);
            }

            return View(city);
        }
      
        public ActionResult Update(long id)
        {
           var model= _UnitOfWork.CityRepository.Find(id);
            var cityModel = Mapper.Map<City, CityViewModel>(model);
            return View(cityModel);
        }
        [HttpPost]
        public async Task<ActionResult> Update(CityViewModel city)
        {
            if (city != null && ModelState.IsValid)
            {
               var model = Mapper.Map<CityViewModel, City>(city);
                _UnitOfWork.CityRepository.Update(model);
                await _UnitOfWork.CommitAsync();
                var message = "تم تعديل المدينة بنجاح .";
                return RedirectToAction("Index", "City").Success(message);
            }

            return View(city);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public async Task<ActionResult> Destroy(long id)
        {

            try
            {
                var model =  _UnitOfWork.CityRepository.Find(id);

                if (model != null)
                {
                  _UnitOfWork.CityRepository.Delete(model);
                   await _UnitOfWork.CommitAsync();
                    return Json(0, JsonRequestBehavior.AllowGet);

                }
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(2, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult Delete(Pager search)
        {
            try
            {


                var path = _UnitOfWork.CityRepository.Find(search.Id);
                if (path == null)
                    return new HttpStatusCodeResult(404, "NotFound");


               // AspNetUsers
                var users = _repo.FindUsersInCity(search.Id);
                 if(users)
                    return new HttpStatusCodeResult(404, "UserUsedCity");
                //Requests
                var requests = _UnitOfWork.RequestRepository.All().Any(u => u.CityId == search.Id);
                if (requests)
                    return new HttpStatusCodeResult(404, "RequestsUsedCity");
                //paths
                var paths = _UnitOfWork.PathRepository.All().Any(u => u.FromCityId == search.Id || u.ToCityId==search.Id);
                if (paths)
                    return new HttpStatusCodeResult(404, "PathsUsedCity");



                _UnitOfWork.CityRepository.Delete(path);
                _UnitOfWork.Commit();
                return RedirectToAction("Search", new { Page = search.Page, PageSize = search.PageSize});
            }
            catch (Exception e)
            {


                return new HttpStatusCodeResult(404, "Error");


            }

        }
    }
}