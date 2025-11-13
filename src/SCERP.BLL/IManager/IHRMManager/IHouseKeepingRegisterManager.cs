using System;
using System.Collections.Generic;
using System.Data;
using SCERP.Model.HRMModel;

namespace SCERP.BLL.IManager.IHRMManager
{
   public interface IHouseKeepingRegisterManager
    {
       List<HouseKeepingRegister> GetHouseKeepingRegisters(int pageIndex, string sort, string sortdir, string searchString, out int totalRecords);

       HouseKeepingRegister GetHouseKeepingRegisterById(int houseKeepingRegisterId);
       int EditHouseKeepingRegister(HouseKeepingRegister houseKeepingRegister);
       int SaveHouseKeepingResiter(HouseKeepingRegister houseKeepingRegister);
       int DeleteHouseKeepingRegister(HouseKeepingRegister hkg);

       DataTable GetHouseKeepingIssueReport(string cardId);
    }
}
