using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Remoting.Contexts;
using SCERP.DAL.IRepository.IMisRepository;
using SCERP.Model.Custom;

namespace SCERP.DAL.Repository.MisRepository
{
    public class MisDashboardRepository : IMisDashboardRepository
    {
        private readonly SCERPDBContext _context;
        public MisDashboardRepository(SCERPDBContext context)
        {
            _context = context;
        }

        public DataTable GetMerchadiserWiseOrderStyleDtable()
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;
            using (SqlCommand cmd = new SqlCommand("SpYearlyMerchandiserWiseOrderStatus"))
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

        public DataTable GetBuyerWiseOrderStyleDtable()
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;
            using (SqlCommand cmd = new SqlCommand("SpYearlyBuyerWiseOrderStatus"))
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

        public DataTable GetOrderStatusSummaryDtable()
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;
            using (SqlCommand cmd = new SqlCommand("SpYearlyBuyerWiseOrderStatusSummary"))
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

        public DataTable GetSpMISReprotDashBoard(string compId)
        {

            _context.Database.ExecuteSqlCommand("truncate Table MIS_DashBoard");
            for (int j = 0; j <= 4; j++)
            {
                _context.Database.ExecuteSqlCommand("INSERT INTO MIS_DashBoard ( BuyerRefId, DataType, M01 ) SELECT BuyerRefId, '" + j.ToString() + "', 0 AS DataType FROM OM_Buyer WHERE CompId = '" + compId + "'");
            }

            int adj = 0;
            int syear = 202000;

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



                if (ii + adj > 12)
                {
                    mv = Convert.ToString(syear + ii + adj - 12 + 100);
                }
                else
                {
                    mv = Convert.ToString(syear + ii + adj); 
                }
                
              

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

        public DataTable GetOrderStatusWithValue(string compId)
        {

            _context.Database.ExecuteSqlCommand("truncate Table MIS_DashBoard");
            for (int j = 1; j <= 2; j++)
            {
                _context.Database.ExecuteSqlCommand("INSERT INTO MIS_DashBoard ( BuyerRefId, DataType, M01 ) SELECT BuyerRefId, '" + j + "', 0 AS DataType FROM OM_Buyer WHERE CompId = '" + compId + "'");
            }

            int adj = 0;
            int syear = 202000;

            for (int ii = 1; ii <= 12; ii++)
            {
                string fnm;
                string mv;
                if (ii <= 9)
                {
                    fnm = "M0" + (ii);

                }
                else
                {
                    fnm = "M" + (ii);
                }


                if (ii + adj > 12)
                {
                    mv = Convert.ToString(syear + ii + adj - 12 + 100);
                }
                else
                {
                    mv = Convert.ToString(syear + ii + adj);
                }


                string sqlMuster = @"{0} AS Qty from OM_BuyerOrder as BO  inner join OM_BuyOrdStyle as BOST on BO.OrderNo=BOST.OrderNo and BO.CompId=BOST.CompId
                     inner join OM_BuyOrdShip as SH on BOST.OrderStyleRefId=SH.OrderStyleRefId and BOST.CompId=SH.CompId
               
                     WHERE (BOST.CompId = '" + compId + "') AND (BO.SCont = 'N') AND (BOST.ActiveStatus = 1) AND (YEAR(SH.ShipDate) * 100 + MONTH(SH.ShipDate) = " + mv + " ) AND (BO.BuyerRefId = MIS_DashBoard.BuyerRefId)";

                string sqtQuery = @"SELECT CONVERT(int,SUM(SH.Quantity-ISNULL(SH.DespatchQty,0)))";
                string sqlt = String.Format("" + sqlMuster + "", sqtQuery);

                _context.Database.ExecuteSqlCommand("Update  MIS_DashBoard set " + fnm + " = 0 where DataType='1'");
                _context.Database.ExecuteSqlCommand("Update  MIS_DashBoard set " + fnm + " =isnull((" + sqlt + "),0) where DataType='1'");

                sqtQuery = @"SELECT CONVERT(int, SUM((SH.Quantity-ISNULL(SH.DespatchQty,0))  *BOST.Rate)) ";
                sqlt = String.Format("" + sqlMuster + "", sqtQuery);

                _context.Database.ExecuteSqlCommand("Update  MIS_DashBoard set " + fnm + " = 0 where DataType='2'");
                _context.Database.ExecuteSqlCommand("Update  MIS_DashBoard set " + fnm + " =isnull((" + sqlt +
                                                    "),0) where DataType='2'");

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

        public DataTable GetBuyerOrderMaster(string BuyerRefId, string OrderNo, string OrderStyleRefId)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;
            using (SqlCommand cmd = new SqlCommand("SpMisBuyerOrderMasterDashboard"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@BuyerRefId", SqlDbType.VarChar, 7).Value = BuyerRefId;
                cmd.Parameters.Add("@OrderNo", SqlDbType.VarChar, 12).Value = OrderNo;
                cmd.Parameters.Add("@OrderStyleRefId", SqlDbType.VarChar, 7).Value = OrderStyleRefId;
                cmd.Connection = connection;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetLineWiseSewingProduction(int currentYear, string compId)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;
            using (SqlCommand cmd = new SqlCommand("SpProdLineWiseMonthlySewingProduction"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CurrntYear", SqlDbType.Int).Value = currentYear;
                cmd.Parameters.Add("@CompId", SqlDbType.VarChar, 3).Value = compId;
                cmd.Connection = connection;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable StyleSewingProduction(int currentYear, string currentMonth, int lineId, string compId)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;
            using (SqlCommand cmd = new SqlCommand("SpProdStyleWiseSewingProduction"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CurrentYear", SqlDbType.Int).Value = currentYear;
                cmd.Parameters.Add("@CurrentMonth", SqlDbType.VarChar).Value = currentMonth;
                cmd.Parameters.Add("@LineId", SqlDbType.Int).Value = lineId;
                cmd.Parameters.Add("@CompId", SqlDbType.VarChar, 3).Value = compId;
                cmd.Connection = connection;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable StyleSewingDetailProduction(int currentYear, string currentMonth, int lineId, string compId)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;
            using (SqlCommand cmd = new SqlCommand("SpProdStyleWiseSewingDetailProduction"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CurrentYear", SqlDbType.Int).Value = currentYear;
                cmd.Parameters.Add("@CurrentMonth", SqlDbType.VarChar).Value = currentMonth;
                cmd.Parameters.Add("@LineId", SqlDbType.Int).Value = lineId;
                cmd.Parameters.Add("@CompId", SqlDbType.VarChar, 3).Value = compId;
                cmd.Connection = connection;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable GetLineWiseDailySewingProduction(int currentYear, string currentMonth, string compId)
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;
            using (SqlCommand cmd = new SqlCommand("SpProdLineWiseDailySewingProduction"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CurrentYear", SqlDbType.Int).Value = currentYear;
                cmd.Parameters.Add("@CurrentMonth", SqlDbType.VarChar).Value = currentMonth;
                cmd.Parameters.Add("@CompId", SqlDbType.VarChar, 3).Value = compId;
                cmd.Connection = connection;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public DataTable CommarcialExportImport(string compId)
        {
            _context.Database.ExecuteSqlCommand("truncate Table MIS_DashBoard");
            for (int j = 0; j <= 3; j++)
            {
                _context.Database.ExecuteSqlCommand("INSERT INTO MIS_DashBoard ( BuyerRefId, DataType, M01 ) SELECT BuyerRefId, '" + j.ToString() + "', 0 AS M01 FROM OM_Buyer WHERE CompId = '" + compId + "'");
            }


            int jj = 0;
            int yc = 2016;
            int madj = 6;
            for (int ii = 1; ii <= 12; ii++)
            {
                string fnm;
                string fnmx;
                string mv;

                jj = ii + madj;

                if (ii <= 9)
                {
                    fnm = "M0" + (ii);

                }
                else
                {
                    fnm = "M" + (ii);
                }



                if (jj > 12)
                {
                    jj = jj - 12;
                    yc = yc + 1;
                }


                mv = Convert.ToString(yc * 100 + jj);


                string sqlt = @" select SUM(COMMLcInfo.LcAmount) from COMMLcInfo inner join OM_Buyer on 
                                COMMLcInfo.BuyerId=OM_Buyer.BuyerId where OM_Buyer.BuyerRefId=MIS_DashBoard.BuyerRefId 
                                and Year(COMMLcInfo.LcDate)*100+MONTH(COMMLcInfo.LcDate)='" + mv + "'";


                _context.Database.ExecuteSqlCommand("Update  MIS_DashBoard set " + fnm + " = 0 where DataType='0'");
                _context.Database.ExecuteSqlCommand("Update  MIS_DashBoard set " + fnm + " =isnull((" + sqlt +
                                                    "),0) where DataType='0'");
                sqlt = @"select SUM(CommBbLcInfo.BbLcAmount) from CommBbLcInfo
                          inner join COMMLcInfo on CommBbLcInfo.LcRefId=COMMLcInfo.LcId
                              inner join OM_Buyer on COMMLcInfo.BuyerId=OM_Buyer.BuyerId
                             where OM_Buyer.BuyerRefId=MIS_DashBoard.BuyerRefId 
                   and Year(CommBbLcInfo.BbLcDate)*100+MONTH(CommBbLcInfo.BbLcDate)='" + mv + "'";

                _context.Database.ExecuteSqlCommand("Update  MIS_DashBoard set " + fnm + " = 0 where DataType='1'");
                _context.Database.ExecuteSqlCommand("Update  MIS_DashBoard set " + fnm + " =isnull((" + sqlt +
                                                    "),0) where DataType='1'");

                sqlt = @"select SUM(CommExport.InvoiceValue) from CommExport
                  inner join COMMLcInfo on CommExport.LcId=COMMLcInfo.LcId
                        inner join OM_Buyer on COMMLcInfo.BuyerId=OM_Buyer.BuyerId
                        where OM_Buyer.BuyerRefId=MIS_DashBoard.BuyerRefId 
                   and Year(CommExport.InvoiceDate)*100+MONTH(CommExport.InvoiceDate)='" + mv + "'";
                _context.Database.ExecuteSqlCommand("Update  MIS_DashBoard set " + fnm + " = 0 where DataType='2'");
                _context.Database.ExecuteSqlCommand("Update  MIS_DashBoard set " + fnm + " =isnull((" + sqlt +
                                                    "),0) where DataType='2'");

                sqlt = @"select SUM(CommExport.RealizedValue) from CommExport
                  inner join COMMLcInfo on CommExport.LcId=COMMLcInfo.LcId
                        inner join OM_Buyer on COMMLcInfo.BuyerId=OM_Buyer.BuyerId
                        where OM_Buyer.BuyerRefId=MIS_DashBoard.BuyerRefId 
                   and Year(CommExport.RealizedDate)*100+MONTH(CommExport.RealizedDate)='" + mv + "'";
                _context.Database.ExecuteSqlCommand("Update  MIS_DashBoard set " + fnm + " = 0 where DataType='3'");
                _context.Database.ExecuteSqlCommand("Update  MIS_DashBoard set " + fnm + " =isnull((" + sqlt +
                                                    "),0) where DataType='3'");


            }

            SqlConnection connection = (SqlConnection)_context.Database.Connection;

            using (SqlCommand cmd = new SqlCommand("SpMisExportImportDashBoard"))
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

        public SpMisSewingProductionBoard GetSewingProductionBoard(DateTime currentDate, string compId)
        {
            var spParams = new SqlParameter[] { new SqlParameter("@CurrentDate", currentDate), new SqlParameter("@CompId", compId) };
            return _context.Database.SqlQuery<SpMisSewingProductionBoard>("spMisSewingProductionBoard @CurrentDate,@CompId", spParams).ToList()
                .FirstOrDefault();
        }

        public DataTable GetMontlyBuyerWiseProductionPlan()
        {
            SqlConnection connection = (SqlConnection)_context.Database.Connection;
            using (SqlCommand cmd = new SqlCommand("spMisMontlyBuyerWiseProductionPlan"))
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
