using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using SCERP.Common;
using SCERP.Model;
using SCERP.Model.CommonModel;
using SCERP.Web.Areas.Common.Models.ViewModel;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Common.Controllers
{
    public class CustomSqlQuaryController : BaseController
    {
        public ActionResult Index(CustomSqlQuaryViewModel model)
        {
           
            try
            {
                ModelState.Clear();
                model.CustomSqlQuaries = CustomSqlQuaryManager.GetCustomSqlQueres();
                model.TableNameList = CustomSqlQuaryManager.GetAllTableNames();
                if (model.SqlQuary != null)
                {
                    if (model.SqlQuary.ToLower().StartsWith("select") &&!model.SqlQuary.ToLower().Contains("into"))
                    {
                        var sqlQueryResult = CustomSqlQuaryManager.GetSqlQueryResult(model.SqlQuary);
                        model.DataTable = sqlQueryResult;

                        return View(model);
                    }
                    else
                    {
                        return ErrorResult("Only Select Statement is permited");
                    }
                  
                }
                else if(model.CustomSqlQuaryId>0)
                {
                    var customSqlQuery = CustomSqlQuaryManager.GetCustomSqlQuary(model.CustomSqlQuaryId);
                    DataTable sqlQueryResult = CustomSqlQuaryManager.GetSqlQueryResult(customSqlQuery.SqlQuary);
                    model.DataTable = sqlQueryResult;
                    model.SqlQuary = customSqlQuery.SqlQuary;
                    return View(model);
                }
                else
                {
                    return View(model);
                }


            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
                return ErrorResult(exception.Message);
            }
         
        }

        public ActionResult Save(CustomSqlQuaryViewModel model)
        {
            CustomSqlQuary quary=new CustomSqlQuary
            {
                SqlReportParameter = model.Parameters.Select(x => x.Value).ToList(),
                CustomSqlQuaryId = model.CustomSqlQuaryId,
                SqlQuaryRefId = model.SqlQuaryRefId,
                Name = model.Name,
                SqlQuary = model.SqlQuary,
                Description = model.Description
            };
            int saveIndex = (model.CustomSqlQuaryId > 0) ? CustomSqlQuaryManager.EditustomSqlQuary(quary) : CustomSqlQuaryManager.SaveCustomSqlQuary(quary);

           return saveIndex>0 ? Reload() : ErrorResult("Fail To Save");
        }
        public ActionResult Edit(CustomSqlQuaryViewModel model)
        {
            ModelState.Clear();
            model.SqlQuaryRefId = CustomSqlQuaryManager.GetNewSqlQuaryRefId();
            if (model.CustomSqlQuaryId>0)
            {
                var customSqlQuery= CustomSqlQuaryManager.GetCustomSqlQuary(model.CustomSqlQuaryId);
                model.Parameters = customSqlQuery.SqlReportParameter.ToDictionary(x => x.Pvalue, x => x);
                model.Name = customSqlQuery.Name;
                model.Description = customSqlQuery.Description;
                model.SqlQuaryRefId = customSqlQuery.SqlQuaryRefId;
                customSqlQuery.SqlReportParameter=new List<SqlReportParameter>();
            }
            return View(model);
        }

        public ActionResult ReportParameter(CustomSqlQuaryViewModel model)
        {
            model.Key = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            model.Parameters.Add(model.Key,new SqlReportParameter()
            {
                CustomSqlQuaryId=model.CustomSqlQuaryId

            } );
            return PartialView("~/Areas/Common/Views/CustomSqlQuary/_ReportParameter.cshtml", model);
        }


        public PartialViewResult PrintPreview(CustomSqlQuaryViewModel model)
        {

            if (model.CustomSqlQuaryId>0)
            {
                var customSqlQuery = CustomSqlQuaryManager.GetCustomSqlQuary(model.CustomSqlQuaryId);
                DataTable sqlQueryResult = CustomSqlQuaryManager.GetSqlQueryResult(customSqlQuery.SqlQuary);
                model.DataTable = sqlQueryResult;
                model.Name = customSqlQuery.Name;  
            }
            return PartialView("~/Areas/Common/Views/CustomSqlQuary/_PrintPreview.cshtml",model);

        }

        public void GetExcel(CustomSqlQuaryViewModel model)
        {
            if (model.SqlQuary != null)
            {
                if (model.SqlQuary.ToLower().StartsWith("select") && !model.SqlQuary.ToLower().Contains("into"))
                {
                    var sqlQueryResult = CustomSqlQuaryManager.GetSqlQueryResult(model.SqlQuary);
                    model.DataTable = sqlQueryResult;
                    DataTable dt = sqlQueryResult; 
                    var attachment = "attachment; filename=SQLExported.xls";
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
}