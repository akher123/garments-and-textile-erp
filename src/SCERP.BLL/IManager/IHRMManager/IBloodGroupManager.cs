using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.BLL.IManager.IHRMManager
{
    public interface IBloodGroupManager
    {
        List<BloodGroup> GetAllBloodGroups();
        BloodGroup GetBloodGroupById(int? id);
    }
}
