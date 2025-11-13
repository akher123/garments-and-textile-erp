
using System.Collections.Generic;
using SCERP.Model;

namespace SCERP.BLL.IManager.ICommonManager
{
    public interface IStateManager
    {
        List<State> GetStatesByPaging(State model, out int totalRecords);
        State GetStateById(int stateId);
        int EditState(State model);
        int SaveState(State model);
        int DeleteState(int stateId);
        List<State> GetStates();
        object GetStateByCountry(int countryId);
    }
}
