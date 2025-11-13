using SCERP.DAL.IRepository.IMisRepository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.DAL.Repository.MisRepository
{
    public class MobileAppsRepository : IMobileAppsRepository
    {
        private readonly SCERPDBContext _context;
        public MobileAppsRepository(SCERPDBContext context)
        {
            _context = context;
        }

        public DataTable GetMonthlyShipmentSummary(string compId, DateTime? fromDate, DateTime? toDate)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;
            using (SqlCommand cmd = new SqlCommand("SpMonthlyShipmentSummary"))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = fromDate;
                cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = toDate;
                cmd.Parameters.Add("@CompId", SqlDbType.VarChar).Value = compId;

                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }


        public DataTable GetSpMISReprotDashBoard(string compId)
        {

            _context.Database.ExecuteSqlCommand("truncate Table MIS_DashBoard");
            for (int j = 0; j <= 4; j++)
            {
                _context.Database.ExecuteSqlCommand("INSERT INTO MIS_DashBoard ( BuyerRefId, DataType, M01 ) SELECT BuyerRefId, '" + j.ToString() + "', 0 AS DataType FROM OM_Buyer WHERE CompId = '" + compId + "'");
            }

            for (int ii = 1; ii <= 12; ii++)
            {
                string fnm;
                string fnmx;
                string mv;
                if (ii <= 9)
                {
                    fnm = "M0" + (ii);

                }
                else
                {
                    fnm = "M" + (ii);
                }

                //if (ii <= 8)
                //{
                //    fnmx = "M0" + (ii + 1);

                //}
                //else
                //{
                //    fnmx = "M" + (ii + 1);
                //}


                mv = Convert.ToString(201800 + ii);
                //if (ii <= 2)
                //{
                //    mv = Convert.ToString(201600 + ii + 10);
                //}
                //else
                //{
                //    mv = Convert.ToString(201700 + ii - 2);
                //}

                string sqlMuster =
                    @"{0} AS Qty
                             FROM  OM_BuyOrdStyle AS OS INNER JOIN
                             OM_BuyerOrder AS O ON OS.CompId = O.CompId AND OS.OrderNo = O.OrderNo
                            inner join OM_BuyOrdShip as SH on OS.OrderStyleRefId=SH.OrderStyleRefId and OS.CompId=SH.CompId
                            WHERE   (OS.CompId = '" + compId + "') AND (O.SCont = 'N') AND (OS.ActiveStatus = 1) AND (YEAR(SH.ShipDate) * 100 + MONTH(SH.ShipDate) = " +
                    mv + ") AND (O.BuyerRefId = MIS_DashBoard.BuyerRefId)";


                string sqtQuery = @"SELECT CONVERT(int, SUM(isnull(SH.Quantity,0)-isnull(SH.DespatchQty,0)))";
                string sqlt = String.Format("" + sqlMuster + "", sqtQuery);

                _context.Database.ExecuteSqlCommand("Update  MIS_DashBoard set " + fnm + " = 0 where DataType='1'");
                _context.Database.ExecuteSqlCommand("Update  MIS_DashBoard set " + fnm + " =isnull((" + sqlt +
                                                    "),0) where DataType='1'");

                sqtQuery = @"SELECT CONVERT(int, SUM(isnull(SH.Quantity*OS.Rate,0)-isnull(SH.DespatchQty*OS.Rate,0))) ";
                sqlt = String.Format("" + sqlMuster + "", sqtQuery);

                _context.Database.ExecuteSqlCommand("Update  MIS_DashBoard set " + fnm + " = 0 where DataType='2'");
                _context.Database.ExecuteSqlCommand("Update  MIS_DashBoard set " + fnm + " =isnull((" + sqlt +
                                                    "),0) where DataType='2'");

                sqtQuery = @"SELECT CONVERT(int, SUM(OS.despatchQty))";
                sqlt = String.Format("" + sqlMuster + "", sqtQuery);

                _context.Database.ExecuteSqlCommand("Update  MIS_DashBoard set " + fnm + " = 0 where DataType='3'");
                _context.Database.ExecuteSqlCommand("Update  MIS_DashBoard set " + fnm + " =isnull((" + sqlt +
                                                    "),0) where DataType='3'");
                //if (fnmx == "M13")
                //{

                //}
                //else
                //{
                //_context.Database.ExecuteSqlCommand("Update  MIS_DashBoard set " + fnmx + " = 0 where DataType='0'");
                //_context.Database.ExecuteSqlCommand("Update  MIS_DashBoard set " + fnmx + " =((SELECT top 1 " + fnm + " FROM MIS_DashBoard as C WHERE (BuyerRefId = MIS_DashBoard.BuyerRefId) AND DataType = '0') + (SELECT top 1 " + fnm + " FROM MIS_DashBoard as A WHERE (BuyerRefId = MIS_DashBoard.BuyerRefId) AND DataType = '1')-(SELECT  top 1  " + fnm + " FROM MIS_DashBoard as B WHERE (BuyerRefId = MIS_DashBoard.BuyerRefId) AND DataType = '3')) where DataType='0'");
                //}
                _context.Database.ExecuteSqlCommand("Update  MIS_DashBoard set " + fnm + " = 0 where DataType='4'");
                _context.Database.ExecuteSqlCommand("Update  MIS_DashBoard set " + fnm + " =((SELECT  top 1 " + fnm + " FROM MIS_DashBoard as C WHERE (BuyerRefId = MIS_DashBoard.BuyerRefId) AND DataType = '0')+(SELECT  top 1 " + fnm + " FROM MIS_DashBoard as A WHERE (BuyerRefId = MIS_DashBoard.BuyerRefId) AND DataType = '1')-(SELECT  top 1 " + fnm + " FROM MIS_DashBoard as B WHERE (BuyerRefId = MIS_DashBoard.BuyerRefId) AND DataType = '3')) where DataType='4'");

            }
            SqlConnection connection = (SqlConnection)_context.Database.Connection;

            using (SqlCommand cmd = new SqlCommand("SpMISReprotDashBoard"))
            {


                cmd.Connection = connection;

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
