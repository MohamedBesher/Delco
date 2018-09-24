using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Results;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using PagedList;
using Saned.Delco.Admin.Extensions;
using Saned.Delco.Admin.Models;
using Saned.Delco.Data.Core.Enum;
using Saned.Delco.Data.Core.Models;

namespace Saned.Delco.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ExternalPathsController : BasicController
    {
        // GET: path
        public ActionResult Index()
        {

            PopulateCities();
            return View();
        }

        private void PopulateCities()
        {

            var cities = _UnitOfWork.CityRepository
                .All()
                .OrderBy(e => e.Name);

            ViewBag.cities = cities;

        }


        public ActionResult ExternalPaths_Read([DataSourceRequest] DataSourceRequest request)
        {
            return Json(_UnitOfWork.PathRepository.All()
                .Include(u => u.FromCity)
                .Include(u => u.ToCity)
                .Where(u => u.Type == PathTypesEnum.External).ToDataSourceResult(request));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> ExternalPaths_Create([DataSourceRequest] DataSourceRequest request, Path path)
        {
            if (path != null && ModelState.IsValid)
            {
                _UnitOfWork.PathRepository.Create(path);
                await _UnitOfWork.CommitAsync();
            }

            return Json(new[] { path }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> ExternalPaths_Update([DataSourceRequest] DataSourceRequest request, Path path)
        {
           
            if (path != null && ModelState.IsValid)
            {
                _UnitOfWork.PathRepository.Update(path);
                await _UnitOfWork.CommitAsync();
            }

            return Json(new[] { path }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> ExternalPaths_Destroy([DataSourceRequest] DataSourceRequest request, Path path)
        {
            if (path != null)
            {
                try
                {
                    var oldpath = _UnitOfWork.PathRepository.Find(x => x.Id == path.Id);
                    if (oldpath != null)
                    {
                        _UnitOfWork.PathRepository.Delete(oldpath);
                        await _UnitOfWork.CommitAsync();
                    }
                }
                catch (Exception ex)
                {

                }
            }

            return Json(new[] { path }.ToDataSourceResult(request, ModelState));
        }




        public PartialViewResult Search(PathSearchModel search)
        {

            search.Page = search.Page == 0 ? 1 : search.Page;
            ViewBag.Page = search.Page;
            ViewBag.PageSize = search.PageSize;
            ViewBag.Keyword = search.Keyword;
            ViewBag.FromCityId = search.FromCityId;
            ViewBag.ToCityId = search.ToCityId;

            var paths = _UnitOfWork.PathRepository.All()
                .Include(u => u.FromCity)
                .Include(u => u.ToCity)
                .Where(u => u.Type == PathTypesEnum.External &&

                    (search.FromCityId == 0 || u.FromCityId == search.FromCityId) &&
                    (search.ToCityId == 0 || u.ToCityId == search.ToCityId)

                    ).OrderBy(u => u.Id);

            ViewBag.ResultCount = paths.Count();

            int result = (paths.Count() / search.PageSize) + (paths.Count() % search.PageSize > 0 ? 1 : 0);
            if (search.Page > 1 && result < search.Page)
            {
                ViewBag.Page = search.Page - 1;
                return PartialView(paths.ToPagedList(search.Page - 1, search.PageSize));
            }
            else
                return PartialView(paths.ToPagedList(search.Page, search.PageSize));

        }

        //return PartialView(data);


        public ActionResult Add()
        {
            PopulateCities();
            return View();
        }
        [HttpPost]
        public ActionResult Add(ExternalPathViewModel model)
        {
            try
            {
                if (model.Price == 0)
                {
                    ModelState.AddModelError("Price", "السعر يجب ان يكون اكبر من 0");
                }

                if (!ModelState.IsValid)
                {
                    PopulateCities();
                    return View("Add", model);
                }

                var path = new Path()
                {

                    FromCityId = model.FromCityId,
                    ToCityId = model.ToCityId,
                    Price = model.Price,
                    Type = PathTypesEnum.External
                };

                _UnitOfWork.PathRepository.Create(path);
                _UnitOfWork.Commit();
                var message = string.Format("تم اضافة المسار بنجاح .");
                return RedirectToAction("Index", "ExternalPaths").Success(message);

            }
            catch (Exception e)
            {
                var message = string.Format("حدث خطأ أثناء الحفظ");
                return RedirectToAction("Add", "ExternalPaths").Error(message);
            }

        }

        public ActionResult Edit(int id)
        {

            var path = _UnitOfWork.PathRepository.Find(id);

            if (path == null)
                return HttpNotFound();

            PopulateCities();
            var viewModel = new ExternalPathViewModel()
            {
                FromCityId = path.FromCityId,
                ToCityId = path.ToCityId,
                Price = path.Price,
                Type = PathTypesEnum.External

            };

            return View("Edit", viewModel);
        }

        [HttpPost]
        public ActionResult Edit(ExternalPathViewModel model)
        {
            try
            {
                if (model.Price == 0)
                {
                    ModelState.AddModelError("Price", "السعر يجب ان يكون اكبر من 0");
                }

                if (!ModelState.IsValid)
                {
                    PopulateCities();
                    return View("Edit", model);

                }
                var path = _UnitOfWork.PathRepository.Find(model.Id);
                if (path == null)
                    return HttpNotFound("Path Not Exist!");

                path.FromCityId = model.FromCityId;
                path.ToCityId = model.ToCityId;
                path.Price = model.Price;
                _UnitOfWork.PathRepository.Update(path);
                _UnitOfWork.Commit();

                var message = string.Format("تم تعديل المسار بنجاح .");
                return RedirectToAction("Index", "ExternalPaths").Success(message);
            }
            catch (Exception e)
            {
                var message = string.Format("حدث خطأ أثناء الحفظ");
                return RedirectToAction("Edit", "ExternalPaths").Error(message);
            }

        }


        public ActionResult Delete(PathSearchModel search)
        {
            try
            {

                var path = _UnitOfWork.PathRepository.Find(search.Id);
                if (path == null)
                    return new HttpStatusCodeResult(404, "NotFound");


                _UnitOfWork.PathRepository.Delete(path);
                _UnitOfWork.Commit();
                return RedirectToAction("Search", new { Page = search.Page, PageSize = search.PageSize, FromCityId = search.FromCityId, ToCityId = search.ToCityId });
            }
            catch (Exception e)
            {


                return new HttpStatusCodeResult(404, "NotFound");


            }

        }






    }
}