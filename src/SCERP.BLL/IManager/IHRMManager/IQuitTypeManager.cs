using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using System.Linq;

namespace SCERP.BLL.IManager.IHRMManager
{
    public interface IQuitTypeManager
    {
        List<QuitType> GetAllQuitTypes();
        List<QuitType> GetAllQuitTypesByPaging(int startPage, int pageSize, QuitType model, out int totalRecords);
        QuitType GetQuitTypeById(int quitTypeId);
        int EditQuitType(QuitType model);
        int SaveQuitType(QuitType model);
        bool IsExistQuitType(QuitType model);
        int DeleteQuitType(int quitTypeId);
        List<QuitType> GetAllQuitTypesBySearchKey(QuitType model);
    }
}
