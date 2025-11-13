using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SCERP.Common;
using SCERP.DAL.IRepository.IInventoryRepository;
using SCERP.Model;


namespace SCERP.DAL.Repository.InventoryRepository
{
    public class QualityCertificateRepository : Repository<Inventory_QualityCertificate>, IQualityCertificateRepository
    {

        public QualityCertificateRepository(SCERPDBContext context)
            : base(context)
        {

        }
        public List<VQualityCertificate> GetQualityCertificateByPaging(out int totalRecords, VQualityCertificate model)
        {
            var index = model.PageIndex;
            var pageSize = AppConfig.PageSize;
            Expression<Func<VQualityCertificate, bool>> predicate = x =>(x.QCReferenceNo == model.QCReferenceNo || model.QCReferenceNo == null)
                     && (x.IsGrnConverted == model.IsGrnConverted || model.IsGrnConverted == null)&&
                     ((x.ReceivedRegisterNo.Trim().Contains(model.SearchString) || String.IsNullOrEmpty(model.SearchString.Trim().ToLower()))
                     ||(x.RequisitionNo.Trim().Contains(model.SearchString) ||String.IsNullOrEmpty(model.SearchString.Trim().ToLower()))
                     || (x.InvoiceNo.Trim().Contains(model.SearchString) || String.IsNullOrEmpty(model.SearchString.Trim().ToLower()))
                     || (x.QCReferenceNo.Trim().Contains(model.SearchString) || String.IsNullOrEmpty(model.SearchString.Trim().ToLower()))
                     || (x.SupplierCompanyName.Trim().Contains(model.SearchString) || String.IsNullOrEmpty(model.SearchString.Trim().ToLower()))
                     || (x.ReceivedRegisterNo.Trim().Contains(model.SearchString) || String.IsNullOrEmpty(model.SearchString.Trim().ToLower())))
                      && ((x.SendingDate >= model.FromDate || model.FromDate == null) && (x.SendingDate <= model.ToDate || model.ToDate == null));
            var vQualityCertificate = Context.VQualityCertificates.Where(predicate);
            totalRecords = vQualityCertificate.Count();
            switch (model.sort)
            {
                case "QCReferenceNo":

                    switch (model.sortdir)
                    {
                        case "DESC":
                            vQualityCertificate = vQualityCertificate
                                  .OrderByDescending(r => r.ReceivedDate)

                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            vQualityCertificate = vQualityCertificate
                                .OrderBy(r => r.ReceivedDate)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                default:
                    vQualityCertificate = vQualityCertificate
                               .OrderByDescending(r => r.ReceivedDate)
                               .Skip(index * pageSize)
                               .Take(pageSize);
                    break;

            }
            return vQualityCertificate.ToList();
        }


        public List<VQualityCertificateDetail> GetQualityCertificateDetailIds(long itemStoreId, int qualityCertificateId)
        {
            return Context.VQualityCertificateDetails.Where(x => x.ItemStoreId == itemStoreId && x.QualityCertificateId == qualityCertificateId).ToList();
        }

        //public string GetNewQCReferenceNo()
        //{
        //    string maxGroupCode = Context.Database.SqlQuery<string>(
        //    "select  RIGHT ('000000'+ CAST (isnull(max(QCReferenceNo),0)+1 AS varchar), 6) as QCReferenceNo from Inventory_QualityCertificate").SingleOrDefault();
        //    return Convert.ToString(maxGroupCode);
        //}


        public string GetNewQCReferenceNo()
        {
            var reqNo = Context.Database.SqlQuery<string>(
                    "Select  substring(MAX(QCReferenceNo),4,8 )from Inventory_QualityCertificate")
                    .SingleOrDefault() ?? "0";
            var maxNumericValue = Convert.ToInt32(reqNo);
            var irNo = "QCR" + GetRefNumber(maxNumericValue,5);
            return irNo;
        }

        private string GetRefNumber(int maxNumericValue, int length)
        {
            var refNumber = Convert.ToString(maxNumericValue + 1);
            while (refNumber.Length != length)
            {
                refNumber = "0" + refNumber;
            }
            return refNumber;
        }
    }
}
