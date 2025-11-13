using System;
using System.Collections.Generic;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.DAL.Repository.HRMRepository;
using SCERP.Model;
namespace SCERP.BLL.Manager.HRMManager
{
   public class BankAccountTypeManager:IBankAccountTypeManager
   {
       private readonly IBankAccountTypeRepository _bankAccountTypeRepository = null;
       public BankAccountTypeManager(SCERPDBContext context)
       {
          _bankAccountTypeRepository=new BankAccountTypeRepository(context);
       }

       public List<BankAccountType> GetBankAccountTypes(int startPage, int pageSize, BankAccountType model, out int totalRecords)
       {
           List<BankAccountType> bankAccountTypes;
           try
           {
               bankAccountTypes = _bankAccountTypeRepository.GetBankAccountTypes(startPage, pageSize, model, out totalRecords);
           }
           catch (Exception exception)
           {

               throw new Exception(exception.Message);

           }
           return bankAccountTypes;
       }

       public BankAccountType GetBankAccountTypeById(int bankAccountTypeId)
       {
           BankAccountType bankAccountType;
           try
           {
               bankAccountType =
                   _bankAccountTypeRepository.FindOne(x => x.IsActive && x.BankAccountTypeId == bankAccountTypeId);
           }
           catch (Exception exception)
           {

               throw new Exception(exception.Message);
           }
           return bankAccountType;
       }

       public bool IsExistBankAccountType(BankAccountType model)
       {
           bool isExist;
           try
           {
               isExist = _bankAccountTypeRepository.Exists(x => x.IsActive == true
                   && x.BankAccountTypeId != model.BankAccountTypeId
                    && (x.AccountType.Replace("", " ").ToLower() == model.AccountType.Replace("", " ").ToLower()));
           }
           catch (Exception exception)
           {

               throw new Exception(exception.Message);
           }
           return isExist;
       }

       public int EditBankAccountType(BankAccountType model)
       {
           var editIndex = 0;
           try
           {
               var bankAccountTypeObj = _bankAccountTypeRepository.FindOne(x => x.IsActive && x.BankAccountTypeId == model.BankAccountTypeId);
               bankAccountTypeObj.AccountType = model.AccountType;
               bankAccountTypeObj.Description = model.Description;
               bankAccountTypeObj.EditedDate = DateTime.Now;
               bankAccountTypeObj.EditedBy = PortalContext.CurrentUser.UserId;
               editIndex = _bankAccountTypeRepository.Edit(bankAccountTypeObj);

           }
           catch (Exception exception)
           {

               throw new Exception(exception.Message);
           }
           return editIndex;
       }

       public int SaveBankAccountType(BankAccountType model)
       {
           var saveIndex = 0;
           try
           {
               model.CreatedBy = PortalContext.CurrentUser.UserId;
               model.CreatedDate = DateTime.Now;
               model.IsActive = true;
               saveIndex = _bankAccountTypeRepository.Save(model);
           }
           catch (Exception exception)
           {
               throw new Exception(exception.Message);
           }
           return saveIndex;
       }

       public int DeleteBankAccountType(int bankAccountTypeId)
       {
           var deleteIndex = 0;
           try
           {
               var bankAccountTypeObj = _bankAccountTypeRepository.FindOne(x => x.IsActive && x.BankAccountTypeId == bankAccountTypeId);
               bankAccountTypeObj.EditedDate = DateTime.Now;
               bankAccountTypeObj.EditedBy = PortalContext.CurrentUser.UserId;
               bankAccountTypeObj.IsActive = false;
               deleteIndex = _bankAccountTypeRepository.Edit(bankAccountTypeObj);
           }
           catch (Exception exception)
           {

               throw new Exception(exception.Message);
           }
           return deleteIndex;
       }
   }
}
