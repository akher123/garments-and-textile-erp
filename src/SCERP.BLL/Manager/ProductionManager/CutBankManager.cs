using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.DAL.IRepository.IProductionRepository;
using SCERP.Model.Production;

namespace SCERP.BLL.Manager.ProductionManager
{
    public class CutBankManager : ICutBankManager
    {
        private readonly ICutBankRepository _cutBankRepository;
        private readonly IBuyOrdStyleColorRepository colorRepository;
        public CutBankManager(IBuyOrdStyleColorRepository colorRepository, ICutBankRepository cutBankRepository)
        {
            _cutBankRepository = cutBankRepository;
            this.colorRepository = colorRepository;
        }

        public int UpdateCutBank(string orderStyleRefId)
        {
            string compId = PortalContext.CurrentUser.CompId;
            int updated=  _cutBankRepository.UpdateCutBank(compId, orderStyleRefId);
            return updated;
        }

        public List<VwCutBank> GetAllCutBank( string orderStyleRefId)
        {
            List<VwCutBank> cutBanks = _cutBankRepository.GetAllCutBank(PortalContext.CurrentUser.CompId, orderStyleRefId);
            return cutBanks.ToList();
        }

        public List<VwSewingInputDetail> GetAllCutBankByStyleColor(string orderStyleRefId, string colorRefId, string orderShipRefId)
        {
            List<VwSewingInputDetail> cutBanks = _cutBankRepository.GetAllCutBankByStyleColor(PortalContext.CurrentUser.CompId, orderStyleRefId, colorRefId, orderShipRefId);
            return cutBanks.ToList();
        }

        public Dictionary<string, Dictionary<string, List<string>>> GetPivotDictionaryByStyle(string orderStyleRefId)
        {
             var dic = new Dictionary<string, Dictionary<string, List<string>>>();
             List<VwSewingInputDetail> cutBanks = _cutBankRepository.GetPivotDictionaryByStyle(PortalContext.CurrentUser.CompId, orderStyleRefId);
             var colors=colorRepository.GetBuyOrdStyleColor(orderStyleRefId,PortalContext.CurrentUser.CompId);
             foreach (var color in colors)
             {
                var dictionary = new Dictionary<string, List<string>>();
                var sewingInputDetails = cutBanks.Where(x => x.ColorRefId == color.ColorRefId&&x.OrderStyleRefId==orderStyleRefId);
                List<string> sizeList = sewingInputDetails.Select(x => x.SizeName).ToList();
                sizeList.Add("TotalQty");
                List<string> sizeRefIdList = sewingInputDetails.Select(x => x.SizeRefId).ToList();
                List<string> orderQtyList = sewingInputDetails.Select(x => Convert.ToString(x.OrderQty)).ToList();
                orderQtyList.Add(Convert.ToString(sewingInputDetails.Sum(x => x.OrderQty)));
                List<string> bankList = sewingInputDetails.Select(x => Convert.ToString(x.BankQty)).ToList();
                bankList.Add(Convert.ToString(sewingInputDetails.Sum(x => x.BankQty)));
                List<string> totalInputQtylist = sewingInputDetails.Select(x => Convert.ToString(x.InputQuantity)).ToList();
                totalInputQtylist.Add(Convert.ToString(sewingInputDetails.Sum(x => x.InputQuantity)));
                List<string> inputPercentList = sewingInputDetails.Select(x => x.OrderQty > 0 ? String.Format("{0:0.00}" + " " + "%", (x.InputQuantity * 100.00m) / x.OrderQty) : "0").ToList();
                inputPercentList.Add( String.Format("{0:0.00}" + " " + "%", sewingInputDetails.Sum(x => x.InputQuantity) >0? sewingInputDetails.Sum(x => x.InputQuantity) * 100.0m / sewingInputDetails.Sum(x => x.OrderQty):0));
                dictionary.Add("Size", sizeList);
                dictionary.Add("OrderQty", orderQtyList);
                dictionary.Add("CuttBankQty", bankList);
                dictionary.Add("TotalInputQty", totalInputQtylist);
                dictionary.Add("Input %", inputPercentList);
                dic.Add(color.ColorName, dictionary);
            }
            return dic;
        }
    }
}
