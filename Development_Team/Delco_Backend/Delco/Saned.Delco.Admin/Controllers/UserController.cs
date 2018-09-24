using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity;
using PagedList;
using Saned.Delco.Admin.Error;
using Saned.Delco.Admin.Extensions;
using Saned.Delco.Admin.Models;
using Saned.Delco.Admin.Properties;
using Saned.Delco.Data.Core.Enum;
using Saned.Delco.Data.Core.Models;
using Saned.Delco.Data.Extentions;
using ApplicationUser = Saned.Delco.Data.Core.Models.ApplicationUser;
using Saned.Delco.Data.Persistence.Tools;
using Saned.Delco.Data.Persistence.Repositories;
using WebGrease.Css.Extensions;

namespace Saned.Delco.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UserController : BasicController
    {
        // GET: User


        //public ActionResult Add()
        //{
        //    return View();
        //}




        //[HttpPost]
        //public async Task<ActionResult> Add(UserViewModel viewModel)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            IdentityResult result =
        //                await _repo.RegisterUser(viewModel.FullName,
        //                    viewModel.PhoneNumber,
        //                    viewModel.Email,
        //                    viewModel.UserName,
        //                    viewModel.Password
        //                    );
        //            GetErrorResult(result);


        //        }

        //        var u = await _repo.FindUser(viewModel.UserName, viewModel.Password);
        //        return View(u);
        //    }
        //    catch (Exception ex)
        //    {
        //        string msg = ex.Message;
        //        return View();
        //    }

        //}

        [AcceptVerbs(HttpVerbs.Post)]
        [Route("ApproveAgent/{id}")]
        public async Task<ActionResult> ApproveAgent(string id)
        {
            try
            {
                var oldUser = await _repo.FindUserById(id);
                if (oldUser != null)
                {


                    var result=await _repo.UpdatePhoneNumberConfirmed(oldUser, true);
                    if (result.Succeeded)
                    {

                        var setting =
                    _UnitOfWork
                        .EmailSetting
                        .GetEmailSetting(EmailType.ActivateAgent.GetHashCode().ToString());


                        EmailManager.SendWelcomeEmail(EmailType.ActivateAgent.GetHashCode().ToString(),
                            oldUser.Email,
                            setting.MessageBodyAr.Replace("@FullName", oldUser.FullName));

                        return Json("OK", JsonRequestBehavior.AllowGet);

                    }
                    else
                    {
                        return Json("NotFound", JsonRequestBehavior.AllowGet);

                    }

                    //await _repo.GenerateToken(id, EmailType.EmailConfirmation);

                    //var setting =
                    //    _UnitOfWork.EmailSetting.GetEmailSetting(EmailType.EmailConfirmation.GetHashCode().ToString());
                    //EmailManager EmailManager = new EmailManager();

                    // string smsUrl = Settings.Default.SmsUrl.ToString();


                    ////var sent = await _repo.SendSmsConfirmation(smsUrl, oldUser);
                    ////if (!sent)
                    ////{
                    ////    return Json("MessageNotSent", JsonRequestBehavior.AllowGet);
                    ////}

                   

                    // var result = await _UnitOfWork.CommitAsync();

                    //return RedirectToAction("AgentsRequest").Success("تم قبول المندوب بنجاح.");


                }
                //return RedirectToAction("AgentsRequest").Error("هذا المندوب غير موجود .");

                return Json("NotFound", JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {

                ErrorSaver.SaveError(ex.Message);

                return Json("Error", JsonRequestBehavior.AllowGet);

                // return RedirectToAction("AgentsRequest").Error("حدث خطأ أثناء قبول المندوب.");

            }
        }


        [Route("ApproveUser/{id}")]
        public async Task<ActionResult> ApproveUser(string id)
        {
            try
            {
                var oldUser = await _repo.FindUserById(id);
                if (oldUser != null)
                {


                    var result = await _repo.UpdatePhoneNumberConfirmed(oldUser, true);
                    if (result.Succeeded)
                    {

                        var setting =
                    _UnitOfWork
                        .EmailSetting
                        .GetEmailSetting(EmailType.ActivateAgent.GetHashCode().ToString());


                        EmailManager.SendWelcomeEmail(EmailType.ActivateAgent.GetHashCode().ToString(),
                            oldUser.Email,
                            setting.MessageBodyAr.Replace("@FullName", oldUser.FullName));

                        return Json("OK", JsonRequestBehavior.AllowGet);

                    }
                    else
                    {
                        return Json("NotFound", JsonRequestBehavior.AllowGet);

                    }
                }
                //return RedirectToAction("AgentsRequest").Error("هذا المندوب غير موجود .");

                return Json("NotFound", JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {

                ErrorSaver.SaveError(ex.Message);

                return Json("Error", JsonRequestBehavior.AllowGet);

                // return RedirectToAction("AgentsRequest").Error("حدث خطأ أثناء قبول المندوب.");

            }
        }


        public async Task<ActionResult> User_Read([DataSourceRequest] DataSourceRequest request)
        {
            var role = await _repo.IsRoleByName(RolesEnum.User.ToString());
            var users = await _repo.FindUsers(role.Id);

            return Json(users.ToDataSourceResult(request));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> User_Create([DataSourceRequest] DataSourceRequest request, UserViewModel user)
        {
            if (user != null && ModelState.IsValid)
            {
                await
                    _repo.CreateUser(user.FullName, user.PhoneNumber, user.UserName, user.Email, user.Password, 0, "",
                        "", "");
                await _UnitOfWork.CommitAsync();
            }

            return Json(new[] { user }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> User_Update([DataSourceRequest] DataSourceRequest request, UserViewModel user)
        {
            if (user != null && ModelState.IsValid)
            {
                //_UnitOfWork.CityRepository.Update(city);
                await _UnitOfWork.CommitAsync();
            }

            return Json(new[] { user }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> User_Destroy([DataSourceRequest] DataSourceRequest request, UserViewModel user)
        {
            if (user != null)
            {
                try
                {
                    var oldUser = await _repo.FindUser(user.Id);
                    if (oldUser != null)
                    {
                        _repo.DeleteById(oldUser.Id);
                        await _UnitOfWork.CommitAsync();
                    }
                }
                catch (Exception ex)
                {

                }
            }

            return Json(new[] { user }.ToDataSourceResult(request, ModelState));
        }




        public ActionResult Users()
        {
            return View();
        }

        public ActionResult _Users(Pager model)
        {

            ViewBag.Keyword = model.Keyword;
            ViewBag.Page = model.Page;
            ViewBag.PageSize = model.PageSize;

            var role = Task.Run(() => _repo.IsRoleByName(RolesEnum.User.ToString())).Result;
            var users = Task.Run(() => _repo.FindUsers(role.Id)).Result;

            var usersResult = users.Where(x => string.IsNullOrEmpty(model.Keyword) ||
                      x.Email.Contains(model.Keyword) || x.PhoneNumber.Contains(model.Keyword) ||
                      x.FullName.Contains(model.Keyword) || x.UserName.Contains(model.Keyword)).ToList();
            return PartialView(usersResult.ToPagedList(model.Page, model.PageSize));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> Destroy(Pager search)
        {
            try
            {
                var user =  _repo.FindUserByIdNotAsync(search.UserId);
                var userRequest = _UnitOfWork.RequestRepository.Filter(x => x.UserId == user.Id);
                if (user != null)
                {
                    if (userRequest.Any())
                    {
                        var isInprogress = userRequest.Any(x => x.Status == RequestStatusEnum.Inprogress);
                        if (isInprogress)
                             SendDeleteNotification(userRequest.Where(x => x.Status == RequestStatusEnum.Inprogress).ToList(),
                                NotificationType.UserDelete.GetHashCode());
                        userRequest.ForEach(a => a.CancelVsSetUserToNull());
                    }

                    await _repo.DeleteById(user.Id);
                    _UnitOfWork.Commit();

                    return RedirectToAction("_Users", new { Page = search.Page, PageSize = search.PageSize });


                    //  return Json(0, JsonRequestBehavior.AllowGet);
                }

                return new HttpStatusCodeResult(404, "NotFound");

                // return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(404, "NotFound");

                return Json(2, JsonRequestBehavior.AllowGet);
            }
        }



        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> DestroyAgentbyId(string id)
        {
            try
            {
                var user = await _repo.FindUserById(id);
                var userRequest = _UnitOfWork.RequestRepository.Filter(x => x.UserId == user.Id);
                if (user != null)
                {
                    if (userRequest.Any())
                    {
                        var isInprogress = userRequest.Any(x => x.Status == RequestStatusEnum.Inprogress);
                        if (isInprogress)
                             SendDeleteNotification(userRequest.Where(x => x.Status == RequestStatusEnum.Inprogress).ToList(),
                                NotificationType.UserDelete.GetHashCode());
                        userRequest.ForEach(a => a.UserId = null);
                    }
                    _repo.DeleteById(user.Id);
                    _UnitOfWork.Commit();
                    return Json("OK", JsonRequestBehavior.AllowGet);


                }
                return Json("NotFound", JsonRequestBehavior.AllowGet);
                //  return RedirectToAction("AgentsRequest").Error("هذا المندوب غير موجود .");

            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);

                //return RedirectToAction("AgentsRequest").Error("حدث خطأ أثناء قبول المندوب.");            
            }
        }

        [System.Web.Mvc.AcceptVerbs(HttpVerbs.Post)]


        public async Task<ActionResult> DeleteAgent(Pager search)
        {
          
                try
                {
                var user =  _repo.FindUserByIdNotAsync(search.UserId);

                    var userRequest = _UnitOfWork.RequestRepository.Filter(x => x.AgentId == user.Id);
                    var userRefuseRequest =  _UnitOfWork.RefuseRequestsRepository.Filter(x => x.AgentId == user.Id);
                    var agentRatingUsers =  _UnitOfWork.RatingRepository.Filter(x => x.AgentId == user.Id);
                    if (user != null)
                    {
                        if (userRequest.Any())
                        {
                            var isInprogress = userRequest.Any(x => x.Status == RequestStatusEnum.Inprogress);
                            if (isInprogress)
                             SendDeleteNotification(
                                    userRequest.Where(x => x.Status == RequestStatusEnum.Inprogress).ToList(),
                                    NotificationType.UserDelete.GetHashCode());
                            userRequest.ForEach(a => a.AgentId = null);
                        }

                        if(userRefuseRequest.Any())
                           _UnitOfWork.RefuseRequestsRepository.RemoveRange(userRefuseRequest);


                    if (agentRatingUsers.Any())
                        _UnitOfWork.RatingRepository.RemoveRange(agentRatingUsers);


                       _UnitOfWork.Commit();
                    await _repo.DeleteById(user.Id);
                       
                        return 
                    RedirectToAction("_Agents", new {Page = search.Page, PageSize = search.PageSize})
                                .Success("تم الحذف بنجاح");


                    }

                   
                        return new HttpStatusCodeResult(404, "NotFound");              
            }
            catch (Exception e)
            {


                return new HttpStatusCodeResult(404, "Error");


            }

        }


        [System.Web.Mvc.AcceptVerbs(HttpVerbs.Post)]


        public  ActionResult HideAllDelivedRequests(string userId)
        {

            try
            {
                var user = _repo.FindUserByIdNotAsync(userId);
                var userRequest = _UnitOfWork.RequestRepository.Filter(x => x.AgentId == userId);
                if (user != null)
                {
                    if (userRequest.Any())
                    {
                        var isInprogress = userRequest.Any(x => x.Status == RequestStatusEnum.Delivered);                       
                        userRequest.ForEach(a => a.IsHidden = true);
                        _UnitOfWork.Commit();

                        return new HttpStatusCodeResult(200, "Ok");


                    }
                    else
                        return new HttpStatusCodeResult(404, "NoRequests");


                }
                else
                {
                    return new HttpStatusCodeResult(404, "NotFound");

                }


            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(404, "Error");




            }

        }


        public async Task<ActionResult> Update(string id)
        {
            var model = await _repo.FindUserById(id);
            var userModel = Mapper.Map<ApplicationUser, UserEditViewModel>(model);
            return View(userModel);
        }

        [HttpPost]
        public async Task<ActionResult> Update(UserEditViewModel viewModel)
        {
            try
            {

                if (ModelState.IsValid)
                {


                    if (_repo.CheckifEmailAvailable(viewModel.Email))
                    {
                        await _repo.UpdateUser(viewModel.Id, viewModel.FullName, viewModel.Email);
                        await _UnitOfWork.CommitAsync();
                        var message = "تم تعديل المستخدم بنجاح .";
                        return RedirectToAction("Users", "User").Success(message);
                    }
                    else
                    {
                        ModelState.AddModelError("", @"هذا البريد موجود من قبل");
                    }
                }

                return View(viewModel);
            }
            catch (Exception ex)
            {


                var message = "حدث خطأ أثناء الحفظ";
                return View().Success(message);
            }

        }


        public ActionResult Agents()
        {
            return View();
        }


        public ActionResult _Agents(Pager model)
       {
            ViewBag.Page = model.Page;
            ViewBag.Keyword = model.Keyword;
            ViewBag.PageSize = model.PageSize;
                var role = Task.Run(() => _repo.IsRoleByName(RolesEnum.Agent.ToString())).Result;
            var users = (Task.Run(() => _repo.FindUsers(role.Id)).Result).Where(x => x.PhoneNumberConfirmed);

            var usersResult = users.Where(x => string.IsNullOrEmpty(model.Keyword) ||
                    x.Email.Contains(model.Keyword) || x.PhoneNumber.Contains(model.Keyword) ||
                    x.FullName.Contains(model.Keyword) || x.UserName.Contains(model.Keyword)).ToList();
            return PartialView(usersResult.ToPagedList(model.Page, model.PageSize));

        }


     

        public async Task<ActionResult> AgentsRequest(Pager model)
        {


            var role = await _repo.IsRoleByName(RolesEnum.Agent.ToString());
            var users = await _repo.FindNewAgentsByRoleId(role.Id);
            foreach (var user in users)
            {
                await _repo.UpdateIsSeen(user, true);

            }
            return View(users.ToPagedList(model.Page, model.PageSize));
        }

        public async Task<ActionResult> AgentRequestDetails(string id)
        {
            try
            {
                PassengerNumber passengerNumber = new PassengerNumber();
                var model = await _repo.FindUserById(id);
                if (model.Car != null)
                {
                    passengerNumber =
                   _UnitOfWork.PassengerNumberRepository.Filter(x => x.Id == model.Car.PassengerNumberId)
                       .FirstOrDefault();
                }

                var userRate = _UnitOfWork.RatingRepository.Filter(x => x.AgentId == model.Id).ToList();
                var agentViewModel = new AgentViewModel()
                {
                    Id = model.Id,
                    UserName = model.UserName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    FullName = model.FullName,
                    RequestType = model.Type.GetHashCode(),
                    TypeRequest = model.Type.GetStringValue(),
                    CompanyName = model.Car?.CompanyName,
                    Type = model.Car?.Type,
                    Model = model.Car?.Model,
                    Color = model.Car?.Color,
                    CityId = model.CityId ?? model.CityId.Value,
                    Cityname = _UnitOfWork.CityRepository.Filter(x => x.Id == model.CityId).FirstOrDefault()?.Name,
                    PassengerNumber = passengerNumber?.Name + passengerNumber?.Value,
                    PlateNumber = model.Car?.PlateNumber,
                    PassengerNumberId = model.Car?.PassengerNumberId,
                    Rate = (userRate.Count > 0 ? userRate.Average(x => x.Degree).ToString() : "لا يوجد تقيمات بعد")
                };
                return View(agentViewModel);
            }
            catch (Exception ex)
            {

                return View();
            }


        }




        public async Task<ActionResult> UpdateAgent(string id)
        {
            try
            {
                SetViewBags();
                var model = await _repo.FindUserById(id);
                var agentViewModel = new AgentViewModel()
                {
                    Id = model.Id,
                    UserName = model.UserName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    FullName = model.FullName,
                    RequestType = model.Type.GetHashCode(),
                    CompanyName = model.Car.CompanyName,
                    Model = model.Car.Model,
                    Type = model.Car.Type,
                    Color = model.Car.Color,
                    CityId = model.CityId ?? model.CityId.Value,
                    PlateNumber = model.Car.PlateNumber,
                    PassengerNumberId = model.Car.PassengerNumberId
                };
                return View(agentViewModel);

            }
            catch (Exception ex)
            {

                return View();
            }

        }

        [HttpPost]
        public async Task<ActionResult> UpdateAgent(AgentViewModel viewModel)
        {
            try
            {
                SetViewBags();
                if (ModelState.IsValid)
                {
                    if (_repo.CheckifEmailAvailable(viewModel.Email))
                    {
                        await
                            _repo.UpdateAgent(viewModel.Id, viewModel.FullName, viewModel.Email, viewModel.Color,
                                viewModel.CompanyName, viewModel.Model,
                                viewModel.PlateNumber, viewModel.Type, viewModel.RequestType, viewModel.CityId,
                                viewModel.PassengerNumberId);
                        await _UnitOfWork.CommitAsync();
                        var message = "تم تعديل المستخدم بنجاح .";
                        return RedirectToAction("Agents", "User").Success(message);
                    }
                    else
                    {
                        ModelState.AddModelError("", @"هذا البريد موجود من قبل");
                    }
                }

                return View(viewModel);
            }
            catch (Exception ex)
            {
                var message = "حدث خطأ أثناء الحفظ";
                return View().Success(message);
            }

        }


        public async Task<ActionResult> UserDetails(string id)
        {

            var user = await _repo.FindUserById(id);
            return View(user);
        }

        public async Task<ActionResult> AgentDetails(string id)
        {
            try
            {
                PassengerNumber passengerNumber = new PassengerNumber();
                var model = await _repo.FindUserById(id);
                if (model.Car != null)
                {
                    passengerNumber =
                   _UnitOfWork.PassengerNumberRepository.Filter(x => x.Id == model.Car.PassengerNumberId)
                       .FirstOrDefault();
                }

                var userRate = _UnitOfWork.RatingRepository.Filter(x => x.AgentId == model.Id).ToList();


                var requests = _UnitOfWork.RequestRepository.All().Where(u => u.AgentId== model.Id &&  u.Status == RequestStatusEnum.Delivered && !u.IsHidden);
                var  requestSum = requests.Any() ? requests.Sum(u => u.Price) : 0;

                var setting = _UnitOfWork.SettingRepository.GetSetting();
                var appPercentage = Math.Round((requestSum * (setting.ManagementPercentage / 100)),2);
                var totalAfterApp = Math.Round((requestSum - appPercentage),2);
                //

                var agentViewModel = new AgentViewModel()
                {
                    Id = model.Id,
                    UserName = model.UserName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    FullName = model.FullName,
                    RequestType = model.Type.GetHashCode(),
                    TypeRequest = model.Type.GetStringValue(),
                    CompanyName = model.Car?.CompanyName,
                    Type = model.Car?.Type,
                    Model = model.Car?.Model,
                    Color = model.Car?.Color,
                    CityId = model.CityId ?? model.CityId.Value,
                    Cityname = _UnitOfWork.CityRepository.Filter(x => x.Id == model.CityId).FirstOrDefault()?.Name,
                    PassengerNumber = passengerNumber?.Name + passengerNumber?.Value,
                    PlateNumber = model.Car?.PlateNumber,
                    PassengerNumberId = model.Car?.PassengerNumberId,
                    Rate = (userRate.Count > 0 ? userRate.Average(x => x.Degree).ToString() : "لا يوجد تقيمات بعد"),
                    ExecutedRequests = (requests.Any() ? requests.Count().ToString() : "لا يوجد طلبات منفذة"),
                    Total = requestSum.ToString(),
                    AppPercentage = appPercentage.ToString(CultureInfo.InvariantCulture),
                    TotalAfterApp = totalAfterApp.ToString(),

                };
                return View(agentViewModel);
            }
            catch (Exception ex)
            {

                return View();
            }


        }

        [AcceptVerbs(HttpVerbs.Get)]
        public async Task<ActionResult> DestroyAgent(string id)
        {

            try
            {
                var user = await _repo.FindUserById(id);
                var userRequest = _UnitOfWork.RequestRepository.Filter(x => x.AgentId == user.Id);
                if (user != null)
                {
                    var isInprogress = userRequest.Any(x => x.Status == RequestStatusEnum.Inprogress);
                    if (isInprogress)
                         SendDeleteNotification(userRequest.Where(x => x.Status == RequestStatusEnum.Inprogress).ToList(), NotificationType.AgentDelete.GetHashCode());
                    UpdateUserRequest(userRequest.ToList());
                    _repo.DeleteById(user.Id);
                    _UnitOfWork.Commit();
                    return Json(0, JsonRequestBehavior.AllowGet);

                }
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(2, JsonRequestBehavior.AllowGet);
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public async Task<ActionResult> UnSuspend(string id)
        {

            try
            {
                var user = await _repo.FindUserById(id);

                if (user != null)
                {
                    user.IsSuspend = false;
                    await _repo.UpdateAgent(user);
                    _UnitOfWork.RefuseRequestsRepository.Delete(x => x.AgentId == id);
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


        private void SetViewBags()
        {

            var cities = _UnitOfWork.CityRepository.All().OrderBy(e => e.Name);
            var passengerNumbers = _UnitOfWork.PassengerNumberRepository.All().OrderBy(e => e.Name)
                .Select(p => new SelectListItem
                {
                    Text = p.Name + " " + p.Value,
                    Value = p.Id.ToString()
                });


            var requestType = new List<SelectListItem>()
            {
                new SelectListItem() {Text = RequestTypesEnum.Trip.ToString(), Value = "1"},
                new SelectListItem() {Text = RequestTypesEnum.Request.ToString(), Value = "2"},
                new SelectListItem() {Text = RequestTypesEnum.Both.ToString(), Value = "3"},
            };

            ViewBag.cities = cities;
            ViewBag.passengerNumbers = passengerNumbers;
            ViewBag.RequestType = requestType;


        }

        private void SendDeleteNotification(List<Request> requests, int type)
        {
            foreach (var req in requests)
            {
               SendNotificationNotasync(req, type, "", "");
            }
        }

        private void UpdateRequest(List<Request> requests)
        {
            foreach (var req in requests)
            {
                req.UserId = null;
                _UnitOfWork.RequestRepository.Update(req);
                _UnitOfWork.Commit();
            }
        }

        private void UpdateUserRequest(List<Request> requests)
        {
            foreach (var req in requests)
            {
                req.AgentId = null;
                req.Status = RequestStatusEnum.New;
                _UnitOfWork.RequestRepository.Update(req);
                _UnitOfWork.Commit();
            }
        }

        //[AllowAnonymous]
        //[HttpPost]
        //public ActionResult CheckExistingEmail(string Email ,string userId)
        //{
        //    try
        //    {
        //      var isEmailExists=  _repo.CheckifEmailAvailable(Email, userId);
        //        return Json(!isEmailExists);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(false);
        //    }
        //}




    }
}