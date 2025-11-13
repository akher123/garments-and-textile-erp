using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.DAL.Repository.HRMRepository;
using SCERP.Model;
using SCERP.Model.Custom;

namespace SCERP.BLL.Manager.HRMManager
{
   public class EfficiencyRateManager:BaseManager,IEfficiencyRateManager
    {

       private readonly IEfficiencyRateRepository _efficiencyRateRepository = null;

       public EfficiencyRateManager(SCERPDBContext context)
       {
        _efficiencyRateRepository=new EfficiencyRateRepository(context);
       }


       public List<EfficiencyRate> GetAllEfficiencyByPaging(int startPage, int pageSize, out int totalRecords, SearchFieldModel searchFieldModel, EfficiencyRate model)
       {
           
           var efficiencyRates = new List<EfficiencyRate>();
           try
           {
               efficiencyRates = _efficiencyRateRepository.GetAllEfficiencyByPaging(startPage, pageSize, out totalRecords, searchFieldModel, model).ToList();

           }
           catch (Exception exception) 
           {
               Errorlog.WriteLog(exception);
               totalRecords = 0;
           }

           return efficiencyRates;
           
       }

       public EfficiencyRate GetEficiencyRateById(int efficiencyRateId)
       {

           EfficiencyRate efficiencyRate = null; 
           //var efficiencyRate = new EfficiencyRate();
           try
           {
               efficiencyRate =
                   _efficiencyRateRepository.FindOne(x => x.IsActive && x.EfficiencyRateId == efficiencyRateId);

           }
           catch (Exception exception)
           {
               throw new Exception(exception.Message);
           }
           return efficiencyRate;
       }


       public bool IsExistEfficiencyRate(EfficiencyRate efficiencyRate)
       {
           bool isExist = false;
           try
           {
               isExist =
                  _efficiencyRateRepository.Exists(
                      x =>
                          x.IsActive &&
                          x.EfficiencyRateId != efficiencyRate.EfficiencyRateId &&
                          x.SkillOperationId == efficiencyRate.SkillOperationId && x.Rate.Replace(" ", "").ToLower().Equals(efficiencyRate.Rate.Replace(" ", "").ToLower()));
           }
           catch (Exception exception)
           {
               throw new Exception(exception.InnerException.Message);
           }
           return isExist;
       }

       public int EditEfficiencyRate(EfficiencyRate efficiencyRate)
       {
           var editefficiencyrate = 0;
           try
           {
               efficiencyRate = _efficiencyRateRepository.FindOne(x => x.EfficiencyRateId == efficiencyRate.EfficiencyRateId && x.IsActive);
               efficiencyRate.FromDate = efficiencyRate.FromDate;
               efficiencyRate.ToDate = efficiencyRate.ToDate;
               efficiencyRate.EditedDate = DateTime.Now;
               efficiencyRate.EditedBy = PortalContext.CurrentUser.UserId;
               editefficiencyrate = _efficiencyRateRepository.Edit(efficiencyRate);
           }
           catch (Exception exception)
           {
               throw new Exception(exception.InnerException.Message);
           }

           return editefficiencyrate;
       }

       public int SaveEfficiencyRate(EfficiencyRate efficiencyRate)
       {
           var savedefficiencyrate = 0; 
           try
           {
             
               efficiencyRate.CreatedDate = DateTime.Now;
               efficiencyRate.CreatedBy = PortalContext.CurrentUser.UserId;
               efficiencyRate.IsActive = true;
               savedefficiencyrate = _efficiencyRateRepository.Save(efficiencyRate);
           }
           catch (Exception exception)
           {
               throw new Exception(exception.InnerException.Message);
           }

           return savedefficiencyrate;
       }

       public int DeleteEfficiencyRate(int efficiencyRateId)
       {

           var deleteIndex = 0;
           try
           { 
               var efficiencyRateObj = GetEficiencyRateById(efficiencyRateId);
               efficiencyRateObj.IsActive = false;
               efficiencyRateObj.CreatedDate = DateTime.Now;
               efficiencyRateObj.CreatedBy = PortalContext.CurrentUser.UserId;
               efficiencyRateObj.EditedDate = DateTime.Now;
               efficiencyRateObj.EditedBy = PortalContext.CurrentUser.UserId;
               deleteIndex = _efficiencyRateRepository.Edit(efficiencyRateObj);

           }
           catch (Exception exception)
           {

               throw new Exception(exception.Message);
           }
           return deleteIndex;

       }
    }
}
