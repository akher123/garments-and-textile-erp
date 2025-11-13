using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.CommercialModel;

namespace SCERP.BLL.IManager.ICommercialManager
{
    public interface IReceiveManager
    {
        List<CommReceive> GetAllReceivesByPaging(int startPage, int pageSize, out int totalRecords, CommReceive receive);

        List<CommReceive> GetAllReceives();

        CommReceive GetReceiveById(int? id);

        int SaveReceive(CommReceive receive);

        int EditReceive(CommReceive receive);

        int DeleteReceive(CommReceive receive);

        bool CheckExistingReceive(CommReceive receive);

        List<CommReceive> GetReceiveBySearchKey(int searchByCountry, string searchByCommReceive);
    }
}