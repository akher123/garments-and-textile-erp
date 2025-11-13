using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model;

namespace SCERP.BLL.IManager.IInventoryManager
{
   public interface IInventoryAuthorizedPersonManager
    {
       List<Inventory_AuthorizedPerson> GetInventoryAuthorizedPersonsByPaging(out int totalRecords, Inventory_AuthorizedPerson model);
       Inventory_AuthorizedPerson GetAuthorizedPersonById(int authorizedPersonId);
       VEmployeeCompanyInfoDetail GetEmployeeByEmployeeCardId(string employeeCardId);
       int SaveInventoryAuthorizedPerson(Inventory_AuthorizedPerson model);
       int EditInventoryAuthorizedPerson(Inventory_AuthorizedPerson model);
       int DeleteInventoryAuthorizedPerson(int authorizedPersonId);
       bool CheckUserIsStorePerson(int processTypeId, int processId, Guid? userId);
       List<Inventory_AuthorizedPerson> GetAuthorizedPersonsByProcessTypeId(int processTypeId,int processId);
       IEnumerable GetAuthorizedPersons(int processTypeId, int processId);
       int FindSoterRequisiotionProcessId(int storePurchaseRequisition, Guid? userId);
       bool IsExistAuthorizedPerson(Inventory_AuthorizedPerson model);
    }
}
