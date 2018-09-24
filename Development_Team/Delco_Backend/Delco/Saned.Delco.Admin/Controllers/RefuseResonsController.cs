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
    public class RefuseResonsController : BasicController
    {
        // GET: path
        public ActionResult Index()
        {

            return View();
        }

            

        public PartialViewResult Search(Pager search)
        {
            
            search.Page = search.Page == 0 ? 1 : search.Page;
            ViewBag.Page = search.Page;
            ViewBag.PageSize = search.PageSize;
            ViewBag.Keyword = search.Keyword;
          

            var paths = _UnitOfWork.RefuseReasonsRepository.All()
                .Where(u => 
                    (string.IsNullOrEmpty(search.Keyword) || u.Value.Contains(search.Keyword))                                      
                    ).OrderBy(u=>u.Id);

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
            return View();
        }
        [HttpPost]
        public ActionResult Add(RefuseResonViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("Add", model);
                }

                var refuse = new RefuseReason()
                {

                    Value = model.Value,
                   
                };

                _UnitOfWork.RefuseReasonsRepository.Create(refuse);
                _UnitOfWork.Commit();
                var message = string.Format("تم اضافة سبب الرفض بنجاح .");
                return RedirectToAction("Index", "RefuseResons").Success(message);

            }
            catch (Exception e)
            {
                var message = string.Format("حدث خطأ أثناء الحفظ");
                return RedirectToAction("Add", "RefuseResons").Error(message);
            }

        }

        public ActionResult Edit(int id)
        {

            var refuse = _UnitOfWork.RefuseReasonsRepository.Find(id);

            if (refuse == null)
                 return HttpNotFound();

            var viewModel = new RefuseResonViewModel()
            {
                Value = refuse.Value


            };

            return View("Edit", viewModel);
        }

        [HttpPost]
        public ActionResult Edit(RefuseResonViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("Edit", model);

                }
                var refuse = _UnitOfWork.RefuseReasonsRepository.Find(model.Id);
                if (refuse == null)
                    return HttpNotFound("refuse Not Exist!");

                refuse.Value = model.Value;
                _UnitOfWork.RefuseReasonsRepository.Update(refuse);
                _UnitOfWork.Commit();

                var message = string.Format("تم تعديل سبب الرفض  بنجاح .");
                return RedirectToAction("Index", "RefuseResons").Success(message);
            }
            catch (Exception e)
            {
                var message = string.Format("حدث خطأ أثناء الحفظ");
                return RedirectToAction("Edit", "RefuseResons").Error(message);
            }

        }


        public ActionResult Delete(Pager search)
        {
            try
            {

                var path = _UnitOfWork.RefuseReasonsRepository.Find(search.Id);
                if (path == null)                    
                    return new HttpStatusCodeResult(404, "NotFound");


                _UnitOfWork.RefuseReasonsRepository.Delete(path);
                _UnitOfWork.Commit();
                return RedirectToAction("Search", new { Page = search.Page,PageSize=search.PageSize ,Keyword=search.Keyword});
            }
            catch (Exception e)
            {
                

                return new HttpStatusCodeResult(404, "AlreadyUsed");


            }

        }






    }
}