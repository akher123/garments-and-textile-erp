using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.DAL.IRepository;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model.MerchandisingModel;

namespace SCERP.BLL.Manager.MerchandisingManager
{
    public  class BuyerTnaTemplateManager:IBuyerTnaTemplateManager
    {
        public IBuyerTnaTemplateRepository _buyerTnaTemplateRepository;
        public BuyerTnaTemplateManager(IBuyerTnaTemplateRepository buyerTnaTemplateRepository)
        {
            this._buyerTnaTemplateRepository = buyerTnaTemplateRepository;
         
        }

        public List<BuyerTnaTemplateModel> GetTemplates(string compId, string buyerRefId, int templateTypeId)
        {
           return _buyerTnaTemplateRepository.GetTemplates(compId, buyerRefId, templateTypeId);
        }

        public int SaveBuyerTnaTemplateLayout(List<OM_BuyerTnaTemplate> buyerTnaTemplates)
        {
            int saved = 0;
            OM_BuyerTnaTemplate  buyerTnaTemplat= buyerTnaTemplates.First();
            if (buyerTnaTemplat == null) 
                throw new ArgumentNullException("Missing Activity Duration !");
            _buyerTnaTemplateRepository.Delete(x =>
                x.BuyerRefId == buyerTnaTemplat.BuyerRefId && x.TemplateTypeId == buyerTnaTemplat.TemplateTypeId &&
                x.CompId == PortalContext.CurrentUser.CompId);
             saved=  _buyerTnaTemplateRepository.SaveList(buyerTnaTemplates);
            return saved;
        }
    }
}
