using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.ICommonManager;
using SCERP.Common;
using SCERP.DAL.IRepository.ICommonRepository;
using SCERP.Model.CommonModel;
using SCERP.Model;

namespace SCERP.BLL.Manager.CommonManager
{
    public class MailSendManager : IMailSendManager
    {
        public readonly IMailSendRepository _MailSendRepository;

        public MailSendManager(IMailSendRepository mailSendRepository)
        {
            _MailSendRepository = mailSendRepository;
        }
        public List<VwMailSend> GetAllMailSendByPaging(int pageIndex, string sort, string sortdir, string searchString, out int totalRecords)
        {
             var index = pageIndex;
            var pageSize = AppConfig.PageSize;

            var mailSendList = _MailSendRepository.GetAllMailSendByPaging(PortalContext.CurrentUser.CompId, searchString);
            totalRecords = mailSendList.ToList().Count();
             switch (sort)
            {
                case "PersonName":
                    switch (sortdir)
                    {
                        case "DESC":
                            mailSendList = mailSendList
                                 .OrderByDescending(r => r.PersonName)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            mailSendList = mailSendList
                                 .OrderBy(r => r.MailSendId)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                default:
                    mailSendList = mailSendList
                        .OrderByDescending(r => r.MailSendId)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
             return mailSendList.ToList();
        }

        public MailSend GetMailSendByMailSendId(int mailSendId, string compId)
        { 
            return _MailSendRepository.FindOne(x => x.CompId == compId && x.MailSendId == mailSendId);
        }

        public bool IsMailSendExist(MailSend model)
        {
            return _MailSendRepository.Exists(x => x.CompId == PortalContext.CurrentUser.CompId && x.MailSendId != model.MailSendId && x.ReportName==model.ReportName && x.FileName==model.FileName && x.PersonName == model.PersonName);
        }

        public int EditMailSend(MailSend model)
        {
            var mailSend = _MailSendRepository.FindOne(x => x.CompId == PortalContext.CurrentUser.CompId && x.MailSendId==model.MailSendId);
            mailSend.ReportName = model.ReportName;
            mailSend.MailSubject = model.MailSubject;
            mailSend.MailBody = model.MailBody;
            mailSend.MailAddress = model.MailAddress;
            mailSend.PersonName = model.PersonName;
            return _MailSendRepository.Edit(mailSend);
        }

        public int SaveMailSend(MailSend model)
        {
            model.CompId = PortalContext.CurrentUser.CompId;
            model.IsActive = true;
            return _MailSendRepository.Save(model);
        }

        public int DeleteMailSend(int mailSendId)
        {
            return
                _MailSendRepository.Delete(
                    x => x.CompId == PortalContext.CurrentUser.CompId && x.MailSendId == mailSendId);
        }
    }
 }

