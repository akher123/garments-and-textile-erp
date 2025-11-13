using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SCERP.Common;
using SCERP.DAL.IRepository.IInventoryRepository;
using SCERP.Model;
using SCERP.Model.InventoryModel;

namespace SCERP.DAL.Repository.InventoryRepository
{
    public class MaterialIssueRepository :Repository<Inventory_MaterialIssue>, IMaterialIssueRepository
    {
        public MaterialIssueRepository(SCERPDBContext context) : base(context)
        {

        }

        public VMaterialIssue GetVMaterialIssueById(int materialIssueId)
        {
           return Context.VMaterialIssues.SingleOrDefault(x => x.MaterialIssueId == materialIssueId);
        }

        public string GetNewIssueReceiveNo()
        {
            string issueReceiveNo =
                Context.Database.SqlQuery<string>(
                    "Select  substring(MAX(IssueReceiveNo),3,8 )from Inventory_MaterialIssue")
                    .SingleOrDefault() ?? "0";
            int maxNumericValue = Convert.ToInt32(issueReceiveNo);
            var irNo = "IR" + GetRefNumber(maxNumericValue, 6);
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
        public IQueryable<VMaterialIssue> GetMaterialIssuesByPaging(Expression<Func<VMaterialIssue, bool>> predicate)
        {
            return Context.VMaterialIssues.Where(predicate);
        }

        public IQueryable<VMaterialLoanReturn> GetMaterialLoanReturns(Expression<Func<VMaterialLoanReturn, bool>> predicate)
        {
            return Context.VMaterialLoanReturns.Where(predicate);
        }

        public IQueryable<VLoanGiven> GetMaterialLoanGiven()
        {
            return Context.VLoanGivens;
        }

        public VLoanGiven GetVLaonGivenById(int materialIssueId)
        {
            return Context.VLoanGivens.FirstOrDefault(x => x.MaterialIssueId == materialIssueId&&x.IType==4);
        }

        public IQueryable<VLoanGiven> GetMaterialLoanGiven(VLoanGiven model, out int totalRecords)
        {
            var index = model.PageIndex;
            var pageSize = AppConfig.PageSize;
            Expression<Func<VLoanGiven, bool>> predicate;
            predicate = x =>
                      ((x.LoanRefNo.Trim().Contains(model.SearchString) || String.IsNullOrEmpty(model.SearchString.Trim().ToLower()))
                      || (x.Supplyer.Trim().Contains(model.SearchString) || String.IsNullOrEmpty(model.SearchString.Trim().ToLower()))
                      || (x.IssueReceiveNo.Trim().Contains(model.SearchString) || String.IsNullOrEmpty(model.SearchString.Trim().ToLower())))
                     && ((x.IssueReceiveDate >= model.FromDate || model.FromDate == null) && (x.IssueReceiveDate <= model.ToDate || model.ToDate == null));
            IQueryable<VLoanGiven> loanGivens = Context.VLoanGivens.Where(predicate);
         
            totalRecords = loanGivens.Count();
            switch (model.sort)
            {

                case "LoanRefNo":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            loanGivens = loanGivens
                                .OrderByDescending(r => r.LoanRefNo)
                                .Skip(index*pageSize)
                                .Take(pageSize);
                            break;
                        default:
                            loanGivens = loanGivens
                                .OrderBy(r => r.LoanRefNo)
                                .Skip(index*pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                case "Supplyer":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            loanGivens = loanGivens
                                .OrderByDescending(r => r.Supplyer)
                                .Skip(index*pageSize)
                                .Take(pageSize);
                            break;
                        default:
                            loanGivens = loanGivens
                                .OrderBy(r => r.Supplyer)
                                .Skip(index*pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                case "IssueReceiveNo":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            loanGivens = loanGivens
                                .OrderByDescending(r => r.IssueReceiveNo)
                                .Skip(index*pageSize)
                                .Take(pageSize);
                            break;
                        default:
                            loanGivens = loanGivens
                                .OrderBy(r => r.IssueReceiveNo)
                                .Skip(index*pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                case "IssueReceiveDate":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            loanGivens = loanGivens
                                .OrderByDescending(r => r.IssueReceiveDate)
                                .Skip(index*pageSize)
                                .Take(pageSize);
                            break;
                        default:
                            loanGivens = loanGivens
                                .OrderBy(r => r.IssueReceiveDate)
                                .Skip(index*pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;
                default:
                    loanGivens = loanGivens
                        .OrderByDescending(r => r.IssueReceiveNo)
                        .Skip(index*pageSize)
                        .Take(pageSize);
                    break;
            }
            return loanGivens;
        }

        public DataTable GetChemicalIssueChallan(int materialIssueId)
        {
            SqlConnection connection = (SqlConnection)Context.Database.Connection;

            using (SqlCommand cmd = new SqlCommand("spInvChamicaleIssureChallan"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@MaterialIssueId", SqlDbType.Int).Value = materialIssueId;

                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetLoanReturnChallan(int materialIssueId)
        {
              SqlConnection connection = (SqlConnection)Context.Database.Connection;

              using (SqlCommand cmd = new SqlCommand("spLoanReturnChallanReport"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@MaterialIssueId", SqlDbType.Int).Value = materialIssueId;

                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
          
        }
    }
}
