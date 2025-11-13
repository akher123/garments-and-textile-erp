using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Common.Controllers
{
    public class DocumentController : BaseController
    {
        public ActionResult DocumentFileUpload(HttpPostedFileBase file, string refId)
        {
            if (!Directory.Exists((Server.MapPath(@"~/UploadDocument"))))
            {
                Directory.CreateDirectory((Server.MapPath(@"~/UploadDocument")));
            }
            var flder = Guid.NewGuid();
            if (file != null && file.ContentLength > 0)
            {
                var documentFileName = flder + "_" + refId.Replace("/","") + (file.FileName);
                var path = Path.Combine(Server.MapPath(@"~/UploadDocument"), documentFileName);
                file.SaveAs(path);
                return Json(new { Success = true, DocumentFilePath = Url.Content(@"~/UploadDocument/" + documentFileName) }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteDocumnetFile(string filePath)
        {
            var mapPath = Server.MapPath(filePath);
            if (!System.IO.File.Exists(mapPath))
            {
                return Json(new { Success = false, FilePath = filePath });
            }
            System.IO.File.Delete(mapPath);
            return Json(new { Success = true, FilePath = filePath });
        }
        public FileResult DownLoad(string path)
        {
            var noSlash=  path.Count(x => x == '_');
            var fileName = path.Split('_')[noSlash];
            return !System.IO.File.Exists(Server.MapPath(path)) ? null : File(Server.MapPath(path), System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
	}
}