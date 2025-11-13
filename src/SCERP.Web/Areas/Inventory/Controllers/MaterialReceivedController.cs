using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.BLL.IManager.IInventoryManager;
using SCERP.Common;
using SCERP.Model.InventoryModel;
using SCERP.Model.Maintenance;
using SCERP.Web.Areas.Inventory.Models.ViewModels;
using SCERP.Web.Controllers;


namespace SCERP.Web.Areas.Inventory.Controllers
{
    public class MaterialReceivedController : BaseController
    {
        private readonly IMaterialReceivedManager _materialReceivedManager;
        private readonly IMateraialReceivedDetailManager _materaialReceivedDetailManager;

        public MaterialReceivedController(IMaterialReceivedManager materialReceivedManager, IMateraialReceivedDetailManager materaialReceivedDetailManager)
        {
            _materialReceivedManager = materialReceivedManager;
            _materaialReceivedDetailManager = materaialReceivedDetailManager;
        }
        [AjaxAuthorize(Roles = "generalregister-1,generalregister-2,generalregister-3")]
        public ActionResult Index(MaterialReceivedViewModel model)
        {
            try
            {
                ModelState.Clear();
                var totalRecords = 0;
                model.MaterialReceivedList = _materialReceivedManager.GetMaterialReceivedByPaging(model.FromDate, model.ToDate, model.SearchString, Convert.ToString(RegisterType.GENERALREGISTER), PortalContext.CurrentUser.CompId, model.PageIndex, model.sort, model.sortdir, out totalRecords);
                model.TotalRecords = totalRecords;
                model.FromDate = DateTime.Now;
                model.ToDate = DateTime.Now;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail To Retrive Data :" + exception);
            }
            return View(model);
        }
        [AjaxAuthorize(Roles = "generalregister-2,generalregister-3")]
        public ActionResult Edit(MaterialReceivedViewModel model)
        {
            try
            {
                ModelState.Clear();
                if (model.MaterialReceived.MaterialReceivedId > 0)
                {
                    model.MaterialReceived = _materialReceivedManager.GetMaterialReceivedId(model.MaterialReceived.MaterialReceivedId, PortalContext.CurrentUser.CompId);
                    List<Inventory_MaterialReceivedDetail> materialReceivedDetailList = _materaialReceivedDetailManager.GetMaterialReceivedDetailByMaterialReceivedId(model.MaterialReceived.MaterialReceivedId, PortalContext.CurrentUser.CompId);
                    model.MaterialReceivedDetailDictionary = materialReceivedDetailList.ToDictionary(x => Convert.ToString(x.MaterialReceivedDetailId), x => x);
                }
                else
                {
                    model.MaterialReceived.MaterialReceivedRefId = _materialReceivedManager.GetMaterialReceivedRefId();
                    model.MaterialReceived.ReceivedDate = DateTime.Now;
                    model.MaterialReceived.ChallanDate = DateTime.Now;
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                ErrorResult("Fail To Retrive Data" + exception);
            }
            return View(model);
        }
        public ActionResult AddNewRow([Bind(Include = "MaterialReceivedDetail")]MaterialReceivedViewModel model)
        {
            model.Key = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            model.MaterialReceivedDetailDictionary.Add(model.Key, model.MaterialReceivedDetail);
            return PartialView("~/Areas/Inventory/Views/MaterialReceived/_AddNewRow.cshtml", model);
        }
          [AjaxAuthorize(Roles = "generalregister-2,generalregister-3")]
        public ActionResult Save(MaterialReceivedViewModel model)
        {
            var index = 0;
            try
            {
                if (_materialReceivedManager.IsMaterialReceivedExist(model.MaterialReceived))
                {
                    return ErrorResult(" This Information Already Exist ! Please Entry Another One");
                }
                else
                {
                    model.MaterialReceived.RegisterType =Convert.ToString(RegisterType.GENERALREGISTER);
                    model.MaterialReceived.Inventory_MaterialReceivedDetail = model.MaterialReceivedDetailDictionary.Select(x => x.Value).Select(x => new Inventory_MaterialReceivedDetail()
                    {
                        CompId = PortalContext.CurrentUser.CompId,
                        Item = x.Item,
                        Color = x.Color,
                        Size = x.Size,
                        Brand = x.Brand,
                        LotNo = x.LotNo,
                        UnitName = x.UnitName,
                        ReceivedQty = x.ReceivedQty,
                        Rate = x.Rate,
                        TotalAmount = ((x.ReceivedQty) * (x.Rate)),
                        BuyerNameDtl = x.BuyerNameDtl,
                        OrderNameDtl = x.OrderNameDtl,
                        StyleNameDtl = x.StyleNameDtl

                    }).ToList();
                    if (model.MaterialReceived.Inventory_MaterialReceivedDetail.Count > 0)
                    {
                        if (model.MaterialReceived.MaterialReceivedId > 0)
                        {
                            index = _materialReceivedManager.EditMaterialReceived(model.MaterialReceived);
                        }
                        else
                        {
                            model.MaterialReceived.BuyerName = "---";
                            model.MaterialReceived.OrderNo = "---";
                            model.MaterialReceived.StyleNo = "---";
                            index = _materialReceivedManager.SaveMaterialReceived(model.MaterialReceived);
                        }

                    }
                }

            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Failed to Save/Edit Material Received :" + exception);
            }
            return index > 0 ? Reload() : ErrorResult("Fail to Save/Edit Material Received !");
        }
        [AjaxAuthorize(Roles = "generalregister-3")]
        public ActionResult Delete(long materialReceivedId)
        {
            var index = 0;
            try
            {
                string compId = PortalContext.CurrentUser.CompId;
                index = _materialReceivedManager.DeleteMaterialReceived(materialReceivedId, compId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail to Delete Material Received :" + exception);
            }
            return index > 0 ? Reload() : ErrorResult("Fail to Delete Material Received !");
        }

        public ActionResult MaterialReceivedReport(MaterialReceivedViewModel model)
        {
            ModelState.Clear();
            if (!model.IsSearch)
            {
                model.IsSearch = true;
            }
            model.FromDate = DateTime.Now;
            model.ToDate = DateTime.Now.AddDays(2);
            return View(model);
        }
    }
}