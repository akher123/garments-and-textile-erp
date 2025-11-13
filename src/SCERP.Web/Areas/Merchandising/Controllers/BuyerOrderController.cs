using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Reporting.WebForms;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Areas.Merchandising.Models.ViewModel;
using SCERP.Web.Controllers;
using SCERP.Web.Helpers;

namespace SCERP.Web.Areas.Merchandising.Controllers
{
    public class BuyerOrderController : BaseController
    {
        private readonly IBuyerOrderManager _buyerOrderManager;
        private readonly IOmBuyerManager _omBuyerManager;
        private readonly IAgentManager _agentManager;
        private readonly IConsigneeManager _consigneeManager;
        private readonly IMerchandiserManager _merchandiserManager;
        private readonly IOrderTypeManager _orderTypeManager;
        private readonly ISeasonManager _seasonManager;
        private readonly IPaymentTermManager _paymentTermManager;
        private readonly IBuyOrdShipManager _buyOrdShipManager;
        public BuyerOrderController(IBuyOrdShipManager buyOrdShipManager, IBuyerOrderManager buyerOrderManager, IOmBuyerManager omBuyerManager,
            IAgentManager agentManager, IConsigneeManager consigneeManager,
            IMerchandiserManager merchandiserManager, IOrderTypeManager orderTypeManager, ISeasonManager seasonManager,
            IPaymentTermManager paymentTermManager)
        {
            _buyOrdShipManager = buyOrdShipManager;
            this._buyerOrderManager = buyerOrderManager;
            this._omBuyerManager = omBuyerManager;
            this._agentManager = agentManager;
            this._consigneeManager = consigneeManager;
            this._merchandiserManager = merchandiserManager;
            this._orderTypeManager = orderTypeManager;
            this._seasonManager = seasonManager;
            this._paymentTermManager = paymentTermManager;
        }

        [AjaxAuthorize(Roles = "buyerorder-1,buyerorder-2,buyerorder-3")]
        public ActionResult Index(BuyerOrderViewModel model)
        {
            ModelState.Clear();
            var totalRecords = 0;
            model.Closed = model.Closed ?? "O";
            model.BuyerOrders = _buyerOrderManager.GetBuyerOrderPaging(model.Closed, model.PageIndex, model.sort, model.sortdir, model.SearchString, model.FromDate, model.ToDate, out totalRecords);
            model.TotalRecords = totalRecords;
          
            return View("~/Areas/Merchandising/Views/BuyerOrder/Index.cshtml", model);
        }

        [AjaxAuthorize(Roles = "buyerorder-2,buyerorder-3")]
        public ActionResult Edit(BuyerOrderViewModel model)
        {
            ModelState.Clear();
            model.OmBuyers = _omBuyerManager.GetAllBuyers();
            model.OmSheAgents = _agentManager.GetShepAgents();
            model.OmAgents = _agentManager.GetAgents();
            model.OmConsignees = _consigneeManager.GetConsignees();
            model.OmMerchandisers = _merchandiserManager.GetPermitedMerchandisers();
            model.OmOrderTypes = _orderTypeManager.GetOrdertypes();
            model.OmSeasons = _seasonManager.GetSeasons();
            model.OmPaymentTerms = _paymentTermManager.GetPaymentTerms();
            model.OrderDate = DateTime.Now;
            if (model.BuyerOrderId > 0)
            {
                var buyerOrder = _buyerOrderManager.GetBuyerOrderById(model.BuyerOrderId);
                model.RefNo = buyerOrder.RefNo;
                model.OrderNo = buyerOrder.OrderNo;
                model.OrderDate = buyerOrder.OrderDate;
                model.SampleOrdNo = buyerOrder.SampleOrdNo;
                model.BuyerRefId = buyerOrder.BuyerRefId;
                model.DGRefNo = buyerOrder.DGRefNo;
                model.BuyerRef = buyerOrder.BuyerRef;
                model.Quantity = buyerOrder.Quantity;
                model.OAmount = buyerOrder.OAmount;
                model.MerchandiserId = buyerOrder.MerchandiserId;
                model.AgentRefId = buyerOrder.AgentRefId;
                model.ShipagentRefId = buyerOrder.ShipagentRefId;
                model.ConsigneeRefId = buyerOrder.ConsigneeRefId;
                model.PayTermRefId = buyerOrder.PayTermRefId;
                model.OrderTypeRefId = buyerOrder.OrderTypeRefId;
                model.SMode = buyerOrder.SMode;
                model.Fab = buyerOrder.Fab;
                model.SCont = buyerOrder.SCont;
                model.SeasonRefId = buyerOrder.SeasonRefId;
                model.Remarks = buyerOrder.Remarks;
                model.OrderStatus = buyerOrder.OrderStatus;

            }
            else
            {
                model.OrderNo = _buyerOrderManager.GetNewRefNo();
            }
            return View(model);
        }

        public ActionResult ReportPrint(BuyerOrderViewModel model)
        {
            model.BuyerOrders = _buyerOrderManager.GetVBuyerOrder(model);
            var fromDate = new ReportParameter("FromDate", model.FromDate.ToString());
            var toDate = new ReportParameter("ToDate", model.ToDate.ToString());
            var reportDataSource = new ReportDataSourceModel()
            {
                DataSource = model.BuyerOrders,
                Path = "~/Areas/Merchandising/Report/RDLC/BuyerOrderList.rdlc",
                DataSetName = "BuyerOrderDS",
                ReportParameters = new[] { fromDate, toDate }
            };
            return PartialView("~/Views/Shared/ReportViwerAPX.aspx", reportDataSource);
        }

        public ActionResult OrderlistReport(BuyerOrderViewModel model)
        {
            return View(model);
        }

        [AjaxAuthorize(Roles = "buyerorder-1,buyerorder-2,buyerorder-3")]
        public ActionResult BuyerOrderDetail(BuyerOrderViewModel model)
        {
            return View(model);
        }
        [AjaxAuthorize(Roles = "buyerorder-2,buyerorder-3")]
        public ActionResult Save(OM_BuyerOrder model)
        {
            var index = 0;
            var errorMessage = "";
            try
            {

                index = model.BuyerOrderId > 0
                    ? _buyerOrderManager.EditBuyerOrder(model)
                    : _buyerOrderManager.SaveBuyerOrder(model);
            }
            catch (Exception exception)
            {
                errorMessage = exception.Message;
                Errorlog.WriteLog(exception);
            }
            return index > 0 ? Reload() : ErrorResult("Failed to save Batch ! " + errorMessage);

        }

        [AjaxAuthorize(Roles = "buyerorder-3")]
        public JsonResult Delete(OM_BuyerOrder model)
        {
            var deleteIndex = 0;
            try
            {
                deleteIndex = _buyerOrderManager.DeleteBuyerOrder(model.OrderNo);
                if (deleteIndex == -1)
                {
                    return ErrorResult("This Order already In Progress !");
                }
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }
            return deleteIndex > 0 ? Reload() : ErrorResult("Fail To delte Item store");
        }

        [AjaxAuthorize(Roles = "buyerorder-1,buyerorder-2,buyerorder-3")]
        public ActionResult ShowOrderSheet(BuyerOrderViewModel model)
        {
            try
            {
                model.OrderSheet = _buyerOrderManager.GetOrderSheet(model.OrderNo);
                return PartialView("~/Areas/Merchandising/Views/BuyerOrder/_OrderSheetReport.cshtml", model);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("System Error :" + exception.Message);
            }

        }

        public JsonResult GetOrderByBuyer(string buyerRefId)
        {
            IEnumerable orderByBuyer = _buyerOrderManager.GetOrderByBuyer(buyerRefId);
            return Json(orderByBuyer, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DetailOrderSheet(BuyerOrderViewModel model)
        {
            try
            {
                //step 1
                MemoryStream msReport = new MemoryStream();
                model.OrderSheet = _buyOrdShipManager.GetOrderSheetDetail(model.OrderNo);

                using (Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 100f, 30f))
                {

                    // step 2
                    PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, msReport);
                    pdfWriter.CloseStream = false;
                    var iTextEvents = new ITextEvents();
                    iTextEvents.Header = "SHIPMENT WISE ORDER SHEET ";
                    pdfWriter.PageEvent = iTextEvents;

                    //open the stream 
                    pdfDoc.Open();
                    Font fontTinyItalic = FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                    PdfPTable table1 = new PdfPTable(7);
                    table1.HorizontalAlignment = 1;

                    table1.TotalWidth = 550f;
                    table1.LockedWidth = true;
                    float[] widths = new float[] { 65f, 2f, 80f, 72f, 65f, 3f, 72 };
                    table1.SetWidths(widths);
                    table1.AddCell(new PdfPCell(new Phrase("RefNo", fontTinyItalic)) { VerticalAlignment = Element.ALIGN_LEFT, HorizontalAlignment = Element.ALIGN_MIDDLE });
                    table1.AddCell(":");
                    table1.AddCell(new PdfPCell(new Phrase(model.OrderSheet.VBuyerOrder.OrderNo, fontTinyItalic)) { VerticalAlignment = Element.ALIGN_LEFT, HorizontalAlignment = Element.ALIGN_MIDDLE });
                    table1.AddCell("");
                    table1.AddCell(new PdfPCell(new Phrase("Buyer", fontTinyItalic)) { VerticalAlignment = Element.ALIGN_LEFT, HorizontalAlignment = Element.ALIGN_MIDDLE });
                    table1.AddCell(":");
                    table1.AddCell(new PdfPCell(new Phrase(model.OrderSheet.VBuyerOrder.BuyerName, fontTinyItalic)) { VerticalAlignment = Element.ALIGN_LEFT, HorizontalAlignment = Element.ALIGN_MIDDLE });

                    table1.AddCell(new PdfPCell(new Phrase("Order No", fontTinyItalic)) { VerticalAlignment = Element.ALIGN_LEFT, HorizontalAlignment = Element.ALIGN_MIDDLE });
                    table1.AddCell(":");
                    table1.AddCell(new PdfPCell(new Phrase(model.OrderSheet.VBuyerOrder.RefNo, fontTinyItalic)) { VerticalAlignment = Element.ALIGN_LEFT, HorizontalAlignment = Element.ALIGN_MIDDLE });
                    table1.AddCell(new PdfPCell() { Border = 0 });

                    table1.AddCell(new PdfPCell(new Phrase("Order Type", fontTinyItalic)) { VerticalAlignment = Element.ALIGN_LEFT, HorizontalAlignment = Element.ALIGN_MIDDLE });
                    table1.AddCell(":");
                    table1.AddCell(new PdfPCell(new Phrase(model.OrderSheet.VBuyerOrder.OTypeName, fontTinyItalic)) { VerticalAlignment = Element.ALIGN_LEFT, HorizontalAlignment = Element.ALIGN_MIDDLE });

                    table1.AddCell(new PdfPCell(new Phrase("Order Date", fontTinyItalic)) { VerticalAlignment = Element.ALIGN_LEFT, HorizontalAlignment = Element.ALIGN_MIDDLE });
                    table1.AddCell(":");
                    table1.AddCell(new PdfPCell(new Phrase(String.Format("{0:d/M/yyyy}", model.OrderSheet.VBuyerOrder.OrderDate), fontTinyItalic)) { VerticalAlignment = Element.ALIGN_LEFT, HorizontalAlignment = Element.ALIGN_MIDDLE });
                    table1.AddCell(new PdfPCell() { Border = 0 });

                    table1.AddCell(new PdfPCell(new Phrase("Payment Term", fontTinyItalic)) { VerticalAlignment = Element.ALIGN_LEFT, HorizontalAlignment = Element.ALIGN_MIDDLE });
                    table1.AddCell(":");
                    table1.AddCell(new PdfPCell(new Phrase(model.OrderSheet.VBuyerOrder.PayTerm, fontTinyItalic)) { VerticalAlignment = Element.ALIGN_LEFT, HorizontalAlignment = Element.ALIGN_MIDDLE });

                    table1.AddCell(new PdfPCell(new Phrase("Order Qty", fontTinyItalic)) { VerticalAlignment = Element.ALIGN_LEFT, HorizontalAlignment = Element.ALIGN_MIDDLE });
                    table1.AddCell(":");
                    table1.AddCell(new PdfPCell(new Phrase(model.OrderSheet.VBuyerOrder.Quantity.ToString(), fontTinyItalic)) { VerticalAlignment = Element.ALIGN_LEFT, HorizontalAlignment = Element.ALIGN_MIDDLE });
                    table1.AddCell(new PdfPCell() { Border = 0 });


                    table1.AddCell(new PdfPCell(new Phrase("Max Ship Date", fontTinyItalic)) { VerticalAlignment = Element.ALIGN_LEFT, HorizontalAlignment = Element.ALIGN_MIDDLE });
                    table1.AddCell(":");
                    table1.AddCell(new PdfPCell(new Phrase(String.Format("{0:d/M/yyyy}", model.OrderSheet.VBuyerOrder.ShipDate), fontTinyItalic)) { VerticalAlignment = Element.ALIGN_LEFT, HorizontalAlignment = Element.ALIGN_MIDDLE });

                    table1.AddCell(new PdfPCell(new Phrase("No Of Style", fontTinyItalic)) { VerticalAlignment = Element.ALIGN_LEFT, HorizontalAlignment = Element.ALIGN_MIDDLE });

                    table1.AddCell(":");

                    table1.AddCell(new PdfPCell(new Phrase(model.OrderSheet.VBuyerOrder.TotalStyleInOrder.ToString(), fontTinyItalic)) { VerticalAlignment = Element.ALIGN_LEFT, HorizontalAlignment = Element.ALIGN_MIDDLE });
                    table1.AddCell(new PdfPCell() { Border = 0 });

                    table1.AddCell(new PdfPCell(new Phrase("Merchandiser ", fontTinyItalic)) { VerticalAlignment = Element.ALIGN_LEFT, HorizontalAlignment = Element.ALIGN_MIDDLE });
                    table1.AddCell(":");
                    table1.AddCell(new PdfPCell(new Phrase(model.OrderSheet.VBuyerOrder.EmpName, fontTinyItalic)) { VerticalAlignment = Element.ALIGN_LEFT, HorizontalAlignment = Element.ALIGN_MIDDLE });

                    table1.AddCell(new PdfPCell(new Phrase("Fabrication", fontTinyItalic)) { VerticalAlignment = Element.ALIGN_LEFT, HorizontalAlignment = Element.ALIGN_MIDDLE });
                    table1.AddCell(":");
                    table1.AddCell(new PdfPCell(new Phrase(model.OrderSheet.VBuyerOrder.Fab, fontTinyItalic)) { VerticalAlignment = Element.ALIGN_LEFT, HorizontalAlignment = Element.ALIGN_MIDDLE, Colspan = 2 });

                    table1.AddCell(new PdfPCell(new Phrase("Ship Mode", fontTinyItalic)) { VerticalAlignment = Element.ALIGN_LEFT, HorizontalAlignment = Element.ALIGN_MIDDLE });
                    table1.AddCell(":");
                    table1.AddCell(new PdfPCell(new Phrase(model.OrderSheet.VBuyerOrder.SMode, fontTinyItalic)) { VerticalAlignment = Element.ALIGN_LEFT, HorizontalAlignment = Element.ALIGN_MIDDLE });

                    table1.AddCell("");
                    pdfDoc.Add(table1);


                    foreach (var styleShip in model.OrderSheet.OrderStyles)
                    {


                        PdfPTable table2 = new PdfPTable(4);
                        table2.SpacingBefore = 5;
                        table1.HorizontalAlignment = 1;
                        table2.TotalWidth = 500f;
                        table2.LockedWidth = true;
                        float[] table2widths = new float[] { 28, 2, 50 ,20};
                        table2.SetWidths(table2widths);



                        table2.AddCell(new PdfPCell() { Phrase = new Phrase("Season", fontTinyItalic), BackgroundColor = BaseColor.PINK });
                        table2.AddCell(":");
                        table2.AddCell(new PdfPCell() { Phrase = new Phrase(styleShip.Value.BuyOrdStyle.SeasonName, fontTinyItalic) });
                        styleShip.Value.BuyOrdStyle.ImagePath = styleShip.Value.BuyOrdStyle.ImagePath ?? "/UploadDocument/bank_image.jpg";
                        var mapPath1 = Server.MapPath(styleShip.Value.BuyOrdStyle.ImagePath);
                        if (System.IO.File.Exists(mapPath1))
                        {
                            Image img = Image.GetInstance(mapPath1);
                            img.ScaleToFit(80, 64);
                            PdfPCell c2 = new PdfPCell(img);
                            c2.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                            c2.Rowspan = 4;
                            c2.Border = Rectangle.NO_BORDER;
                            table2.AddCell(c2);
                        }

                        table2.AddCell(new PdfPCell() { Phrase = new Phrase("Style", fontTinyItalic), BackgroundColor = BaseColor.PINK });
                        table2.AddCell(":");
                        table2.AddCell(new PdfPCell() { Phrase = new Phrase(styleShip.Value.BuyOrdStyle.StyleName, fontTinyItalic) });
                       

                      
                        table2.AddCell(new PdfPCell() { Phrase = new Phrase("Item", fontTinyItalic), BackgroundColor = BaseColor.PINK });
                        table2.AddCell(":");
                        table2.AddCell(new PdfPCell() { Phrase = new Phrase(styleShip.Value.BuyOrdStyle.ItemName, fontTinyItalic) });
                     

                        table2.AddCell(new PdfPCell() { Phrase = new Phrase("Quantity", fontTinyItalic), BackgroundColor = BaseColor.PINK });
                        table2.AddCell(":");
                        table2.AddCell(new PdfPCell() { Phrase = new Phrase(styleShip.Value.BuyOrdStyle.Quantity.ToString(), fontTinyItalic) });
                  

                        pdfDoc.Add(table2);

                        foreach (var ship in styleShip.Value.Shipments)
                        {

                            PdfPTable tableShip = new PdfPTable(3);
                            tableShip.SpacingBefore = 5;
                            tableShip.HorizontalAlignment = 1;
                            tableShip.TotalWidth = 255f;
                            tableShip.LockedWidth = true;
                            float[] tableShipwidths = new float[] { 100f, 3f, 150f };
                            tableShip.SetWidths(tableShipwidths);

                            tableShip.AddCell(new PdfPCell() { Phrase = new Phrase("Port Of Loading", fontTinyItalic), BackgroundColor = BaseColor.GRAY });
                            tableShip.AddCell(":");
                            tableShip.AddCell(new PdfPCell() { Phrase = new Phrase(ship.OrdShip.PortOfLoadingName, fontTinyItalic) });

                            tableShip.AddCell(new PdfPCell() { Phrase = new Phrase("Country", fontTinyItalic), BackgroundColor = BaseColor.GRAY });
                            tableShip.AddCell(":");
                            tableShip.AddCell(new PdfPCell() { Phrase = new Phrase(ship.OrdShip.CountryName, fontTinyItalic) });

                            tableShip.AddCell(new PdfPCell() { Phrase = new Phrase("Paking", fontTinyItalic), BackgroundColor = BaseColor.GRAY });
                            tableShip.AddCell(":");
                            tableShip.AddCell(new PdfPCell() { Phrase = new Phrase(ship.OrdShip.ModeName, fontTinyItalic) });


                            tableShip.AddCell(new PdfPCell() { Phrase = new Phrase("Shipment Date", fontTinyItalic), BackgroundColor = BaseColor.GRAY });
                            tableShip.AddCell(":");
                            tableShip.AddCell(new PdfPCell(new Phrase(String.Format("{0:d/M/yyyy}", ship.OrdShip.ShipDate), fontTinyItalic)) { VerticalAlignment = Element.ALIGN_LEFT, HorizontalAlignment = Element.ALIGN_MIDDLE });

                            tableShip.AddCell(new PdfPCell() { Phrase = new Phrase("Qty", fontTinyItalic), BackgroundColor = BaseColor.GRAY });
                            tableShip.AddCell(":");
                            tableShip.AddCell(new PdfPCell() { Phrase = new Phrase(ship.OrdShip.Quantity.ToString(), fontTinyItalic) });

                            pdfDoc.Add(tableShip);


                            PdfPTable table3 = new PdfPTable(ship.ShipTable.Columns.Count);

                            table3.WidthPercentage = 100;
                            foreach (DataColumn col in ship.ShipTable.Columns)
                            {

                                table3.HorizontalAlignment = 1;
                                table3.AddCell(new PdfPCell() { Phrase = new Phrase(col.ColumnName, fontTinyItalic), BackgroundColor = BaseColor.YELLOW });
                                pdfDoc.Add(table3);

                            }

                            foreach (DataRow row in ship.ShipTable.Rows)
                            {

                                PdfPTable table4 = new PdfPTable(ship.ShipTable.Columns.Count);
                                table4.WidthPercentage = 100;
                                table3.HorizontalAlignment = 1;
                                foreach (DataColumn col in ship.ShipTable.Columns)
                                {
                                    table1.HorizontalAlignment = 1;
                                    table4.AddCell(new PdfPCell() { Phrase = new Phrase(row[col.ColumnName].ToString(), fontTinyItalic), VerticalAlignment = Element.ALIGN_RIGHT, HorizontalAlignment = Element.ALIGN_MIDDLE });
                                    pdfDoc.Add(table4);
                                }

                            }
                        }
                    }
                    pdfDoc.Close();
                }
                byte[] byteInfo = msReport.ToArray();
                msReport.Write(byteInfo, 0, byteInfo.Length);
                msReport.Position = 0;
                return new FileStreamResult(msReport, "application/pdf");
            }
            catch (Exception exception)
            {
                ViewBag.ErrorMessage = exception.Message;
                return PartialView("Error");
            }


        }

        public ActionResult ApprovedOrder(BuyerOrderViewModel model)
        {
            ModelState.Clear();
            var totalRecords = 0;
            const string closed = "O";
            model.BuyerOrders = _buyerOrderManager.GetBuyerOrderPaging(closed, model.PageIndex, model.sort, model.sortdir, model.SearchString, model.FromDate, model.ToDate, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }
        [AjaxAuthorize(Roles = "buyerorder-2,buyerorder-3")]
        public JsonResult UpdateOrderStatus(long buyerOrderId, string closed)
        {
            int updated = _buyerOrderManager.UpdateOrderStatus(buyerOrderId, closed);
            return Json(updated > 0, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ShowPdfOrderSheet(BuyerOrderViewModel model)
        {
            try
            {
                //step 1
                MemoryStream msReport = new MemoryStream();
                model.OrderSheet = _buyerOrderManager.GetOrderSheet(model.OrderNo);
                using (Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 100f, 30f))
                {

                    // step 2
                    PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, msReport);
                    pdfWriter.CloseStream = false;
                    var iTextEvents = new ITextEvents();
                    iTextEvents.Header = "Buyer Order Sheet";
                    pdfWriter.PageEvent = iTextEvents;

                    //open the stream 
                    pdfDoc.Open();
                    Font fontTinyItalic = FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                    PdfPTable table1 = new PdfPTable(7);
                    table1.HorizontalAlignment = 1;

                    table1.TotalWidth = 550f;
                    table1.LockedWidth = true;
                    float[] widths = new float[] { 65f, 5f, 80f, 72f, 65f, 5f, 72 };
                    table1.SetWidths(widths);
                    table1.AddCell(new PdfPCell(new Phrase("Reference No", fontTinyItalic)) { VerticalAlignment = Element.ALIGN_LEFT, HorizontalAlignment = Element.ALIGN_MIDDLE });
                    table1.AddCell(":");
                    table1.AddCell(new PdfPCell(new Phrase(model.OrderSheet.VBuyerOrder.OrderNo, fontTinyItalic)) { VerticalAlignment = Element.ALIGN_LEFT, HorizontalAlignment = Element.ALIGN_MIDDLE });
                    table1.AddCell("");


                    table1.AddCell(new PdfPCell(new Phrase("Buyer", fontTinyItalic)) { VerticalAlignment = Element.ALIGN_LEFT, HorizontalAlignment = Element.ALIGN_MIDDLE });
                    table1.AddCell(":");
                    table1.AddCell(new PdfPCell(new Phrase(model.OrderSheet.VBuyerOrder.BuyerName, fontTinyItalic)) { VerticalAlignment = Element.ALIGN_LEFT, HorizontalAlignment = Element.ALIGN_MIDDLE });

                    table1.AddCell(new PdfPCell(new Phrase("Order No", fontTinyItalic)) { VerticalAlignment = Element.ALIGN_LEFT, HorizontalAlignment = Element.ALIGN_MIDDLE });
                    table1.AddCell(":");
                    table1.AddCell(new PdfPCell(new Phrase(model.OrderSheet.VBuyerOrder.RefNo, fontTinyItalic)) { VerticalAlignment = Element.ALIGN_LEFT, HorizontalAlignment = Element.ALIGN_MIDDLE });
                    table1.AddCell(new PdfPCell() { Border = 0 });

                    table1.AddCell(new PdfPCell(new Phrase("Order Type", fontTinyItalic)) { VerticalAlignment = Element.ALIGN_LEFT, HorizontalAlignment = Element.ALIGN_MIDDLE });
                    table1.AddCell(":");
                    table1.AddCell(new PdfPCell(new Phrase(model.OrderSheet.VBuyerOrder.OTypeName, fontTinyItalic)) { VerticalAlignment = Element.ALIGN_LEFT, HorizontalAlignment = Element.ALIGN_MIDDLE });

                    table1.AddCell(new PdfPCell(new Phrase("Order Date", fontTinyItalic)) { VerticalAlignment = Element.ALIGN_LEFT, HorizontalAlignment = Element.ALIGN_MIDDLE });
                    table1.AddCell(":");
                    table1.AddCell(new PdfPCell(new Phrase(String.Format("{0:d/M/yyyy}", model.OrderSheet.VBuyerOrder.OrderDate), fontTinyItalic)) { VerticalAlignment = Element.ALIGN_LEFT, HorizontalAlignment = Element.ALIGN_MIDDLE });
                    table1.AddCell(new PdfPCell() { Border = 0 });

                    table1.AddCell(new PdfPCell(new Phrase("Payment Term", fontTinyItalic)) { VerticalAlignment = Element.ALIGN_LEFT, HorizontalAlignment = Element.ALIGN_MIDDLE });
                    table1.AddCell(":");
                    table1.AddCell(new PdfPCell(new Phrase(model.OrderSheet.VBuyerOrder.PayTerm, fontTinyItalic)) { VerticalAlignment = Element.ALIGN_LEFT, HorizontalAlignment = Element.ALIGN_MIDDLE });

                    table1.AddCell(new PdfPCell(new Phrase("Buyer Order No", fontTinyItalic)) { VerticalAlignment = Element.ALIGN_LEFT, HorizontalAlignment = Element.ALIGN_MIDDLE });
                    table1.AddCell(":");
                    table1.AddCell(new PdfPCell(new Phrase(model.OrderSheet.VBuyerOrder.BuyerRef, fontTinyItalic)) { VerticalAlignment = Element.ALIGN_LEFT, HorizontalAlignment = Element.ALIGN_MIDDLE });
                    table1.AddCell(new PdfPCell() { Border = 0 });

                    table1.AddCell(new PdfPCell(new Phrase("Season", fontTinyItalic)) { VerticalAlignment = Element.ALIGN_LEFT, HorizontalAlignment = Element.ALIGN_MIDDLE });
                    table1.AddCell(":");
                    table1.AddCell(new PdfPCell(new Phrase(String.Format("{0}", model.OrderSheet.VBuyerOrder.SeasonName), fontTinyItalic)) { VerticalAlignment = Element.ALIGN_LEFT, HorizontalAlignment = Element.ALIGN_MIDDLE });

                    table1.AddCell(new PdfPCell(new Phrase("Order Qty", fontTinyItalic)) { VerticalAlignment = Element.ALIGN_LEFT, HorizontalAlignment = Element.ALIGN_MIDDLE });
                    table1.AddCell(":");
                    table1.AddCell(new PdfPCell(new Phrase(model.OrderSheet.VBuyerOrder.Quantity.ToString(), fontTinyItalic)) { VerticalAlignment = Element.ALIGN_LEFT, HorizontalAlignment = Element.ALIGN_MIDDLE });
                    table1.AddCell(new PdfPCell() { Border = 0 });

                    table1.AddCell(new PdfPCell(new Phrase("Merchandiser ", fontTinyItalic)) { VerticalAlignment = Element.ALIGN_LEFT, HorizontalAlignment = Element.ALIGN_MIDDLE });
                    table1.AddCell(":");
                    table1.AddCell(new PdfPCell(new Phrase(model.OrderSheet.VBuyerOrder.EmpName, fontTinyItalic)) { VerticalAlignment = Element.ALIGN_LEFT, HorizontalAlignment = Element.ALIGN_MIDDLE });

                    table1.AddCell(new PdfPCell(new Phrase("Fabrication", fontTinyItalic)) { VerticalAlignment = Element.ALIGN_LEFT, HorizontalAlignment = Element.ALIGN_MIDDLE });
                    table1.AddCell(":");
                    table1.AddCell(new PdfPCell(new Phrase(model.OrderSheet.VBuyerOrder.Fab, fontTinyItalic)) { VerticalAlignment = Element.ALIGN_LEFT, HorizontalAlignment = Element.ALIGN_MIDDLE, Colspan = 2 });
                    table1.AddCell("");
                    table1.AddCell("");
                    table1.AddCell("");
                    table1.AddCell("");
                    pdfDoc.Add(table1);


                    foreach (var styleShip in model.OrderSheet.OrderStyles)
                    {
                       
                        PdfPTable table2 = new PdfPTable(4);
                        table2.SpacingBefore = 5;
                        table1.HorizontalAlignment = 1;
                        table2.TotalWidth = 400f;
                        table2.LockedWidth = true;
                        float[] table2widths = new float[] { 25, 5, 50,20 };
                        table2.SetWidths(table2widths);

                        table2.AddCell(new PdfPCell() { Phrase = new Phrase("Style", fontTinyItalic), BackgroundColor = BaseColor.GRAY });
                        table2.AddCell(":");
                        table2.AddCell(new PdfPCell() { Phrase = new Phrase(styleShip.Value.BuyOrdStyle.StyleName, fontTinyItalic) });
                        styleShip.Value.BuyOrdStyle.ImagePath = styleShip.Value.BuyOrdStyle.ImagePath ?? "/UploadDocument/bank_image.jpg";
                        var mapPath1 = Server.MapPath(styleShip.Value.BuyOrdStyle.ImagePath);
                        if (System.IO.File.Exists(mapPath1))
                        {
                            Image img = Image.GetInstance(mapPath1);
                            img.ScaleToFit(80, 64);
                            PdfPCell c2 = new PdfPCell(img);
                            c2.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                            c2.Rowspan = 3;
                            table2.AddCell(c2);
                        }


                        table2.AddCell(new PdfPCell() { Phrase = new Phrase("Item", fontTinyItalic), BackgroundColor = BaseColor.GRAY });

                        table2.AddCell(":");
                        table2.AddCell(new PdfPCell() { Phrase = new Phrase(styleShip.Value.BuyOrdStyle.ItemName, fontTinyItalic) });


                        table2.AddCell(new PdfPCell() { Phrase = new Phrase("Quantity", fontTinyItalic), BackgroundColor = BaseColor.GRAY });
                        table2.AddCell(":");
                        table2.AddCell(new PdfPCell() { Phrase = new Phrase(styleShip.Value.BuyOrdStyle.Quantity.ToString(), fontTinyItalic) });
                        pdfDoc.Add(table2);

                        PdfPTable table3 = new PdfPTable(styleShip.Value.OrderShipTable.Columns.Count);
                        foreach (DataColumn col in styleShip.Value.OrderShipTable.Columns)
                        {

                            table1.HorizontalAlignment = 1;
                            table3.AddCell(new PdfPCell() { Phrase = new Phrase(col.ColumnName, fontTinyItalic), BackgroundColor = BaseColor.YELLOW });
                            pdfDoc.Add(table3);

                        }
                        foreach (DataRow row in styleShip.Value.OrderShipTable.Rows)
                        {

                            PdfPTable table4 = new PdfPTable(styleShip.Value.OrderShipTable.Columns.Count);
                            foreach (DataColumn col in styleShip.Value.OrderShipTable.Columns)
                            {
                                table1.HorizontalAlignment = 1;
                                table4.AddCell(new PdfPCell() { Phrase = new Phrase(row[col.ColumnName].ToString(), fontTinyItalic), VerticalAlignment = Element.ALIGN_RIGHT, HorizontalAlignment = Element.ALIGN_MIDDLE });
                                pdfDoc.Add(table4);
                            }

                        }
                    }
                    pdfDoc.Close();
                }
                byte[] byteInfo = msReport.ToArray();
                msReport.Write(byteInfo, 0, byteInfo.Length);
                msReport.Position = 0;
                return new FileStreamResult(msReport, "application/pdf");
            }
            catch (Exception exception)
            {


                ViewBag.ErrorMessage = exception.Message;
                return PartialView("Error");
            }


        }
    }



}