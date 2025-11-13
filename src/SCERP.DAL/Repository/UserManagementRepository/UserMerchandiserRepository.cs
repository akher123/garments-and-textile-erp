using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.IUserManagementRepository;
using SCERP.Model.UserRightManagementModel;

namespace SCERP.DAL.Repository.UserManagementRepository
{
    public class UserMerchandiserRepository :Repository<UserMerchandiser>, IUserMerchandiserRepository
    {
        public UserMerchandiserRepository(SCERPDBContext context) : base(context)
        {
        }
    }
}
