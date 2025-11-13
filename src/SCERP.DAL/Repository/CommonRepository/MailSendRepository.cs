using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.DAL.IRepository.ICommonRepository;
using SCERP.Model.CommonModel;

namespace SCERP.DAL.Repository.CommonRepository
{
    public class MailSendRepository : Repository<MailSend>, IMailSendRepository
    {
        public MailSendRepository(SCERPDBContext context) : base(context)
        {
        }

        public IQueryable<VwMailSend> GetAllMailSendByPaging(string compId, string searchString)
        {
            return Context.VwMailSends.Where(x => x.CompId == compId && x.ModuleName == searchString || String.IsNullOrEmpty(searchString)
                       && x.ReportName == searchString || String.IsNullOrEmpty(searchString)
                       && x.FileName == searchString || String.IsNullOrEmpty(searchString)
                       && x.MailSubject == searchString || String.IsNullOrEmpty(searchString));
        }
    }
}
