using SCERP.BLL.IManager.ICommercialManager;
using SCERP.Common;
using SCERP.DAL.IRepository;
using SCERP.Model;
using SCERP.Model.CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SCERP.BLL.Manager.CommercialManager
{
    public class ProFormaInvoiceManager : IProFormaInvoiceManager
    {
        public readonly IRepository<ProFormaInvoice> ProFormaInvoiceRepository;
        public ProFormaInvoiceManager(IRepository<ProFormaInvoice> ProFormaInvoiceRepository)
        {
            this.ProFormaInvoiceRepository = ProFormaInvoiceRepository;
        }

        public int DeleteProFormaInvoice(int piId)
        {
            ProFormaInvoice proFormaInvoice = ProFormaInvoiceRepository.FindOne(x => x.PiId == piId);
            return ProFormaInvoiceRepository.DeleteOne(proFormaInvoice);
        }

        public int EditProFormaInvoice(ProFormaInvoice model)
        {
            var ProFormaInvoice = ProFormaInvoiceRepository.FindOne(x => x.CompId == PortalContext.CurrentUser.CompId && x.PiId == model.PiId);
            ProFormaInvoice.PiNo = model.PiNo;
            ProFormaInvoice.ReceivedDate = model.ReceivedDate;
            ProFormaInvoice.SupplierId = model.SupplierId;
            ProFormaInvoice.EndDate = model.EndDate;
            ProFormaInvoice.Remarks = model.Remarks;
            ProFormaInvoice.EditedBy = model.EditedBy;
            ProFormaInvoice.EditedDate = model.EditedDate;
            return ProFormaInvoiceRepository.Edit(ProFormaInvoice);
        }

        public List<Mrc_SupplierCompany> GetAllSuppliers(string compId)
        {

            List<Mrc_SupplierCompany> suppliers = ProFormaInvoiceRepository.GetWithInclude(x => x.CompId == compId, "Mrc_SupplierCompany").Select(x => x.Mrc_SupplierCompany).Distinct().ToList();
            return suppliers;
        }

        public string GetNewRefId(string compId)
        {
            var maxRefId = ProFormaInvoiceRepository.Filter(x => x.CompId == compId).Max(x => x.PiRefId) ?? "0";
            return maxRefId.IncrementOne().PadZero(7);
        }

        public List<ProFormaInvoice> GetPiBySupplier(int supplierId, string compId)
        {
            List<ProFormaInvoice> pros = ProFormaInvoiceRepository.Filter(x => x.CompId == compId & x.SupplierId == supplierId).ToList();
            return pros;
        }

        public ProFormaInvoice GetProFormaInvoiceById(int piId)
        {
            return ProFormaInvoiceRepository.FindOne(x => x.PiId == piId);
        }

        public List<ProFormaInvoice> GetProFormaInvoiceByPaging(int pageIndex, string sort, string sortdir, string searchString, out int totalRecords)
        {
            var index = pageIndex;
            var pageSize = AppConfig.PageSize;
            var hourList =
                ProFormaInvoiceRepository.GetWithInclude(x => x.PiNo.Trim().Contains(searchString) || String.IsNullOrEmpty(searchString)
                             || x.PiRefId.Contains(searchString) || String.IsNullOrEmpty(searchString), "Mrc_SupplierCompany");
            totalRecords = hourList.Count();
            switch (sort)
            {
                case "PiNo":
                    switch (sortdir)
                    {
                        case "DESC":
                            hourList = hourList
                                 .OrderByDescending(r => r.PiNo)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            hourList = hourList
                                 .OrderBy(r => r.PiNo)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                default:
                    hourList = hourList
                        .OrderByDescending(r => r.PiRefId)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
            return hourList.ToList();
        }

        public ProFormaInvoice GetProFormaInvoiceByRefId(string compId, string purchaseOrderNo)
        {
            return ProFormaInvoiceRepository.FindOne(x=>x.CompId==compId&&x.PiRefId==purchaseOrderNo);
        }

        public List<ProFormaInvoice> GetProFormaInvoiceBySupplierIds(int[] supplierIds)
        {
            return ProFormaInvoiceRepository.Filter(x => supplierIds.Contains(x.SupplierId)).ToList();
        }

        public bool IsPiExist(int supplierId, string piNo, int PiId)
        {
            return ProFormaInvoiceRepository.Exists(
            x => x.CompId == PortalContext.CurrentUser.CompId && x.PiId != PiId && x.PiNo == piNo && x.SupplierId == supplierId);
        }

        public int SaveProFormaInvoice(ProFormaInvoice model)
        {
            model.CompId = PortalContext.CurrentUser.CompId;
            model.PiRefId = GetNewRefId(model.CompId);
            model.PType = "Y";
            return ProFormaInvoiceRepository.Save(model);
        }
    }
}
