using System;
using System.Collections.Generic;
using System.Linq;
using SCERP.BLL.IManager.ICommonManager;
using SCERP.Common;
using SCERP.DAL.IRepository.ICommercialRepository;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model;
using SCERP.Model.CommercialModel;
using SCERP.Model.Production;

namespace SCERP.BLL.Manager.CommercialManager
{
    public class ExportManager : IExportManager
    {
        private readonly IExportRepository _exportRepository;
        private readonly IPackingListRepository _packingListRepository;
        private readonly IExportDetailRepository _exportDetailRepository;
        private readonly IBuyerOrderRepository _buyerOrderRepository;
        

        public ExportManager(IExportRepository exportRepository, IExportDetailRepository exportDetailRepository, IBuyerOrderRepository buyerOrderRepository, IPackingListRepository packingListRepository)
        {
            _exportRepository = exportRepository;
            _exportDetailRepository = exportDetailRepository;
            _packingListRepository = packingListRepository;
            _buyerOrderRepository = buyerOrderRepository;
        }

        public List<CommExport> GetExportByPaging(ProSearchModel<CommExport> model, out int totalRecords)
        {
            var index = model.PageIndex;

            string compId = PortalContext.CurrentUser.CompId;
            var pageSize = AppConfig.PageSize;

            IQueryable<CommExport> exports = _exportRepository.GetWithInclude(x => x.CompId == compId &&
                                                                                   ((x.InvoiceNo.Contains(model.SearchString.Trim()) || String.IsNullOrEmpty(model.SearchString))
                                                                                    || (x.COMMLcInfo.LcNo.Contains(model.SearchString) || String.IsNullOrEmpty(model.SearchString)))
                                                                                    || (x.BillOfLadingNo.Contains(model.SearchString.Trim()) || String.IsNullOrEmpty(model.SearchString))
                                                                                    || (x.SBNo.Contains(model.SearchString.Trim())  || String.IsNullOrEmpty(model.SearchString))
                                                                                    || (x.COMMLcInfo.FirstAuditStatus.Contains(model.SearchString.Trim()) || String.IsNullOrEmpty(model.SearchString))
                                                                                    || (x.ExportNo.Contains( model.SearchString.Trim()) || String.IsNullOrEmpty(model.SearchString))
                                                                                    || (x.FinalDestination.Contains(model.SearchString.Trim()) || String.IsNullOrEmpty(model.SearchString))
                                                                                    || (x.BankRefNo.Contains(model.SearchString.Trim()) || String.IsNullOrEmpty(model.SearchString))
                                                                                    || (x.ExportNo.Contains(model.SearchString.Trim()) || String.IsNullOrEmpty(model.SearchString))
                                                                                    
                                                                                   && ((x.RealizedDate >= model.FromDate || model.FromDate == null)
                                                                                       && (x.RealizedDate <= model.ToDate || model.ToDate == null)), "COMMLcInfo");

            totalRecords = exports.Count();

            switch (model.sort)
            {
                case "ExportNo":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            exports = exports
                                .OrderByDescending(r => r.ExportNo)
                                .Skip(index*pageSize)
                                .Take(pageSize);
                            break;
                        default:
                            exports = exports
                                .OrderBy(r => r.ExportNo)
                                .Skip(index*pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                case "InvoiceNo ":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            exports = exports
                                .OrderByDescending(r => r.InvoiceNo)
                                .Skip(index*pageSize)
                                .Take(pageSize);
                            break;
                        default:
                            exports = exports
                                .OrderBy(r => r.InvoiceNo)
                                .Skip(index*pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                case "BankRefNo ":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            exports = exports
                                .OrderByDescending(r => r.BankRefNo)
                                .Skip(index*pageSize)
                                .Take(pageSize);
                            break;
                        default:
                            exports = exports
                                .OrderBy(r => r.BankRefNo)
                                .Skip(index*pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                case "RealizedValue":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            exports = exports
                                .OrderByDescending(r => r.RealizedValue)
                                .Skip(index*pageSize)
                                .Take(pageSize);
                            break;
                        default:
                            exports = exports
                                .OrderBy(r => r.RealizedValue)
                                .Skip(index*pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                case "RealizedDate":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            exports = exports
                                .OrderByDescending(r => r.RealizedDate)
                                .Skip(index*pageSize)
                                .Take(pageSize);
                            break;
                        default:
                            exports = exports
                                .OrderBy(r => r.RealizedDate)
                                .Skip(index*pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                case "BillOfLadingNo":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            exports = exports
                                .OrderByDescending(r => r.BillOfLadingNo)
                                .Skip(index*pageSize)
                                .Take(pageSize);
                            break;
                        default:
                            exports = exports
                                .OrderBy(r => r.BillOfLadingNo)
                                .Skip(index*pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                default:
                    exports = exports
                        .OrderByDescending(r => r.ExportRefId)
                        .Skip(index*pageSize)
                        .Take(pageSize);
                    break;
            }
            return exports.ToList();
        }

        public CommExport GetExportById(long exportId)
        {
            string compId = PortalContext.CurrentUser.CompId;
            CommExport export = _exportRepository.FindOne(x => x.CompId == compId && x.ExportId == exportId);
            if (export == null)
            {
                throw new ArgumentException("Export Not found");
            }
            return export;
        }

        public List<CommExport> GetExportByLcId(int LcId)
        {
            string compId = PortalContext.CurrentUser.CompId;
            List<CommExport> export = _exportRepository.Filter(x => x.CompId == compId && x.LcId == LcId).ToList();
            if (export == null)
            {
                throw new ArgumentException("Export Not found");
            }
            return export;
        }

        public string GetNewExportRefId()
        {
            var compId = PortalContext.CurrentUser.CompId;
            var exportRef = _exportRepository.Filter(x => x.CompId == compId).Max(x => x.ExportRefId);
            return exportRef.IncrementOne().PadZero(7);
        }

        public int EditExport(CommExport commExport)
        {
            var compId = PortalContext.CurrentUser.CompId;
            var editIndex = 0;
            var isExist = _exportRepository.Exists(x => x.CompId == compId && x.InvoiceNo == commExport.InvoiceNo && x.ExportId != commExport.ExportId);
            if (!isExist)
            {
                var export = _exportRepository.FindOne(x => x.CompId == compId && x.ExportId == commExport.ExportId);
                if (export == null)
                {
                    throw new ArgumentException("Export Not Found");
                }
                else
                {
                    export.ExportRefId = commExport.ExportRefId;
                    export.ExportNo = commExport.ExportNo;
                    export.ExportDate = commExport.ExportDate;
                    export.LcId = commExport.LcId;
                    export.SalseContactId = commExport.SalseContactId;
                    export.InvoiceNo = commExport.InvoiceNo;
                    export.InvoiceDate = commExport.InvoiceDate;
                    export.InvoiceQuantity = commExport.InvoiceQuantity;
                    export.InvoiceValue = commExport.InvoiceValue;
                    export.RealizedDate = commExport.RealizedDate;
                    export.RealizedValue = commExport.RealizedValue;
                    export.ShortFallAmount = commExport.ShortFallAmount;
                    export.ShortFallReason = commExport.ShortFallReason;
                    export.BankRefDate = commExport.BankRefDate;
                    export.PaymentMode = commExport.PaymentMode;
                    export.ShipmentMode = commExport.ShipmentMode;
                    export.BankRefNo = commExport.BankRefNo;
                    export.BillOfLadingDate = commExport.BillOfLadingDate;
                    export.BillOfLadingNo = commExport.BillOfLadingNo;
                    export.SBNo = commExport.SBNo;
                    export.PortOfDischarge = commExport.PortOfDischarge;
                    export.PortOfLanding = commExport.PortOfLanding;
                    export.FinalDestination = commExport.FinalDestination;
                    export.IncoTerm = commExport.IncoTerm;
                    export.FinalDestination = commExport.FinalDestination;
                    export.SBNoDate = commExport.SBNoDate;
                    export.FcAmount = commExport.FcAmount;
                    export.UdNoLocal = commExport.UdNoLocal;
                    export.UdNoForeign = commExport.UdNoForeign;
                    export.UdDateLocal = commExport.UdDateLocal;
                    export.UdDateForeign = commExport.UdDateForeign;
                    export.EditedBy = PortalContext.CurrentUser.UserId;
                    export.EditedDate = DateTime.Now;
                    editIndex = _exportRepository.Edit(export);
                }
            }
            else
            {
                throw new ArgumentException("Invoice Already Exist !");
            }
            return editIndex;
        }

        public int SaveExport(CommExport commExport)
        {
            var saveIndex = 0;
            var compId = PortalContext.CurrentUser.CompId;
            var isExist = _exportRepository.Exists(x => x.CompId == compId && x.InvoiceNo == commExport.InvoiceNo);
            if (!isExist)
            {
                commExport.CompId = PortalContext.CurrentUser.CompId;
                commExport.CreatedBy = PortalContext.CurrentUser.UserId;
                commExport.CreateDate = DateTime.Now;
                saveIndex = _exportRepository.Save(commExport);
            }
            else
            {
                throw new ArgumentException("This Invoice Already Exist !");
            }

            return saveIndex;
        }

        public int DeleteExport(long exportId)
        {
            int deleteIndex = 0;
            var compId = PortalContext.CurrentUser.CompId;
            var export = _exportRepository.FindOne(x => x.CompId == compId && x.ExportId == exportId);
            if (export != null)
            {
                deleteIndex = _exportRepository.DeleteOne(export);
            }
            else
            {
                throw new Exception("Delete Failed");
            }
            return deleteIndex;
        }

        public List<CommExport> GetExportLsit(DateTime? fromDate, DateTime? toDate, string searchString)
        {
            var compId = PortalContext.CurrentUser.CompId;
            IQueryable<CommExport> exports = _exportRepository.GetWithInclude(x => x.CompId == compId &&
                                                                                   ((x.InvoiceNo == searchString || String.IsNullOrEmpty(searchString))
                                                                                    || (x.COMMLcInfo.LcNo == searchString || String.IsNullOrEmpty(searchString)))
                                                                                   && ((x.RealizedDate >= fromDate || fromDate == null)
                                                                                       && (x.RealizedDate <= toDate || toDate == null)), "COMMLcInfo");
            return exports.ToList();
        }

        public List<OM_BuyerOrder> GetBuyerOrderbyExportId(long exportId)
        {
            string compId = PortalContext.CurrentUser.CompId;
            CommExport export = _exportRepository.FindOne(x => x.CompId == compId && x.ExportId == exportId);

            if (export != null)
            {
                var lcId = export.LcId;
                List<OM_BuyerOrder> buyerOrder = _buyerOrderRepository.GetWithInclude(p => p.CompId == compId && p.LcRefId == lcId).ToList();
                return buyerOrder;
            }
            return null;
        }

        public List<OM_BuyOrdStyle> GetStyleNoByOrderNo(string orderNo)
        {
            return _exportRepository.GetBuyerStyleByOrderNo(orderNo);
        }

        public int SaveExportDetail(CommExportDetail exportDetail)
        {
            var saveIndex = 0;

            exportDetail.CreatedBy = PortalContext.CurrentUser.UserId;
            exportDetail.CreatedDate = DateTime.Now;
            exportDetail.IsActive = true;

            saveIndex = _exportDetailRepository.Save(exportDetail);
            return saveIndex;
        }

        public int EditExportDetail(CommExportDetail expDetail)
        {
            var editIndex = 0;

            var exportDetail = _exportDetailRepository.FindOne(p => p.ExportId == expDetail.ExportId && p.OrderStyleRefId == expDetail.OrderStyleRefId);

            exportDetail.ExportId = expDetail.ExportId;
            exportDetail.OrderStyleRefId = expDetail.OrderStyleRefId;
            exportDetail.CartonQuantity = expDetail.CartonQuantity;
            exportDetail.ItemQuantity = expDetail.ItemQuantity;
            exportDetail.Rate = expDetail.Rate;
            exportDetail.ItemDescription = expDetail.ItemDescription;
            exportDetail.ExportCode = expDetail.ExportCode;
            exportDetail.EditedBy = PortalContext.CurrentUser.UserId;
            exportDetail.EditedDate = DateTime.Now;
            exportDetail.IsActive = true;

            editIndex = _exportDetailRepository.Edit(exportDetail);
            return editIndex;
        }

        public bool IsExistExportDetail(CommExportDetail expDetail)
        {
            CommExportDetail exportDetail = new CommExportDetail();
            exportDetail = _exportDetailRepository.FindOne(p => p.ExportId == expDetail.ExportId && p.OrderStyleRefId == expDetail.OrderStyleRefId);

            if (exportDetail != null)
                return true;
            else
                return false;
        }

        public List<CommExportDetail> GetExportDetailByExportId(long exportId)
        {
            List<CommExportDetail> exportDetail = _exportDetailRepository.GetWithInclude(p => p.IsActive && p.ExportId == exportId, "CommExport").ToList();
            return exportDetail;
        }

        public List<CommPackingListDetail> GetPackingDetailByExportId(long exportId)
        {
            List<CommPackingListDetail> packingListDetail = _packingListRepository.GetWithInclude(p => p.IsActive && p.ExportId == exportId, "CommExport").ToList();
            return packingListDetail;
        }

        public int SavePackDetail(CommPackingListDetail packingListDetail)
        {
            var saveIndex = 0;

            packingListDetail.CreatedBy = PortalContext.CurrentUser.UserId;
            packingListDetail.CreatedDate = DateTime.Now;
            packingListDetail.IsActive = true;

            saveIndex = _packingListRepository.Save(packingListDetail);
            return saveIndex;
        }

        public int EditPackDetail(CommPackingListDetail packDetail)
        {
            var editIndex = 0;

            var packingDetail = _packingListRepository.FindOne(p => p.ExportId == packDetail.ExportId && p.OrderStyleRefId == packDetail.OrderStyleRefId);

            packingDetail.ExportId = packDetail.ExportId;
            packingDetail.OrderStyleRefId = packDetail.OrderStyleRefId;
            packingDetail.Block = packDetail.Block;
            packingDetail.CountryName = packDetail.CountryName;
            packingDetail.ColorName = packDetail.ColorName;
            packingDetail.SizeName = packDetail.SizeName;
            packingDetail.CartonQuantity = packDetail.CartonQuantity;
            packingDetail.CartonCapacity = packDetail.CartonCapacity;
            packingDetail.CartonFrom = packDetail.CartonFrom;
            packingDetail.ContainerNo = packDetail.ContainerNo;

            packingDetail.EditedBy = PortalContext.CurrentUser.UserId;
            packingDetail.EditedDate = DateTime.Now;
            packingDetail.IsActive = true;

            editIndex = _packingListRepository.Edit(packingDetail);
            return editIndex;
        }
    }
}
