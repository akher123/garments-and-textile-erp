using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.HRMModel;

namespace SCERP.BLL.IManager.IHRMManager
{
   public interface IExceptionDayManager
    {
       
       List<ExceptionDay> GetAllExceptionDayByPaging(ExceptionDay model, out int totalRecords);
       ExceptionDay GetExceptionDayByExceptionDayId(int exceptionDayId);
       int SaveExceptionDay(ExceptionDay model);
       int EditExceptionDay(ExceptionDay model);
       int DeleteExceptionDay(int exceptionDayId);

       bool IsExceptionDayExist(ExceptionDay model);
    }
}
