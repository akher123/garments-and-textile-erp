using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using SCERP.Common;
using SCERP.Web.Areas.Common.Models.ViewModel;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Common.Controllers
{
    public class CustomReportController : BaseController
    {

        public ActionResult Index(CustomSqlQuaryViewModel model)
        {

            ModelState.Clear();
            var plist = new Dictionary<string, ReportParameterViewModel>();
            model.VUserReports = CustomSqlQuaryManager.GetUserReport(PortalContext.CurrentUser.Name);
            if (model.CustomSqlQuaryId > 0)
            {
                var customSqlQuery = CustomSqlQuaryManager.GetCustomSqlQuary(model.CustomSqlQuaryId);
                model.SqlQuary = customSqlQuery.SqlQuary;
                var reportParameters = CustomSqlQuaryManager.GetSqlReportParametersByReportId(model.CustomSqlQuaryId);
                foreach (var rp in reportParameters)
                {
                    var parameter = new ReportParameterViewModel
                    {
                        ReportParameterId = rp.ReportParameterId,
                        Pname = rp.Pname,
                        Querystring = rp.Querystring,
                        ControlType = rp.ControlType,
                        LabelFor = rp.LabelFor
                    };
                    if (rp.ControlType.Equals("dropdown"))
                    {
                        var rdModel = CustomSqlQuaryManager.GetDropDownListbySqlQuery(rp.Querystring);
                        parameter.Dropdowns = rdModel;
                    }
                    plist.Add(rp.Pvalue, parameter);
                }
                model.ReportParameterViewModels = plist;

            }

            return View(model);
        }
        public PartialViewResult PrintPreview(CustomSqlQuaryViewModel model)
        {

            DataTable sqlQueryResult;
            if (model.CustomSqlQuaryId > 0)
            {
                var customSqlQuery = CustomSqlQuaryManager.GetCustomSqlQuary(model.CustomSqlQuaryId);
                sqlQueryResult = CustomSqlQuaryManager.GetSqlQueryResult(!model.SqlQuary.IsNullOrWhiteSpace() ? model.SqlQuary : customSqlQuery.SqlQuary);

                model.DataTable = sqlQueryResult;
                model.Name = customSqlQuery.Name;
            }
            return PartialView("~/Areas/Common/Views/CustomSqlQuary/_PrintPreview.cshtml", model);

        }
        public ActionResult ShowReport(CustomSqlQuaryViewModel model)
        {

            ModelState.Clear();
            string sqlQuery = "";
            model.VUserReports = CustomSqlQuaryManager.GetUserReport(PortalContext.CurrentUser.Name);
            var customSqlQuery = CustomSqlQuaryManager.GetCustomSqlQuary(model.CustomSqlQuaryId);
            sqlQuery = customSqlQuery.SqlQuary;
            string param = "";
            string betParam = "";
            var parameters = model.ReportParameterViewModels.Where(x => x.Value.Pvalue != null);

            foreach (var key in model.ReportParameterViewModels.Keys.ToArray())
            {
                if (model.ReportParameterViewModels[key].ControlType.Equals("dropdown"))
                {
                       var rdModel = CustomSqlQuaryManager.GetDropDownListbySqlQuery(model.ReportParameterViewModels[key].Querystring);
                       model.ReportParameterViewModels[key].Dropdowns = rdModel;
                }

            }
            foreach (var rp in parameters)
            {
                if (rp.Key.Equals("FromDate") && rp.Value.ControlType.Equals("date"))
                {

                    betParam = rp.Value.Pname + " " + " between " + " " + " convert(datetime,'" + rp.Value.Pvalue + "',103) " + " and ";
                }
                else if (rp.Key.Equals("ToDate") && rp.Value.ControlType.Equals("date"))
                {
                    betParam += " " + " convert(datetime,'" + rp.Value.Pvalue + "',103) ";
                }
                else
                {
                    if (param.Length > 0)
                    {

                        param += " " + " and " + " ";
                    }
                    param += rp.Value.Pname + " = " + "'" + rp.Value.Pvalue + "'";
                }

            }

            if (betParam.Length > 0)
            {
                if (param.Length > 0)
                {
                    betParam = "and" + " " + betParam;
                }

            }
            if (parameters.Any())
            {
                param = "where" + " " + param;
            }

            sqlQuery = sqlQuery + " " + param + " " + betParam;
            DataTable sqlQueryResult = CustomSqlQuaryManager.GetSqlQueryResult(sqlQuery);
            model.DataTable = sqlQueryResult;
            model.SqlQuary = sqlQuery;
            return View("~/Areas/Common/Views/CustomReport/Index.cshtml", model);
        }
        public void GetExcel(CustomSqlQuaryViewModel model)
        {
            if (model.CustomSqlQuaryId > 0)
            {
                DataTable sqlQueryResult;
                if (!model.SqlQuary.IsNullOrWhiteSpace())
                {
                    sqlQueryResult = CustomSqlQuaryManager.GetSqlQueryResult(model.SqlQuary);
                }
                else
                {
                    var customSqlQuery = CustomSqlQuaryManager.GetCustomSqlQuary(model.CustomSqlQuaryId);
                    sqlQueryResult = CustomSqlQuaryManager.GetSqlQueryResult(customSqlQuery.SqlQuary);
                }

                model.DataTable = sqlQueryResult;
                var dt = sqlQueryResult;
                const string attachment = "attachment; filename=SQLExported.xls";
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/vnd.ms-excel";
                string tab = "";
                foreach (DataColumn dc in dt.Columns)
                {
                    Response.Write(tab + dc.ColumnName);
                    tab = "\t";
                }
                Response.Write("\n");
                foreach (DataRow dr in dt.Rows)
                {
                    tab = "";
                    for (var i = 0; i < dt.Columns.Count; i++)
                    {
                        if (dt.Columns[i].DataType.Name == "DateTime")
                        {
                            Response.Write(tab + String.Format("{0:dd-MM-yyyy}", dr[i]));
                        }
                        else
                        {
                            Response.Write(tab + dr[i].ToString());
                        }

                        tab = "\t";
                    }
                    Response.Write("\n");
                }
                Response.End();

            }

        }
    }
}