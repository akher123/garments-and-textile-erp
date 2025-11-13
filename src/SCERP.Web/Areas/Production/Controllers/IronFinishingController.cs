using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.Model.Production;
using SCERP.Web.Areas.Production.Models;
using SCERP.Web.Controllers;
using SCERP.Web.Helpers;
using SCERP.Web.Models;

namespace SCERP.Web.Areas.Production.Controllers
{
    public class IronFinishingController : BaseController
    {
        private readonly IFinishingProcessManager _finishingProcessManager;
        private readonly IOmBuyerManager _buyerManager;
        private readonly IHourManager _hourManager;
        public IronFinishingController(IFinishingProcessManager finishingProcessManager, IOmBuyerManager buyerManager, IHourManager hourManager)
        {
            _finishingProcessManager = finishingProcessManager;
            _buyerManager = buyerManager;
            _hourManager = hourManager;
        }

        [AjaxAuthorize(Roles = "ironfinishing-1,ironfinishing-2,ironfinishing-3")]
        public ActionResult Index(FinishingProcessViewModel model)
        {
            ModelState.Clear();
            const string prifix = "IR";
            model.Buyers = _buyerManager.GetCuttingProcessStyleActiveBuyers() as IEnumerable;
            model.FinishingProcess.FinishingProcessRefId = _finishingProcessManager.GetNewFinishingProcessRefId((int)FinishType.Iron, prifix);
            return View(model);
        }
        [AjaxAuthorize(Roles = "ironfinishing-2,ironfinishing-3")]
        public ActionResult Find([Bind(Include = "FinishingProcess")] FinishingProcessViewModel model)
        {
            ModelState.Clear();
            const string prifix = "IR";
            model.Buyers = _buyerManager.GetCuttingProcessStyleActiveBuyers() as IEnumerable;
            model.HourList = _hourManager.GetAllHour();
            if (model.FinishingProcess.FinishingProcessId > 0)
            {
                model.FinishingProcess = _finishingProcessManager.GeFabricProcessById(model.FinishingProcess.FinishingProcessId);
                model.VwFinishingProcessDetails = _finishingProcessManager.GetFinishingProcessDetailByStyleColor(model.FinishingProcess.OrderStyleRefId, model.FinishingProcess.ColorRefId, model.FinishingProcess.FinishingProcessId);
                model.VwFinishingProcess = _finishingProcessManager.GetSewingInputProcessByStyleColor(model.FinishingProcess.OrderStyleRefId, model.FinishingProcess.ColorRefId, (int)FinishType.Iron);
            }
            else
            {
                model.VwFinishingProcessDetails = _finishingProcessManager.GetFinishingProcessDetailByStyleColor(model.FinishingProcess.OrderStyleRefId, model.FinishingProcess.ColorRefId, model.FinishingProcess.FinishingProcessId);
                model.VwFinishingProcess = _finishingProcessManager.GetSewingInputProcessByStyleColor(model.FinishingProcess.OrderStyleRefId, model.FinishingProcess.ColorRefId, (int)FinishType.Iron);
                model.FinishingProcess = new PROD_FinishingProcess
                {
                    FinishingProcessRefId = _finishingProcessManager.GetNewFinishingProcessRefId((int)FinishType.Iron, prifix),
                    InputDate = DateTime.Now

                };
            }
          
            if (model.VwFinishingProcessDetails.Count <= 0)
            {
                return ErrorResult("Data Not Found.");
            }
            List<string> sizeList = model.VwFinishingProcessDetails.Select(x => x.SizeName).ToList();
            sizeList.Add("TotalQty");
            List<string> ttlOrderQtyList = model.VwFinishingProcessDetails.Select(x => Convert.ToString(x.TtlOrderQty)).ToList();
            ttlOrderQtyList.Add(Convert.ToString(model.VwFinishingProcessDetails.Sum(x => x.TtlOrderQty)));
            List<string> swInputQtyList = model.VwFinishingProcessDetails.Select(x => Convert.ToString(x.TtlSwInputQty)).ToList();
            swInputQtyList.Add(Convert.ToString(model.VwFinishingProcessDetails.Sum(x => x.TtlSwInputQty)));
           

            List<string> swoutputQtyList = model.VwFinishingProcessDetails.Select(x => Convert.ToString(x.TtlSwOutQty)).ToList();
            swoutputQtyList.Add(Convert.ToString(model.VwFinishingProcessDetails.Sum(x => x.TtlSwOutQty)));
           
            List<string> ttlIronQtyList= model.VwFinishingProcessDetails.Select(x => Convert.ToString(x.TinQuantity)).ToList();
            ttlIronQtyList.Add(Convert.ToString(model.VwFinishingProcessDetails.Sum(x => x.TinQuantity)));

            List<string> inputPercentList = model.VwFinishingProcessDetails.Select(x => x.TtlOrderQty > 0 ? String.Format("{0:0.00}" + " " + "%", (x.TinQuantity * 100.00m) / x.TtlOrderQty) : "0").ToList();
            inputPercentList.Add(String.Format("{0:0.00}" + " " + "%", model.VwFinishingProcessDetails.Sum(x => x.TinQuantity) * 100.0m / model.VwFinishingProcessDetails.Sum(x => x.TtlOrderQty)));
            model.PivotDictionary.Add("Size", sizeList);
            model.PivotDictionary.Add("OrderQty", ttlOrderQtyList);
            model.PivotDictionary.Add("SewingInQty", swInputQtyList);
            model.PivotDictionary.Add("SewingOutQty", swoutputQtyList);
            model.PivotDictionary.Add("TotalIronty", ttlIronQtyList);
            model.PivotDictionary.Add("Iron %", inputPercentList);
            model.FinishingProcessDetailDictionary=new Dictionary<string, PROD_FinishingProcessDetail>();
            foreach (var finishProces in model.VwFinishingProcessDetails)
            {
           
                model.FinishingProcessDetailDictionary.Add(finishProces.SizeRefId,
                    model.FinishingProcess.FinishingProcessId > 0
                        ? new PROD_FinishingProcessDetail()
                        {
                            SizeRefId = finishProces.SizeRefId,
                            InputQuantity = finishProces.InputQuantity,
                            CompId = PortalContext.CurrentUser.CompId
                        }
                        : new PROD_FinishingProcessDetail()
                        {
                            SizeRefId = finishProces.SizeRefId,
                            CompId = PortalContext.CurrentUser.CompId
                        });
            }
          
            return PartialView("~/Areas/Production/Views/IronFinishing/_Plate.cshtml", model);
        }
        [AjaxAuthorize(Roles = "ironfinishing-2,ironfinishing-3")]
        public ActionResult Save(FinishingProcessViewModel model)
        {
            int index = 0;
            string message = "";
            const string prifix = "IR";
            try
            {
                model.FinishingProcess.CompId = PortalContext.CurrentUser.CompId;
                model.FinishingProcess.PreparedBy = PortalContext.CurrentUser.UserId.GetValueOrDefault();
                if (model.FinishingProcess.FinishingProcessId > 0)
                {
                   
                    model.FinishingProcess.PROD_FinishingProcessDetail =model.FinishingProcessDetailDictionary.Select(x => x.Value).Where(x=>x.InputQuantity>0).ToList();
                    if (model.FinishingProcess.PROD_FinishingProcessDetail.Any())
                    {
                        index = _finishingProcessManager.EditFinishingProcess(model.FinishingProcess);
                        message = "Edit Successfully";
                    }
                }
                else
                {
                    model.FinishingProcess.PROD_FinishingProcessDetail = model.FinishingProcessDetailDictionary.Select(x => x.Value).Where(x=>x.InputQuantity>0).ToList();
                    var inputValue = model.FinishingProcess.PROD_FinishingProcessDetail.Aggregate(0, (current, finishProcess) => current + finishProcess.InputQuantity);
                    if (inputValue > 0)
                    {
                        model.FinishingProcess.FType =(int) FinishType.Iron;
                        model.FinishingProcess.FinishingProcessRefId =
                            _finishingProcessManager.GetNewFinishingProcessRefId((int) FinishType.Iron, prifix);
                        index = _finishingProcessManager.SaveFinishingProcess(model.FinishingProcess);
                        message = "Saved Successfully";
                    }
                    else
                    {
                        return ErrorResult("Enter Input Quantity.");
                    }
                }
                if (index > 0)
                {
                    ModelState.Clear();

                    model.Buyers = _buyerManager.GetCuttingProcessStyleActiveBuyers() as IEnumerable;
                    model.HourList = _hourManager.GetAllHour();
                    model.VwFinishingProcessDetails = _finishingProcessManager.GetFinishingProcessDetailByStyleColor(model.FinishingProcess.OrderStyleRefId, model.FinishingProcess.ColorRefId, model.FinishingProcess.FinishingProcessId);
                    if (model.VwFinishingProcessDetails.Count <= 0)
                    {
                        return ErrorResult("Data Not Found.");
                    }
                    List<string> sizeList = model.VwFinishingProcessDetails.Select(x => x.SizeName).ToList();
                    sizeList.Add("TotalQty");
                    List<string> sizeRefIdList = model.VwFinishingProcessDetails.Select(x => x.SizeRefId).ToList();
                   
                    List<string> ttlOrderQtyList = model.VwFinishingProcessDetails.Select(x => Convert.ToString(x.TtlOrderQty)).ToList();
                    ttlOrderQtyList.Add(Convert.ToString(model.VwFinishingProcessDetails.Sum(x => x.TtlOrderQty)));
                    List<string> swInputQtyList = model.VwFinishingProcessDetails.Select(x => Convert.ToString(x.TtlSwInputQty)).ToList();
                    swInputQtyList.Add(Convert.ToString(model.VwFinishingProcessDetails.Sum(x => x.TtlSwInputQty)));


                    List<string> swoutputQtyList = model.VwFinishingProcessDetails.Select(x => Convert.ToString(x.TtlSwOutQty)).ToList();
                    swoutputQtyList.Add(Convert.ToString(model.VwFinishingProcessDetails.Sum(x => x.TtlSwOutQty)));

                    List<string> ttlIronQtyList = model.VwFinishingProcessDetails.Select(x => Convert.ToString(x.TinQuantity)).ToList();
                    ttlIronQtyList.Add(Convert.ToString(model.VwFinishingProcessDetails.Sum(x => x.TinQuantity)));

                    List<string> inputPercentList = model.VwFinishingProcessDetails.Select(x => x.TinQuantity > 0 ? String.Format("{0:0.00}" + " " + "%", (x.InputQuantity * 100.00m) / x.TinQuantity) : "0").ToList();
                    inputPercentList.Add(String.Format("{0:0.00}" + " " + "%", model.VwFinishingProcessDetails.Sum(x => x.InputQuantity) * 100.0m / model.VwFinishingProcessDetails.Sum(x => x.TtlSwOutQty)));
                    model.PivotDictionary.Add("Size", sizeList);
                    model.PivotDictionary.Add("OrderQty", ttlOrderQtyList);
                    model.PivotDictionary.Add("SewingInQty", swInputQtyList);
                    model.PivotDictionary.Add("SewingOutQty", swoutputQtyList);
                    model.PivotDictionary.Add("TotalIronty", ttlIronQtyList);
                    model.PivotDictionary.Add("Iron %", inputPercentList);
                    model.FinishingProcessDetailDictionary=new Dictionary<string, PROD_FinishingProcessDetail>();
                    foreach (var sizeRefId in sizeRefIdList)
                    {
                        model.FinishingProcessDetailDictionary.Add(sizeRefId, new PROD_FinishingProcessDetail() { SizeRefId = sizeRefId, CompId = PortalContext.CurrentUser.CompId });
                    }
                    model.FinishingProcess.InputDate = DateTime.Now;
                    model.VwFinishingProcess = _finishingProcessManager.GetSewingInputProcessByStyleColor(model.FinishingProcess.OrderStyleRefId, model.FinishingProcess.ColorRefId,(int)FinishType.Iron);
                     model.FinishingProcess = new PROD_FinishingProcess
                        {
                            FinishingProcessRefId = _finishingProcessManager.GetNewFinishingProcessRefId((int)FinishType.Iron, prifix),
                            InputDate = DateTime.Now,

                        }; 
                    
                    string partialViewString = RenderViewToString("~/Areas/Production/Views/IronFinishing/_Plate.cshtml", model);
                    return Json(
                            new
                            {
                                SaveStatus = true,
                                StatusMessage = message,
                                partialView = partialViewString,
                                refId = model.FinishingProcess.FinishingProcessRefId,
                            }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return ErrorResult("Fail to Save/Edit :");
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail to Save/Edit :");
            }

        }
        [AjaxAuthorize(Roles = "ironfinishing-3")]
        public ActionResult Delete(long finishingProcessId)
        {
            int deleted = 0;
            deleted = _finishingProcessManager.DeleteFinishingProcess(finishingProcessId);
            return deleted > 0 ? Reload() : ErrorResult("Failed To Delete Iron Process.");
        }

        [AjaxAuthorize(Roles = "dailyironfinishing-1,dailyironfinishing-2,dailyironfinishing-3")]
        public ActionResult DailyIronFinishing(FinishingProcessViewModel model)
        {
            ModelState.Clear();
            model.FinishingProcess.InputDate = model.FinishingProcess.InputDate ?? DateTime.Now;
            model.VwFinishingProcess = _finishingProcessManager.GetFinishingProcess(model.FinishingProcess.InputDate,
                (int)FinishType.Iron);
            return View(model);
        }

        public ActionResult DailyIronFinishingReport(FinishingProcessViewModel model)
        {
            ModelState.Clear();
            model.FinishingProcess.InputDate = model.FinishingProcess.InputDate ?? DateTime.Now;
            model.VwFinishingProcess = _finishingProcessManager.GetFinishingProcess(model.FinishingProcess.InputDate, (int)FinishType.Iron);

            List<ReportParameter> parameters = new List<ReportParameter>()
            {
                new ReportParameter("TitleName", "Daily Iron Finishing Report")
            };
            string path = Path.Combine(Server.MapPath("~/Areas/Production/Report"), "HourlyFinishingReport.rdlc");
            if (!System.IO.File.Exists(path))
            {
                return PartialView("~/Views/Shared/Error.cshtml");
            }
            var reportDataSources = new List<ReportDataSource>() { new ReportDataSource("FinishingDSet",  model.VwFinishingProcess) };
            var deviceInformation = new DeviceInformation() { OutputFormat = 2, PageWidth = 11.69, PageHeight = 8.27, MarginTop = .2, MarginLeft = 0.2, MarginRight = 0.2, MarginBottom = .2 };
            return ReportExtension.ToFile(ReportType.PDF, path, reportDataSources, deviceInformation, parameters);
        }
           
        
    }
}