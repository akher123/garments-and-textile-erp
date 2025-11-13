using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.BLL.IManager.IMerchandisingManager
{
    public interface IComponentManager
    {
        List<OM_Component> GetComponentByPaging(OM_Component model, out int totalRecords);
        OM_Component GetComponentById(long componentId);
        string GetComponentRefId();
        int EditComponent(OM_Component model);
        int SaveComponent(OM_Component model);
        int DeleteComponent(string componentRefId);
        List<OM_Component> GetComponents();
        bool CheckExistingComponent(OM_Component model);
        object AutoCompliteComponent(string searchString); 
    }
}
