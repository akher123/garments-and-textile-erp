using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.IAccountingManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IAccountingRepository;
using SCERP.Model.AccountingModel;

namespace SCERP.BLL.Manager.AccountingManager
{
    public class StylePaymentManager : IStylePaymentManager
    {
        private readonly IStylePaymentRepository _stylePaymentRepository;
        private readonly string _compId;
        public StylePaymentManager(IStylePaymentRepository stylePaymentRepository)
        {
            _stylePaymentRepository = stylePaymentRepository;
            _compId = PortalContext.CurrentUser.CompId;
        }
        public int DeleteStylePayment(int stylePaymnetId)
        {

            return _stylePaymentRepository.Delete(x => x.StylePaymnetId == stylePaymnetId);
        }

        public int EditStylePayment(Acc_StylePayment model)
        {
            Acc_StylePayment StylePayment = _stylePaymentRepository.FindOne(x => x.StylePaymnetId == model.StylePaymnetId);

            StylePayment.StylePaymnetId = model.StylePaymnetId;
            StylePayment.StylePaymentRefId = model.StylePaymentRefId;
            StylePayment.PayDate = model.PayDate;
            StylePayment.BuyerRefId = model.BuyerRefId;
            StylePayment.OrderNo = model.OrderNo;
            StylePayment.OrderStyleRefId = model.OrderStyleRefId;
            StylePayment.CostGroup = model.CostGroup;
            StylePayment.PayAount = model.PayAount;
            StylePayment.CompId = _compId;
            StylePayment.Remarks = model.Remarks;

            return _stylePaymentRepository.Edit(StylePayment);
        }

        public List<VStylePayment> GetAllStylePaymentByPaging(Acc_StylePayment model, out int totalRecords)
        {
            
            var index = model.PageIndex;
            var pageSize = AppConfig.PageSize;

            //var stylePaymentList = _stylePaymentRepository.Filter(x => (x.BuyerRefId.Trim().Contains(model.BuyerRefId.Trim()) || String.IsNullOrEmpty(model.BuyerRefId)) && (x.OrderNo.Trim().Contains(model.OrderNo.Trim()) || String.IsNullOrEmpty(model.OrderNo)) && (x.OrderStyleRefId.Trim().Contains(model.OrderStyleRefId.Trim()) || String.IsNullOrEmpty(model.OrderStyleRefId)) &&(x.CompId==_compId));
            IQueryable<VStylePayment> stylePaymentList= _stylePaymentRepository.GetStylePaymentView(x => (x.BuyerRefId.Trim().Contains(model.BuyerRefId.Trim()) || String.IsNullOrEmpty(model.BuyerRefId)) && (x.OrderNo.Trim().Contains(model.OrderNo.Trim()) || String.IsNullOrEmpty(model.OrderNo)) && (x.OrderStyleRefId.Trim().Contains(model.OrderStyleRefId.Trim()) || String.IsNullOrEmpty(model.OrderStyleRefId)));
            totalRecords = stylePaymentList.Count();
            

            stylePaymentList = stylePaymentList
                       .OrderByDescending(r => r.StylePaymnetId)
                       .Skip(index * pageSize)
                       .Take(pageSize);
            return stylePaymentList.ToList();
        }

        public List<Acc_StylePayment> GetAllStylePayments()
        {
            var stylePaymentList = _stylePaymentRepository.All();
            return stylePaymentList.ToList();
        }

        public string GetNewStylePaymentRefId(string prifix)
        {

            string max = _stylePaymentRepository.Filter(x => x.CompId == _compId).Max(x => x.StylePaymentRefId.Substring(2, 6));
            return prifix + max.IncrementOne().PadZero(6);
        }

        public Acc_StylePayment GetStylePaymentsByStylePaymentsId(int stylePaymentId)
        {
            string compId = PortalContext.CurrentUser.CompId;
            var itemList = _stylePaymentRepository.Filter(x => x.StylePaymnetId == stylePaymentId && x.CompId==compId).FirstOrDefault(x => x.StylePaymnetId == stylePaymentId);
            return itemList;
        }

        public bool IsStylePaymentExist(Acc_StylePayment model)
        {
            return _stylePaymentRepository.Exists(x => x.StylePaymnetId == model.StylePaymnetId && x.CompId == _compId && x.CostGroup == model.CostGroup && x.PayAount == model.PayAount && x.StylePaymentRefId == model.StylePaymentRefId && x.BuyerRefId == model.BuyerRefId && x.OrderNo == model.OrderNo);
        }

        public int SaveStylePayment(Acc_StylePayment model)
        {
            model.CompId = PortalContext.CurrentUser.CompId;

            return _stylePaymentRepository.Save(model);
        }
    }
}
