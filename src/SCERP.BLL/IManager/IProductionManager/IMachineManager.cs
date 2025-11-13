using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;
using SCERP.Model.Production;

namespace SCERP.BLL.IManager.IProductionManager
{
    public interface IMachineManager
    {
        List<Production_Machine> GetMachines(string processorRefId);
        List<VMachine> GetMachineListByPaging(Production_Machine model, out int totalRecords);
        Production_Machine GetMachineById(int machineId);
        int EditMachine(Production_Machine model);
        int SaveMachine(Production_Machine model);
        int DeleteColor(int machineId);
        string GetNewMachineRefId();
        List<Production_Machine> AutocompliteMechineByProcessor(string processorRefId);

        List<Production_Machine> GetLines();

        List<Production_Machine> GetMachines();
    }
}
