using System;
using System.Web.Mvc;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.Web.Areas.Merchandising.Models.ViewModel;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Merchandising.Controllers
{
    public class SampleStyleController : BaseController
    {
        private readonly ISampleStyleManager _sampleStyleManager;

        public SampleStyleController(ISampleStyleManager sampleStyleManager)
        {
            _sampleStyleManager = sampleStyleManager;
        }

        public ActionResult Index(int sampleOrderId)
        {
            SampleStyleViewModel model = new SampleStyleViewModel
            {
                SampleStyles = _sampleStyleManager.GetSampleStyles(sampleOrderId),
                SampleOrderId = sampleOrderId
            };
            return View(model);
        }


        public ActionResult Edit(SampleStyleViewModel model)
        {
            ModelState.Clear();
            if (model.SampleStyle.SampleStyleId > 0)
            {
                model.SampleStyle = _sampleStyleManager.GetSampleStyleById(model.SampleStyle.SampleStyleId);
            }
            else
            {
                model.SampleStyle.SampleOrderId = model.SampleOrderId;
            }

            return View(model);
        }

        public ActionResult Save(SampleStyleViewModel model)
        {
            try
            {
                int saved = model.SampleStyle.SampleStyleId > 0
                    ? _sampleStyleManager.EditSampleStyle(model.SampleStyle)
                    : _sampleStyleManager.SaveSampleStyle(model.SampleStyle);
                if (saved > 0)
                {
                    ModelState.Clear();
                    model.SampleStyles = _sampleStyleManager.GetSampleStyles(model.SampleStyle.SampleOrderId);
                    return PartialView("~/Areas/Merchandising/Views/SampleStyle/Index.cshtml", model);
                }
                else
                {
                    return ErrorResult("Save Failed");
                }

            }
            catch (Exception e)
            {
                Errorlog.WriteLog(e);
                return ErrorResult(e.Message);
            }
        }
        public ActionResult CopyPaste(SampleStyleViewModel model)
        {
            ModelState.Clear();
            if (model.SampleStyle.SampleStyleId > 0)
            {
                model.SampleStyle = _sampleStyleManager.GetSampleStyleById(model.SampleStyle.SampleStyleId);
                model.SampleStyle.SampleStyleId = 0;
            }
            return View("~/Areas/Merchandising/Views/SampleStyle/Edit.cshtml", model);
        }


        public ActionResult Delete(int sampleStyleId)
        {
            var sample = _sampleStyleManager.GetSampleStyleById(sampleStyleId);
            int deleted = _sampleStyleManager.DeleteSampleStyle(sample);
            if (deleted > 0)
            {
                SampleStyleViewModel model = new SampleStyleViewModel
                {
                    SampleStyles = _sampleStyleManager.GetSampleStyles(sample.SampleOrderId)
                };
                return PartialView("~/Areas/Merchandising/Views/SampleStyle/Index.cshtml", model);
            }
            else
            {
                return ErrorResult("Save Failed");
            }
        }
    }
}