using System.Collections;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IHRMRepository;
using SCERP.DAL.Repository.HRMRepository;
using SCERP.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SCERP.BLL.Manager.HRMManager
{
    public class BranchManager : BaseManager, IBranchManager
    {
        private readonly IBranchRepository _branchRepository = null;

        public BranchManager(SCERPDBContext context)
        {
            _branchRepository = new BranchRepository(context);
        }

        public List<Branch> GetAllBranchesByPaging(int startPage, int pageSize, Branch branch, out int totalRecords)
        {
            var branches = new List<Branch>();
            try
            {
                branches =
                    _branchRepository.GetAllBranchesByPaging(startPage, pageSize, out totalRecords, branch).ToList();

            }
            catch (Exception exception)
            {
                totalRecords = 0;
                Errorlog.WriteLog(exception);
            }

            return branches;
        }
        public List<Branch> GetAllBranches()
        {
            var branchces = new List<Branch>();
            try
            {
                branchces = _branchRepository.Filter(x=>x.IsActive).OrderBy(x=>x.Name).ToList();
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return branchces;
        }

        public Branch GetBranchById(int? id)
        {
            var branch=new Branch();
            try
            {
                branch = _branchRepository.GetBranchById(id);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                
            }
            return branch;
        }

        public bool CheckExistingBranch(Branch branch)
        {
            var isExist = false;
            try
            {
                isExist = _branchRepository.
                    Exists(x => x.IsActive && x.Id != branch.Id && x.Company.Id == branch.CompanyId && x.Name.Replace(" ", "").ToLower().Equals(branch.Name.Replace(" ", "").ToLower()));
            }
            catch (Exception excption)
            {
                Errorlog.WriteLog(excption);
            }
            return isExist;
        }



        public int SaveBranch(Branch branch)
        {
            var saveBranch = 0;

            try
            {
                branch.CreatedDate = DateTime.Now;
                branch.CreatedBy = PortalContext.CurrentUser.UserId;
                saveBranch = _branchRepository.Save(branch);
            }
         
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting  
                        // the current instance as InnerException  
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }   

            return saveBranch;
        }


        public int EditBranch(Branch branch)
        {
            var edit = 0;
            try
            {
                branch.EditedDate = DateTime.Now;
                branch.EditedBy = PortalContext.CurrentUser.UserId;
                edit = _branchRepository.Edit(branch);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                edit = 0;
            }
            return edit;
        }


        public int DeleteBranch(Branch branch)
        {
            var deleted = 0;
            try
            {
                branch.IsActive = false;
                deleted = _branchRepository.Edit(branch);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                deleted = 0;
            }
            return deleted;
        }


        public List<Branch> GetAllBranchesBySearchKey(string searchByBranchName, int searchByCompanyName)
        {
            var branches = new List<Branch>();

            try
            {
                branches = _branchRepository.GetAllBranchesBySearchKey(searchByBranchName, searchByCompanyName);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }

            return branches;
        }

        public List<Branch> GetAllBranchesByCompanyId(int id)
        {
            var branches = new List<Branch>();
            try
            {
                branches = _branchRepository.GetAllBranchesByCompanyId(id);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return branches;
        }

        public IEnumerable GetAllPermittedBranchesByCompanyId(int companyId) //From portal context
        {

            List<Common.PermissionModel.UserBranch> branches;
            try
            {
                branches = PortalContext.CurrentUser.PermissionContext.BranchList.Where(x => x.CompanyId == companyId).ToList();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            return branches;

        }

    }
}
