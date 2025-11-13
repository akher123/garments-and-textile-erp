using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.BLL.IManager.ICommercialManager;
using SCERP.BLL.IManager.IPlanningManager;
using SCERP.Model.CommercialModel;
using SCERP.Web.Areas.Commercial.Models.ViewModel;
using SCERP.Web.Controllers;
using SCERP.Common;
using System.Collections;

namespace SCERP.Web.Areas.Commercial.Controllers
{
    public class ReceiveController : BaseController
    {
        private readonly int _pageSize = AppConfig.PageSize;
        private readonly IReceiveManager _receiveManager;

        public ReceiveController(IReceiveManager receiveManager)
        {
            this._receiveManager = receiveManager;
        }

        public ActionResult Index(ReceiveViewModel model)
        {
            try
            {
                ModelState.Clear();

                CommReceive receive = model;
                receive.FromDate = model.FromDate;
                receive.ToDate = model.ToDate;

                int startPage = 0;
                if (model.page.HasValue && model.page.Value > 0)
                {
                    startPage = model.page.Value - 1;
                }

                int totalRecords = 0;
                model.Receives = _receiveManager.GetAllReceivesByPaging(startPage, _pageSize, out totalRecords, receive) ?? new List<CommReceive>();
                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            ModelState.Clear();
            ReceiveViewModel model = new ReceiveViewModel();

            try
            {
                if (model.ReceiveId > 0)
                {
                    CommReceive lc = _receiveManager.GetReceiveById(id);
                    model.ReceiveId = lc.ReceiveId;
                    model.ReceiveRefNo = lc.ReceiveRefNo;
                    model.BbLcId = lc.BbLcId;
                    model.PassBookPageNo = lc.PassBookPageNo;
                    model.ReceiveDate = lc.ReceiveDate;
                    model.MushakChallanDate = lc.MushakChallanDate;
                    model.MushakChallanNo = lc.MushakChallanNo;

                    ViewBag.Title = "Edit LC";
                }
                else
                {
                    ViewBag.Title = "Add LC";
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }

        public ActionResult Save(ReceiveViewModel model)
        {
            int saveIndex = 0;

            try
            {
                CommReceive receive = _receiveManager.GetReceiveById(model.ReceiveId) ?? new CommReceive();

                receive.ReceiveRefNo = model.ReceiveRefNo;
                receive.BbLcId = model.BbLcId;
                receive.ReceiveDate = model.ReceiveDate;
                receive.PassBookPageNo = model.PassBookPageNo;
                receive.MushakChallanNo = model.MushakChallanNo;
                receive.MushakChallanDate = model.MushakChallanDate;
                receive.IsActive = true;

                saveIndex = (model.ReceiveId > 0) ? _receiveManager.EditReceive(receive) : _receiveManager.SaveReceive(receive);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (saveIndex > 0) ? Reload() : ErrorResult("Failed to save data");
        }

        public ActionResult Delete(int id)
        {
            int deleted = 0;
            try
            {
                CommReceive receive = _receiveManager.GetReceiveById(id) ?? new CommReceive();
                deleted = _receiveManager.DeleteReceive(receive);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return (deleted > 0) ? Reload() : ErrorResult("Failed to delete data");
        }

        public ActionResult AddNewRow(ReceiveViewModel model)
        {
            var receiveId = 100;

            var receiveDetail = new CommReceiveDetail
            {
                ItemCode = model.ReceiveDetail.ItemCode,
                Quantity = model.ReceiveDetail.Quantity,
                Rate = model.ReceiveDetail.Rate,

            };
            model.ReceiveDetails.Add(receiveId.ToString(), receiveDetail);

            //else
            //{
            //    model.Key = model.ReceiveDetail.ReceiveId.ToString();      
            //    model.ReceiveDetails.Add(model.Key, model.ReceiveDetail);
            //}

            if (model.ReceiveDetail != null && model.ReceiveDetail.ReceiveId < 0)
            {
                return ErrorResult("Please Insert valid receive information !");
            }
            else
            {
                return PartialView("~/Areas/Commercial/Views/Receive/_AddNewRow.cshtml", model);
            }
        }
    }
}