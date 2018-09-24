using PagedList;
using Saned.Delco.Admin.Extensions;
using Saned.Delco.Admin.Models;
using Saned.Delco.Data.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Saned.Delco.Admin.Controllers
{
    public class PassengerNumbersController : BasicController
    {
        // GET: PassengerNumbers
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult Search(PassengerNumberSearchModel search)
        {

            search.Page = search.Page == 0 ? 1 : search.Page;
            ViewBag.Page = search.Page;
            ViewBag.PageSize = search.PageSize;
            ViewBag.Keyword = search.Keyword;

            decimal searchPrice = 0;

            decimal.TryParse(search.Keyword, out searchPrice);

            var passengerNumbers = _UnitOfWork.PassengerNumberRepository.All().Where(x =>
               string.IsNullOrEmpty(search.Keyword) ||(x.Name.ToLower().Contains(search.Keyword.ToLower()) || x.Value.ToString().Contains(search.Keyword)))  
            .OrderByDescending(x => x.Value);

            ViewBag.ResultCount = passengerNumbers.Count();

            int result = (passengerNumbers.Count() / search.PageSize) + (passengerNumbers.Count() % search.PageSize > 0 ? 1 : 0);
            if (search.Page > 1 && result < search.Page)
            {
                ViewBag.Page = search.Page - 1;
                return PartialView(passengerNumbers.ToPagedList(search.Page - 1, search.PageSize));
            }
            else
                return PartialView(passengerNumbers.ToPagedList(search.Page, search.PageSize));

        }

        //return PartialView(data);


        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(PassengerNumberViewModel model)
        {
            try
            {
                if (model.Value == 0)
                {
                    ModelState.AddModelError("Price", "القيمة يجب ان تكون اكبر من 0");
                }

                if (!ModelState.IsValid)
                {
                    return View("Add", model);
                }

                var passengerNumber = new PassengerNumber()
                {
                    Name = model.Name,
                    Value = model.Value
                };

                _UnitOfWork.PassengerNumberRepository.Create(passengerNumber);
                _UnitOfWork.Commit();
                var message = string.Format("تم الاضافة  بنجاح .");
                return RedirectToAction("Index", "PassengerNumbers").Success(message);

            }
            catch (Exception e)
            {
                var message = string.Format("حدث خطأ أثناء الحفظ");
                return RedirectToAction("Add", "PassengerNumbers").Error(message);
            }

        }

        public ActionResult Edit(int id)
        {

            var passengerNumber = _UnitOfWork.PassengerNumberRepository.Find(id);

            if (passengerNumber == null)
                return HttpNotFound();

            var viewModel = new PassengerNumberViewModel()
            {
                Id = passengerNumber.Id,
                Name = passengerNumber.Name,
                Value = passengerNumber.Value

            };

            return View("Edit", viewModel);
        }

        [HttpPost]
        public ActionResult Edit(PassengerNumberViewModel model)
        {
            try
            {
                if (model.Value == 0)
                {
                    ModelState.AddModelError("Value", "القيمة يجب ان تكون اكبر من 0");
                }

                if (!ModelState.IsValid)
                {
                    return View("Edit", model);
                }
                var path = _UnitOfWork.PassengerNumberRepository.Find(model.Id);
                if (path == null)
                    return HttpNotFound("Path Not Exist!");

                path.Name = model.Name;
                path.Value = model.Value;
                _UnitOfWork.PassengerNumberRepository.Update(path);
                _UnitOfWork.Commit();

                var message = string.Format("تم تعديل عدد الركاب بنجاح .");
                return RedirectToAction("Index", "PassengerNumbers").Success(message);
            }
            catch (Exception e)
            {
                var message = string.Format("حدث خطأ أثناء الحفظ");
                return RedirectToAction("Edit", "PassengerNumbers").Error(message);
            }

        }


        public ActionResult Delete(PassengerNumberSearchModel search)
        {
            try
            {
                var passengerNumber = _UnitOfWork.PassengerNumberRepository.All()
                    .Include(x => x.Requestes).FirstOrDefault(x => x.Id == search.Id);

                if (passengerNumber == null)
                    return new HttpStatusCodeResult(404, "NotFound");
                else
                {
                    if (!passengerNumber.Requestes.Any())
                    {
                        _UnitOfWork.PassengerNumberRepository.Delete(passengerNumber);
                        _UnitOfWork.Commit();
                        return RedirectToAction("Search", new { Page = search.Page, PageSize = search.PageSize });
                    }
                    else
                    {
                        return new HttpStatusCodeResult(404, "NotFound");
                    }
                }


            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(404, "NotFound");
            }

        }

    }
}