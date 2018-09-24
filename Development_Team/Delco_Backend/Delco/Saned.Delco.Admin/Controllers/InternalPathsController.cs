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
    public class InternalPathsController : BasicController
    {
        // GET: path
        public ActionResult Index()
        {

            return View();
        }

        private void PopulateCities()
        {

            var cities = _UnitOfWork.CityRepository
                .All()
                .OrderBy(e => e.Name);
            ViewBag.cities = cities;

        }

        public PartialViewResult Search(PathSearchModel search)
        {

            search.Page = search.Page == 0 ? 1 : search.Page;
            ViewBag.Page = search.Page;
            ViewBag.PageSize = search.PageSize;
            ViewBag.Keyword = search.Keyword;

            //decimal searchPrice = 0;

            //decimal.TryParse(search.Keyword, out searchPrice);

            var paths = _UnitOfWork.PathRepository.All()
                .Include(u => u.FromCity)
                .Where(u => u.Type == PathTypesEnum.Internal &&
                    (string.IsNullOrEmpty(search.Keyword) || u.FromCity.Name.Contains(search.Keyword))
                    //||(searchPrice == 0 || u.Price == searchPrice)
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
        public ActionResult Add(InternalPathViewModel model)
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
                    Price = model.Price,
                    Type = PathTypesEnum.Internal
                };

                _UnitOfWork.PathRepository.Create(path);
                _UnitOfWork.Commit();
                var message = string.Format("تم اضافة المسار بنجاح .");
                return RedirectToAction("Index", "InternalPaths").Success(message);

            }
            catch (Exception e)
            {
                var message = string.Format("حدث خطأ أثناء الحفظ");
                return RedirectToAction("Add", "InternalPaths").Error(message);
            }

        }

        public ActionResult Edit(int id)
        {

            var path = _UnitOfWork.PathRepository.Find(id);

            if (path == null)
                return HttpNotFound();

            PopulateCities();
            var viewModel = new InternalPathViewModel()
            {
                FromCityId = path.FromCityId,
                Price = path.Price,
                Type = PathTypesEnum.Internal

            };

            return View("Edit", viewModel);
        }

        [HttpPost]
        public ActionResult Edit(InternalPathViewModel model)
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
                path.Price = model.Price;
                _UnitOfWork.PathRepository.Update(path);
                _UnitOfWork.Commit();

                var message = string.Format("تم تعديل المسار بنجاح .");
                return RedirectToAction("Index", "InternalPaths").Success(message);
            }
            catch (Exception e)
            {
                var message = string.Format("حدث خطأ أثناء الحفظ");
                return RedirectToAction("Edit", "InternalPaths").Error(message);
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