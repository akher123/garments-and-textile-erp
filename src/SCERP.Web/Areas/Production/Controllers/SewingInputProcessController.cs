using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.Model.Production;
using SCERP.Web.Areas.Production.Models;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Production.Controllers
{
    public class SewingInputProcessController : BaseController
    {
        private readonly ISewingInputProcessManager _sewingInputProcessManager;
        private readonly IOmBuyerManager _buyerManager;
        private readonly ICutBankManager _cutBankManager;
        private readonly IMachineManager _machineManager;
        private readonly IHourManager _hourManager;


        public SewingInputProcessController(ISewingInputProcessManager sewingInputProcessManager, IOmBuyerManager buyerManager, ICutBankManager cutBankManager, IMachineManager machineManager, IHourManager hourManager)
        {

            _sewingInputProcessManager = sewingInputProcessManager;
            _buyerManager = buyerManager;
            _cutBankManager = cutBankManager;
            _machineManager = machineManager;
            _hourManager = hourManager;
        }
        [AjaxAuthorize(Roles = "sewinginput-1,sewinginput-2,sewinginput-3")]
        public ActionResult Index(SewingInputProcessViewModel model)
        {
            ModelState.Clear();
            model.Buyers = _buyerManager.GetCuttingProcessStyleActiveBuyers() as IEnumerable;
            model.SewingInputProcess.SewingInputProcessRefId = _sewingInputProcessManager.GetNewSewingInputProcessRefId();
            model.InputTime = DateTime.Now.ToString("hh:mm tt");
            return View(model);
        }
        [AjaxAuthorize(Roles = "sewinginput-2,sewinginput-3")]
        public ActionResult Find([Bind(Include = "SewingInputProcess")] SewingInputProcessViewModel model)
        {
            ModelState.Clear();
            model.Buyers = _buyerManager.GetCuttingProcessStyleActiveBuyers() as IEnumerable;
            model.HourList = _hourManager.GetAllHour();
            model.SewingInputDetails = _cutBankManager.GetAllCutBankByStyleColor(model.SewingInputProcess.OrderStyleRefId, model.SewingInputProcess.ColorRefId, model.SewingInputProcess.OrderShipRefId);
            if (model.SewingInputDetails.Count <= 0)
            {
                return ErrorResult("Data Not Found.");
            }
            List<string> sizeList = model.SewingInputDetails.Select(x => x.SizeName).ToList();
            sizeList.Add("TotalQty");
            List<string> sizeRefIdList = model.SewingInputDetails.Select(x => x.SizeRefId).ToList();
            List<string> orderQtyList = model.SewingInputDetails.Select(x => Convert.ToString(x.OrderQty)).ToList();
            orderQtyList.Add(Convert.ToString(model.SewingInputDetails.Sum(x => x.OrderQty)));
            List<string> bankList = model.SewingInputDetails.Select(x => Convert.ToString(x.BankQty)).ToList();
            bankList.Add(Convert.ToString(model.SewingInputDetails.Sum(x => x.BankQty)));
            List<string> totalInputQtylist = model.SewingInputDetails.Select(x => Convert.ToString(x.InputQuantity)).ToList();
            totalInputQtylist.Add(Convert.ToString(model.SewingInputDetails.Sum(x => x.InputQuantity)));
            List<string> inputPercentList = model.SewingInputDetails.Select(x =>x.OrderQty>0? String.Format("{0:0.00}" + " " + "%", (x.InputQuantity * 100.00m) / x.OrderQty):"0").ToList();
            inputPercentList.Add(String.Format("{0:0.00}" + " " + "%", model.SewingInputDetails.Sum(x => x.InputQuantity) * 100.0m / model.SewingInputDetails.Sum(x => x.OrderQty)));
            model.PivotDictionary.Add("Size", sizeList);
            model.PivotDictionary.Add("OrderQty", orderQtyList);
            model.PivotDictionary.Add("CuttBankQty", bankList);
            model.PivotDictionary.Add("TotalInputQty", totalInputQtylist);
            model.PivotDictionary.Add("Input %", inputPercentList);
            foreach (var sizeRefId in sizeRefIdList)
            {
                model.SewingInputProcessDetailDictionary.Add(sizeRefId, new PROD_SewingInputProcessDetail() { SizeRefId = sizeRefId, CompId = PortalContext.CurrentUser.CompId });
            }
            model.SewingInputProcess.InputDate = DateTime.Now;
            model.InputTime = DateTime.Now.ToString("hh:mm tt");
            model.Machines = _machineManager.GetLines();
            model.VwSewingInputProcesses = _sewingInputProcessManager.GetSewingInputProcessByStyleColor(model.SewingInputProcess.OrderStyleRefId, model.SewingInputProcess.ColorRefId,model.SewingInputProcess.OrderShipRefId);
            model.SewingInputProcess = new PROD_SewingInputProcess
            {
                SewingInputProcessRefId = _sewingInputProcessManager.GetNewSewingInputProcessRefId(),
                InputDate = DateTime.Now,

            };
            return PartialView("~/Areas/Production/Views/SewingInputProcess/_Plate.cshtml", model);
        }
        [AjaxAuthorize(Roles = "sewinginput-2,sewinginput-3")]
        [HttpPost]
        public ActionResult Save(SewingInputProcessViewModel model)
        {
            int index = 0;
            string message = "";
            try
            {
                model.SewingInputProcess.CompId = PortalContext.CurrentUser.CompId;
                model.SewingInputProcess.PreparedBy = PortalContext.CurrentUser.UserId.GetValueOrDefault();
               // bool isExist = _sewingInputProcessManager.IsSewingInputExist(model.SewingInputProcess);
                    if (model.SewingInputProcess.SewingInputProcessId > 0)
                    {
                        model.SewingInputProcess.InputDate =
                            model.SewingInputProcess.InputDate.GetValueOrDefault().ToMargeDateAndTime(model.InputTime);
                        model.SewingInputProcess.PROD_SewingInputProcessDetail =
                            model.SewingInputProcessDetailDictionary.Select(x => x.Value).ToList();
                        if (model.SewingInputProcess.PROD_SewingInputProcessDetail.Any())
                        {
                            if (model.SewingInputProcess.LineId > 0 && model.SewingInputProcess.HourId > 0)
                            {
                                index = _sewingInputProcessManager.EditSewingInputProcess(model.SewingInputProcess);
                                message = "Edit Successfully";
                            }
                            else
                            {
                                message = "Please Line and Houre Select Properly";
                                return ErrorResult(message);
                            }
                           
                       
                        }
                    }
                    else
                    {
                        model.SewingInputProcess.InputDate =
                            model.SewingInputProcess.InputDate.GetValueOrDefault().ToMargeDateAndTime(model.InputTime);
                        model.SewingInputProcess.PROD_SewingInputProcessDetail =
                            model.SewingInputProcessDetailDictionary.Where(x=>x.Value.InputQuantity>0).Select(x => x.Value).ToList();
                        var inputValue = model.SewingInputProcess.PROD_SewingInputProcessDetail.Aggregate(0,
                            (current, sewingInput) => current + sewingInput.InputQuantity);
                        if (inputValue > 0)
                        {
                          
                            if (model.SewingInputProcess.LineId > 0 && model.SewingInputProcess.HourId>0)
                            {
                                index = _sewingInputProcessManager.SaveSewingInputProcess(model.SewingInputProcess);
                                message = "Saved Successfully";
                            }
                            else
                            {
                                message = "Please Line and Houre Select Properly";
                                return ErrorResult(message);
                            }
                           
                        }
                        else
                        {
                            return ErrorResult("Enter Input Quantity.");
                        }
                    }
                if (index > 0)
                {
                    ModelState.Clear();
                    model.SewingInputDetails = _cutBankManager.GetAllCutBankByStyleColor(model.SewingInputProcess.OrderStyleRefId, model.SewingInputProcess.ColorRefId,model.SewingInputProcess.OrderShipRefId);
                   
                    if (model.SewingInputDetails.Count <= 0)
                    {
                        return ErrorResult("Data Not Found.");
                    }
                    List<string> sizeList = model.SewingInputDetails.Select(x => x.SizeName).ToList();
                    sizeList.Add("TotalQty");
                    List<string> sizeRefIdList = model.SewingInputDetails.Select(x => x.SizeRefId).ToList();
                    List<string> orderQtyList = model.SewingInputDetails.Select(x => Convert.ToString(x.OrderQty)).ToList();
                    orderQtyList.Add(Convert.ToString(model.SewingInputDetails.Sum(x => x.OrderQty)));
                    List<string> bankList = model.SewingInputDetails.Select(x => Convert.ToString(x.BankQty)).ToList();
                    bankList.Add(Convert.ToString(model.SewingInputDetails.Sum(x => x.BankQty)));
                    List<string> totalInputQtylist = model.SewingInputDetails.Select(x => Convert.ToString(x.InputQuantity)).ToList();
                    totalInputQtylist.Add(Convert.ToString(model.SewingInputDetails.Sum(x => x.InputQuantity)));
                    List<string> inputPercentList = model.SewingInputDetails.Select(x =>x.OrderQty>0? String.Format("{0:0.00}" + " " + "%", (x.InputQuantity * 100.00m) / x.OrderQty):"0").ToList();
                    inputPercentList.Add(String.Format("{0:0.00}" + " " + "%", model.SewingInputDetails.Sum(x => x.InputQuantity) * 100.0m / model.SewingInputDetails.Sum(x => x.OrderQty)));
                    model.PivotDictionary.Add("Size", sizeList);
                    model.PivotDictionary.Add("OrderQty", orderQtyList);
                    model.PivotDictionary.Add("CuttBankQty", bankList);
                    model.PivotDictionary.Add("TotalInputQty", totalInputQtylist);
                    model.PivotDictionary.Add("Input %", inputPercentList);
                    model.SewingInputProcessDetailDictionary = new Dictionary<string, PROD_SewingInputProcessDetail>();
                    foreach (var sizeRefId in sizeRefIdList)
                    {
                        model.SewingInputProcessDetailDictionary.Add(sizeRefId, new PROD_SewingInputProcessDetail() { SizeRefId = sizeRefId, CompId = PortalContext.CurrentUser.CompId });
                    }
                    model.InputTime = DateTime.Now.ToString("hh:mm tt");
                    model.HourList = _hourManager.GetAllHour();
                    model.Machines = _machineManager.GetLines();

                    model.VwSewingInputProcesses = _sewingInputProcessManager.GetSewingInputProcessByStyleColor(model.SewingInputProcess.OrderStyleRefId, model.SewingInputProcess.ColorRefId, model.SewingInputProcess.OrderShipRefId);
                    model.SewingInputProcess = new PROD_SewingInputProcess
                    {
                        SewingInputProcessRefId = _sewingInputProcessManager.GetNewSewingInputProcessRefId(),
                        InputDate = DateTime.Now,
                 
                    };

                    string partialViewString = RenderViewToString("~/Areas/Production/Views/SewingInputProcess/_Plate.cshtml", model);
                    return Json(
                            new
                            {
                                SaveStatus = true,
                                StatusMessage = message,
                                partialView = partialViewString,
                                refId = model.SewingInputProcess.SewingInputProcessRefId,
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
        [AjaxAuthorize(Roles = "sewinginput-2,sewinginput-3")]
        public ActionResult Edit([Bind(Include = "SewingInputProcess")] SewingInputProcessViewModel model)
        {
            ModelState.Clear();
            model.SewingInputProcess = _sewingInputProcessManager.GetSewintInputProcessBySewingInputProcessId(model.SewingInputProcess.SewingInputProcessId, PortalContext.CurrentUser.CompId);
            if (model.SewingInputProcess.Locked.GetValueOrDefault()==false)
            {
                model.Buyers = _buyerManager.GetCuttingProcessStyleActiveBuyers() as IEnumerable;
                model.VwSewingInputProcessDetails = _sewingInputProcessManager.GetAllSewingInputInfo(model.SewingInputProcess.SewingInputProcessId);
                List<string> sizeList = model.VwSewingInputProcessDetails.Select(x => x.SizeName).ToList();
                sizeList.Add("TotalQty");
                List<string> orderQtyList = model.VwSewingInputProcessDetails.Select(x => Convert.ToString(x.OrderQty)).ToList();
                orderQtyList.Add(Convert.ToString(model.VwSewingInputProcessDetails.Sum(x => x.OrderQty)));
                List<string> bankList = model.VwSewingInputProcessDetails.Select(x => Convert.ToString(x.BankQty)).ToList();
                bankList.Add(Convert.ToString(model.VwSewingInputProcessDetails.Sum(x => x.BankQty)));
                List<string> totalInputQtylist = model.VwSewingInputProcessDetails.Select(x => Convert.ToString(x.TotalInput)).ToList();
                totalInputQtylist.Add(Convert.ToString(model.VwSewingInputProcessDetails.Sum(x => x.TotalInput)));
                List<string> inputPercentList = model.VwSewingInputProcessDetails.Select(x => x.OrderQty > 0 ? String.Format("{0:0.00}" + " " + "%", (x.InputQuantity * 100.00m) / x.OrderQty) : "0").ToList();
                inputPercentList.Add(String.Format("{0:0.00}" + " " + "%", model.VwSewingInputProcessDetails.Sum(x => x.InputQuantity) * 100.0m / model.VwSewingInputProcessDetails.Sum(x => x.OrderQty)));
                model.PivotDictionary.Add("Size", sizeList);
                model.PivotDictionary.Add("OrderQty", orderQtyList);
                model.PivotDictionary.Add("CuttBankQty", bankList);
                model.PivotDictionary.Add("TotalInputQty", totalInputQtylist);
                model.PivotDictionary.Add("Input %", inputPercentList);
                string key = "1";
                foreach (var input in model.VwSewingInputProcessDetails)
                {
                    model.SewingInputProcessDetailDictionary.Add(key, new PROD_SewingInputProcessDetail() { InputQuantity = Convert.ToInt32(input.InputQuantity), SizeRefId = input.SizeRefId, CompId = PortalContext.CurrentUser.CompId });
                    key = key + '1';
                }
                model.SewingInputProcess.Remarks = model.SewingInputProcess.Remarks;
                model.InputTime = model.SewingInputProcess.InputDate.GetValueOrDefault().ToShortTimeString();

                model.Machines = _machineManager.GetLines();
                model.HourList = _hourManager.GetAllHour();
                return PartialView("~/Areas/Production/Views/SewingInputProcess/_SewingInput.cshtml", model);
            }
            else
            {
                return ErrorResult("This is locked by store because inpute wise accessoris item has issued!");
            }
        
      
        }
        [AjaxAuthorize(Roles = "sewinginput-3")]
        public ActionResult Delete([Bind(Include = "SewingInputProcess")] SewingInputProcessViewModel model)
        {
            int deleted = 0;
            deleted = _sewingInputProcessManager.DeleteSewingInputProcess(model.SewingInputProcess.SewingInputProcessId, PortalContext.CurrentUser.CompId);
            return deleted > 0 ? Reload() : ErrorResult("Failed To Delete Sewing Input Process.");
        }
        [AjaxAuthorize(Roles = "dailysewinginput-1,dailysewinginput-2,dailysewinginput-3")]
        public ActionResult DailySewingInput(SewingInputProcessViewModel model)
        {
            ModelState.Clear();
            model.Machines = _machineManager.GetLines();
            model.IsSearch = true;
            var totalRecords = 0;
            var totalInput = 0;
            model.SewingInputProcess.InputDate = model.SewingInputProcess.InputDate ?? DateTime.Now.Date;
            model.VwSewingInputProcesses = _sewingInputProcessManager.GetSewingInputByPaging(model.SewingInputProcess.InputDate, model.SewingInputProcess.LineId, model.PageIndex, out totalRecords, out totalInput);
            model.TotalRecords = totalRecords;
            model.TotalInput = totalInput;
            model.RowNo = model.PageIndex * 10;
            return View(model);
        }
        [AjaxAuthorize(Roles = "linesizewiseinput-1,linesizewiseinput-2,linesizewiseinput-3")]
        public ActionResult LineAndSizeInput(SewingInputProcessViewModel model)
        {
            model.Buyers = _buyerManager.GetCuttingProcessStyleActiveBuyers() as IEnumerable;
            return View(model);
        }
        [HttpGet]
        public ActionResult SweingInBarcode()
        {
            SewingInputProcessViewModel model=new SewingInputProcessViewModel();
            model.SewingInputProcess.InputDate = DateTime.Now;
            model.InputTime = DateTime.Now.ToString("hh:mm tt");
            model.Machines = _machineManager.GetLines();
            model.HourList = _hourManager.GetAllHour();
            model.VwSewingInputProcesses = _sewingInputProcessManager.DailySweingInPut(DateTime.Now.Date,  model.SewingInputProcess.LineId, PortalContext.CurrentUser.CompId);
            return View(model);
        }
        [HttpPost]
        public ActionResult SweingInBarcode(SewingInputProcessViewModel model)
        {

            PROD_SewingInputProcess bundelDetail = _sewingInputProcessManager.GetInputByBundleId(model.SewingInputProcess.JobNo);
            if (bundelDetail != null)
            {
                return ErrorResult("This bundle is user");
            }
            else
            {
                _sewingInputProcessManager.SaveSweingInBarcode(model.SewingInputProcess);
            }
            model.VwSewingInputProcesses = _sewingInputProcessManager.DailySweingInPut(DateTime.Now.Date, model.SewingInputProcess.LineId, PortalContext.CurrentUser.CompId);
            return PartialView("~/Areas/Production/Views/SewingInputProcess/_SweingInputList.cshtml", model);
        }
        [HttpGet]
        public ActionResult GetSweingInputList(int lineId)
        {
            SewingInputProcessViewModel model=new SewingInputProcessViewModel();
            model.VwSewingInputProcesses = _sewingInputProcessManager.DailySweingInPut(DateTime.Now.Date, lineId, PortalContext.CurrentUser.CompId);
            return PartialView("~/Areas/Production/Views/SewingInputProcess/_SweingInputList.cshtml", model);
        }
        [AjaxAuthorize(Roles = "accessoriesissue-3")]
        public ActionResult SweingAccessoriesIssueLock(long sewingInputProcessId)
        {
            int locked = _sewingInputProcessManager.SweingAccessoriesIssueLock(sewingInputProcessId);
            return ErrorResult(locked > 0 ? "Accessories Delivery Locked/Unlocked Successfully !" : "Accessories Delivery Locked Failed !");
        }
    }
}