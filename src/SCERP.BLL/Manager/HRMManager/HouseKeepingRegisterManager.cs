using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.Common;
using SCERP.DAL.IRepository;
using SCERP.Model.HRMModel;

namespace SCERP.BLL.Manager.HRMManager
{
    public class HouseKeepingRegisterManager : IHouseKeepingRegisterManager
    {
        private readonly IRepository<HouseKeepingRegister> _houseKeepingRegisterRepository;
        public HouseKeepingRegisterManager(IRepository<HouseKeepingRegister> houseKeepingRegisterRepository)
        {
            _houseKeepingRegisterRepository = houseKeepingRegisterRepository;
        }

        public List<HouseKeepingRegister> GetHouseKeepingRegisters(int pageIndex, string sort, string sortdir, string searchString,
            out int totalRecords)
        {
            var index = pageIndex;
            var pageSize = AppConfig.PageSize;
            var houseKeepings =
                _houseKeepingRegisterRepository
                .GetWithInclude(x => x.Employee.EmployeeCardId
                    .Trim()
                    .Contains(searchString) || String.IsNullOrEmpty(searchString)
                    || x.Employee.EmployeeCardId.Contains(searchString) || String.IsNullOrEmpty(searchString), "Employee", "HouseKeepingItem");
            totalRecords = houseKeepings.Count();
            houseKeepings = houseKeepings
                          .OrderByDescending(r => r.Employee.EmployeeCardId)
                          .Skip(index * pageSize)
                          .Take(pageSize);
            return houseKeepings.ToList();
        }

        public HouseKeepingRegister GetHouseKeepingRegisterById(int houseKeepingRegisterId)
        {
            var houseKeepings =
                _houseKeepingRegisterRepository.GetWithInclude(x => x.HouseKeepingRegisterId == houseKeepingRegisterId,"Employee").FirstOrDefault();
            return houseKeepings;
        }
        public int EditHouseKeepingRegister(HouseKeepingRegister houseKeepingRegister)
        {
            HouseKeepingRegister model = _houseKeepingRegisterRepository.FindOne(x => x.HouseKeepingRegisterId == houseKeepingRegister.HouseKeepingRegisterId);
            model.HouseKeepingItemId = houseKeepingRegister.HouseKeepingItemId;
            model.Remarks = houseKeepingRegister.Remarks;
            model.EmployeeId = houseKeepingRegister.EmployeeId;
            model.Quantity = houseKeepingRegister.Quantity;
            model.Rate = houseKeepingRegister.Rate;
            model.ReturnDate = houseKeepingRegister.ReturnDate;
            model.ReturnQty = houseKeepingRegister.ReturnQty;
            model.CompId = houseKeepingRegister.CompId;
            return _houseKeepingRegisterRepository.Edit(model);
        }

        public int SaveHouseKeepingResiter(HouseKeepingRegister model)
        {
            return _houseKeepingRegisterRepository.Save(model);
        }

        public int DeleteHouseKeepingRegister(HouseKeepingRegister hkg)
        {
            return _houseKeepingRegisterRepository.DeleteOne(hkg);
        }

        public DataTable GetHouseKeepingIssueReport(string cardId)
        {
            return
                _houseKeepingRegisterRepository.ExecuteQuery(String.Format("exec spHousekeepingRegister '{0}'", cardId));
        }
    }
}
