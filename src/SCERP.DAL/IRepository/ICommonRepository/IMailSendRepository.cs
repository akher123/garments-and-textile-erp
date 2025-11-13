using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.CommonModel;

namespace SCERP.DAL.IRepository.ICommonRepository
{
    public interface IMailSendRepository : IRepository<MailSend>
    {
        IQueryable<VwMailSend> GetAllMailSendByPaging(string compId, string searchString);
    }
}
