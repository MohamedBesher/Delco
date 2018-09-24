using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using PagedList;
using Saned.Delco.Admin.Error;
using Saned.Delco.Admin.Extensions;
using Saned.Delco.Admin.Models;
using Saned.Delco.Data.Core.Dto;
using Saned.Delco.Data.Core.Enum;
using Saned.Delco.Data.Core.Models;

namespace Saned.Delco.Admin.Controllers
{
    [System.Web.Mvc.Authorize(Roles = "Administrator")]
    public class RequestsController : BasicController



    {
        // GET: path
        public ActionResult Index()
        {
            PopulateCities();
            return View();
        }


        public ActionResult InProgress()
        {
            PopulateCities();
            return View();
        }
        public ActionResult Canceled()
        {
            PopulateCities();
            return View();
        }
        public ActionResult Delivered()
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



        public PartialViewResult Search(RequestSearchModel search)
        {
            
            search.Page = search.Page == 0 ? 1 : search.Page;
            ViewBag.PageSize = search.PageSize;
            ViewBag.Keyword = search.Keyword;
            ViewBag.UserId = search.UserId;
            ViewBag.AgentId = search.AgentId;
            ViewBag.CityId = search.CityId;
            ViewBag.Status = search.Status;
            search.Status=RequestStatusEnum.New;
            search.Type=RequestTypeEnum.Request;

            var paths = SearchRequests(search);

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

     

        public PartialViewResult SearchCanceled(RequestSearchModel search)
        {

            search.Page = search.Page == 0 ? 1 : search.Page;
            ViewBag.PageSize = search.PageSize;
            ViewBag.Keyword = search.Keyword;
            ViewBag.UserId = search.UserId;
            ViewBag.AgentId = search.AgentId;
            ViewBag.CityId = search.CityId;
            ViewBag.Status = search.Status;
            search.Status = RequestStatusEnum.Canceled;
            search.Type = RequestTypeEnum.Request;


            var paths = SearchRequests(search);

            ViewBag.ResultCount = paths.Count();

            int result = (paths.Count() / search.PageSize) + (paths.Count() % search.PageSize > 0 ? 1 : 0);
            if (search.Page > 1 && result < search.Page)
            {
                ViewBag.Page = search.Page - 1;
                return PartialView("Search", paths.ToPagedList(search.Page - 1, search.PageSize));
            }
            else
                return PartialView("Search", paths.ToPagedList(search.Page, search.PageSize));

        }


        public PartialViewResult SearchInProgress(RequestSearchModel search)
        {

            search.Page = search.Page == 0 ? 1 : search.Page;
            ViewBag.PageSize = search.PageSize;
            ViewBag.Keyword = search.Keyword;
            ViewBag.UserId = search.UserId;
            ViewBag.AgentId = search.AgentId;
            ViewBag.CityId = search.CityId;
            ViewBag.Status = search.Status;
            search.Status = RequestStatusEnum.Inprogress;
            search.Type = RequestTypeEnum.Request;


            var paths = SearchRequests(search);

            ViewBag.ResultCount = paths.Count();

            int result = (paths.Count() / search.PageSize) + (paths.Count() % search.PageSize > 0 ? 1 : 0);
            if (search.Page > 1 && result < search.Page)
            {
                ViewBag.Page = search.Page - 1;
                return PartialView("Search", paths.ToPagedList(search.Page - 1, search.PageSize));
            }
            else
                return PartialView("Search", paths.ToPagedList(search.Page, search.PageSize));

        }


        public PartialViewResult SearchDelivered(RequestSearchModel search)
        {

            search.Page = search.Page == 0 ? 1 : search.Page;
            ViewBag.PageSize = search.PageSize;
            ViewBag.Keyword = search.Keyword;
            ViewBag.UserId = search.UserId;
            ViewBag.AgentId = search.AgentId;
            ViewBag.CityId = search.CityId;
            ViewBag.Status = search.Status;
            search.Status = RequestStatusEnum.Delivered;
            search.Type = RequestTypeEnum.Request;


            var paths = SearchRequests(search);

            ViewBag.ResultCount = paths.Count();

            int result = (paths.Count() / search.PageSize) + (paths.Count() % search.PageSize > 0 ? 1 : 0);
            if (search.Page > 1 && result < search.Page)
            {
                ViewBag.Page = search.Page - 1;
                return PartialView("Search", paths.ToPagedList(search.Page - 1, search.PageSize));
            }
            else
                return PartialView("Search", paths.ToPagedList(search.Page, search.PageSize));

        }

        private IQueryable<RequestListDto> SearchRequests(RequestSearchModel search)
        {
            decimal percentage = 0;
            percentage = _UnitOfWork.SettingRepository.All().FirstOrDefault().ManagementPercentage;
            ViewBag.ManagementPercentage = percentage;

            if (!string.IsNullOrEmpty(search.UserId))
            {
                var user = _repo.FindUserByName(search.UserId);
                search.UserId = user!=null ? user.Id : null;
            }
            else          
                search.UserId = null;
          
            var paths = _UnitOfWork.RequestRepository.SearchRequests(search.UserId,
                                                                    search.AgentId,
                                                                    RequestTypeEnum.Request, 
                                                                    search.Status,
                                                                    search.CityId,
                                                                    search.Keyword
            );


            return paths;


            
        }



        public JsonResult GetUsers(string text)
        {

            if (string.IsNullOrEmpty(text))
                return Json(null, JsonRequestBehavior.AllowGet);

            var users = _repo.FindAllUsers(text,RolesEnum.User.ToString());           
            return Json(users, JsonRequestBehavior.AllowGet);
        }
        //return PartialView(data);


        //public ActionResult Add()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public ActionResult Add(RefuseResonViewModel model)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return View("Add", model);
        //        }

        //        var refuse = new RefuseReason()
        //        {

        //            Value = model.Value,

        //        };

        //        _UnitOfWork.RefuseReasonsRepository.Create(refuse);
        //        _UnitOfWork.Commit();
        //        var message = string.Format("تم اضافة سبب الرفض بنجاح .");
        //        return RedirectToAction("Index", "RefuseResons").Success(message);

        //    }
        //    catch (Exception e)
        //    {
        //        var message = string.Format("حدث خطأ أثناء الحفظ");
        //        return RedirectToAction("Add", "RefuseResons").Error(message);
        //    }

        //}

        //public ActionResult Edit(int id)
        //{

        //    var refuse = _UnitOfWork.RefuseReasonsRepository.Find(id);

        //    if (refuse == null)
        //         return HttpNotFound();

        //    var viewModel = new RefuseResonViewModel()
        //    {
        //        Value = refuse.Value


        //    };

        //    return View("Edit", viewModel);
        //}

        //[HttpPost]
        //public ActionResult Edit(RefuseResonViewModel model)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {     
        //            return View("Edit", model);

        //        }
        //        var refuse = _UnitOfWork.RefuseReasonsRepository.Find(model.Id);
        //        if (refuse == null)
        //            return HttpNotFound("refuse Not Exist!");

        //        refuse.Value = model.Value;
        //        _UnitOfWork.RefuseReasonsRepository.Update(refuse);
        //        _UnitOfWork.Commit();

        //        var message = string.Format("تم تعديل سبب الرفض  بنجاح .");
        //        return RedirectToAction("Index", "RefuseResons").Success(message);
        //    }
        //    catch (Exception e)
        //    {
        //        var message = string.Format("حدث خطأ أثناء الحفظ");
        //        return RedirectToAction("Edit", "RefuseResons").Error(message);
        //    }

        //}

        [System.Web.Mvc.AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(RequestSearchModel search)
        {
            try
            {

                var request = _UnitOfWork.RequestRepository.Find(search.Id);
                if (request == null)                    
                    return new HttpStatusCodeResult(404, "NotFound");


                _UnitOfWork.RequestRepository.Delete(request);
                _UnitOfWork.Commit();

                string actionName = "Search";
                if (search.Status == RequestStatusEnum.New)
                    actionName = "Search";
                else if (search.Status == RequestStatusEnum.Inprogress)
                    actionName = "SearchInprogress";
                else if (search.Status == RequestStatusEnum.Canceled)
                    actionName = "SearchCanceled";
                else if (search.Status == RequestStatusEnum.Delivered)
                    actionName = "SearchDelivered";


                return RedirectToAction(actionName, new
                {
                    UserId = search.UserId,
                    AgentId = search.AgentId,
                    Status = search.Status,
                    CityId = search.CityId,
                    Page = search.Page,
                    PageSize = search.PageSize,
                    Keyword = search.Keyword
                });
            
               
            }
            catch (Exception)
            {
                

                return new HttpStatusCodeResult(404, "NotFound");


            }

        }


        public ActionResult Hide(long id)
        {
            try
            {

                var request = _UnitOfWork.RequestRepository.Find(id);
                if (request == null)
                    return Json(new { message = "NotFound" });

                request.IsHidden = true;
                _UnitOfWork.Commit();
                return Json(new { message = "Hidden" });



            }
            catch (Exception)
            {


                return Json(new { message = "NotHidden" });
              


            }

        }
        public ActionResult UnHide(long id)
        {
            try
            {
                var request = _UnitOfWork.RequestRepository.Find(id);
                if (request == null)
                    return Json(new { message = "NotFound" });

                request.IsHidden = false;
                _UnitOfWork.Commit();
                return Json(new { message = "UnHidden" });
            }
            catch (Exception e)
            {


                return Json(new { message = "NotUnHidden" });



            }

        }

        public ActionResult Cancel(long id)
        {



            Request request = _UnitOfWork.RequestRepository.GetbyId(id);
            if (request == null)
                return HttpNotFound();
            ViewBag.Request = request;
            PopulateRefuseResons();
            return View();
        }


        private void PopulateRefuseResons()
        {
            //RefuseRequest
            var reasons = _UnitOfWork.RefuseReasonsRepository
                .All()
                .OrderBy(e => e.Id);

            ViewBag.RefuseReasons = reasons;

        }


        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> Cancel(RefuseRequestViewModel search)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    Request old = _UnitOfWork.RequestRepository.GetbyId(search.Id);
                    if (old == null)
                        return HttpNotFound();
                    ViewBag.Request = old;
                    PopulateRefuseResons();
                    return View("Cancel", search);
                }
                var request = _UnitOfWork.RequestRepository.Find(search.Id);
                if (request == null)
                    return new HttpStatusCodeResult(404, "NotFound");

                if (request.Status != RequestStatusEnum.Inprogress)
                    return RedirectToAction("InProgress", "Requests").Error("لا يمكنك الغاء هذا الطلب .");

                request.Status = RequestStatusEnum.Canceled;

                _UnitOfWork.RefuseRequestsRepository.Create(new RefuseRequest()
                {
                    RequestId = request.Id,
                    UserId = request.UserId,
                    AgentId = request.AgentId,
                    RefuseReasonId = search.RefuseReasonId,
                    Cause = search.Cause
                });
                _UnitOfWork.Commit();

                var value = _UnitOfWork.RefuseReasonsRepository.All().FirstOrDefault(u => u.Id == search.RefuseReasonId);
                if (value != null)
                    await SendNotification(request, NotificationType.RequestCancele.GetHashCode(), "", value.Value.ToString());
                else
                    await SendNotification(request, NotificationType.RequestCancele.GetHashCode(), "", "");


                var message = string.Format("تم الغاء الطلب بنجاح .");
                return RedirectToAction("InProgress", "Requests").Success(message);


            }
            catch (Exception e)
            {
                ErrorSaver.SaveError(e.Message);

                return new HttpStatusCodeResult(404, "NotFound");


            }

        }

        public ActionResult Details(long id)
        {           
            Request request = _UnitOfWork.RequestRepository.GetbyId(id);
            if (request == null)
                return HttpNotFound();
            return View(request);
        }






    }
}