using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Common;
using SCERP.DAL.IRepository.IPlanningRepository;
using SCERP.Model.MerchandisingModel;

namespace SCERP.DAL.Repository.Planning
{
    public class TimeAndActionRepository : Repository<OM_TNA>, ITimeAndActionRepository
    {
        public TimeAndActionRepository(SCERPDBContext context)
            : base(context)
        {
        }

        public int UpdateTna(string compId, int tnaRowId, string key, string value)
        {
           string sqlQuery = String.Format("update OM_TNA SET {0}='{1}' , EditedDate='{4}' , EditedBy='{5}' where TnaRowId='{2}' and CompId='{3}'", key, value, tnaRowId, compId, DateTime.Now, PortalContext.CurrentUser.UserId);
           Context.Database.ExecuteSqlCommand(sqlQuery);
           Context.OM_TnaActivityLog.Add(new OM_TnaActivityLog() { EditedBy = PortalContext.CurrentUser.UserId.GetValueOrDefault(), EditedDate = DateTime.Now, TnaId = tnaRowId ,ValueText=value,KeyName=key});
           return Context.SaveChanges();
        }

        public object GetStyleWiseTna(string orderNo, string orderStyleRefId, string buyerRefId, string compId, string searchKey)
        {
            throw new NotImplementedException();
        }

        public DataTable GetHorizontalTna(string orderNo, string orderStyleRefId, string buyerRefId, string compId)
        {
          
            using (SqlConnection connection = (SqlConnection)Context.Database.Connection)
            {
                using (SqlCommand cmd = new SqlCommand("spTnaHorizontal"))
                {
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@BuyerRefId", SqlDbType.VarChar).Value = buyerRefId;
                    cmd.Parameters.Add("@OrderNo", SqlDbType.VarChar).Value =orderNo;
                    cmd.Parameters.Add("@OrderStyleRefId", SqlDbType.VarChar).Value = orderStyleRefId;
                    cmd.Parameters.Add("@CompId", SqlDbType.VarChar).Value = compId;
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
}
