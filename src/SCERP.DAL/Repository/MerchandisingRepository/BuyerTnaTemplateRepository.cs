using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model.MerchandisingModel;

namespace SCERP.DAL.Repository.MerchandisingRepository
{
    public class BuyerTnaTemplateRepository : Repository<OM_BuyerTnaTemplate>, IBuyerTnaTemplateRepository
    {
        public BuyerTnaTemplateRepository(SCERPDBContext context)
            : base(context)
        {

        }


        public List<BuyerTnaTemplateModel> GetTemplates(string compId, string buyerRefId, int templateTypeId)
        {

            string sp = string.Format(@"exec GetBuyerTnaTemplate @BuyerRefId='{0}',@TempTypeId={1},@CompId='{2}'", buyerRefId, templateTypeId, compId);
            return Context.Database.SqlQuery<BuyerTnaTemplateModel>(sp).ToList();
        }

        public int CreateTnaByBuyerTemplate(string compId, string buyerRefId, int defaultTemplate, string orderStyleRefId, string date)
        {
             int saved = 0;

              string sql = string.Format(@"INSERT INTO OM_TNA ( CompId, OrderStyleRefId, ActivityName, SerialId, LeadTime, PSDate, PEDate, Rmks, Responsible, ShortName, ActiveStatus)
              SELECT T.CompId, '{0}' AS OrderStyleRefId, A.Name, T.SerialNo AS SerialId, 0 AS LeadTime, '{1}' AS PSDate, '{1}' AS PEDate, '-' AS Rmks, A.Responsible, A.ShortName, 1 AS ActiveStatus
             FROM            OM_BuyerTnaTemplate AS T INNER JOIN
                             OM_TnaActivity AS A ON T.ActivityId = A.ActivityId
             WHERE        (T.CompId = '" + compId + "') AND T.BuyerRefId = '" + buyerRefId + "' AND T.TemplateTypeId = " + defaultTemplate + " AND T.SerialNo = 1 ", orderStyleRefId, date);

            Context.Database.ExecuteSqlCommand(sql);
            int limit= base.Count(x => x.BuyerRefId == buyerRefId&&x.CompId==compId);
            for (int i = 2; i <= limit; i++)
            {
                
            sql = string.Format(@"INSERT INTO OM_TNA ( CompId, OrderStyleRefId, ActivityName, SerialId, LeadTime, PSDate, PEDate, Rmks, Responsible, ShortName, ActiveStatus)
            SELECT T.CompId, '{0}' AS OrderStyleRefId, A.Name, T.SerialNo AS SerialId, 0 AS LeadTime, format(DATEADD(day, T.FDuration,
                             (SELECT        TOP (1) Convert(DATE,PEDate,103) 
                               FROM            OM_TNA
                               WHERE        (OrderStyleRefId = '{0}') AND (SerialId = T.RSerialNo))),'dd/MM/yyyy') AS PSDate,
                               format(DATEADD(day, T.FDuration + T.Duration,
                             (SELECT        TOP (1) Convert(DATE,PEDate,103)
                               FROM            OM_TNA AS OM_TNA_1
                               WHERE        (OrderStyleRefId = '{0}') AND (SerialId = T.RSerialNo))),'dd/MM/yyyy') AS PEDate, '-' AS Rmks, A.Responsible, A.ShortName, 1 AS ActiveStatus
           FROM  OM_BuyerTnaTemplate AS T INNER JOIN
                         OM_TnaActivity AS A ON T.ActivityId = A.ActivityId
           WHERE  T.CompId = '" + compId + "' AND T.BuyerRefId = '" + buyerRefId + "' AND T.TemplateTypeId = " + defaultTemplate + "  AND T.SerialNo = " + (i) + " AND T.RType ='E' ", orderStyleRefId);

            saved+=Context.Database.ExecuteSqlCommand(sql);

            sql = string.Format(@"INSERT INTO OM_TNA ( CompId, OrderStyleRefId, ActivityName, SerialId, LeadTime, PSDate, PEDate, Rmks, Responsible, ShortName, ActiveStatus)
            SELECT T.CompId, '{0}' AS OrderStyleRefId, A.Name, T.SerialNo AS SerialId, 0 AS LeadTime, format(DATEADD(day, T.FDuration,
                              (SELECT        TOP (1) Convert(DATE,PSDate,103) 
                               FROM            OM_TNA
                               WHERE        (OrderStyleRefId = '{0}') AND (SerialId = T.RSerialNo))),'dd/MM/yyyy') AS PSDate,
                              format(DATEADD(day, T.FDuration + T.Duration,
                              (SELECT        TOP (1) Convert(DATE,PSDate,103) 
                               FROM            OM_TNA AS OM_TNA_1
                               WHERE        (OrderStyleRefId = '{0}') AND (SerialId = T.RSerialNo))),'dd/MM/yyyy') AS PEDate, '-' AS Rmks, A.Responsible, A.ShortName, 1 AS ActiveStatus
           FROM  OM_BuyerTnaTemplate AS T INNER JOIN
                         OM_TnaActivity AS A ON T.ActivityId = A.ActivityId
           WHERE  T.CompId = '" + compId + "' AND T.BuyerRefId = '" + buyerRefId + "' AND T.TemplateTypeId = " + defaultTemplate + "  AND T.SerialNo = " + (i) + " AND T.RType ='S' ", orderStyleRefId);

            saved += Context.Database.ExecuteSqlCommand(sql);


            }

            return saved;
        }
    }
}
