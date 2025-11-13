using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.ICommonManager;
using SCERP.Common;
using SCERP.DAL.IRepository;
using SCERP.Model.CommonModel;

namespace SCERP.BLL.Manager.CommonManager
{
    public class EmailTemplateManager : IEmailTemplateManager
    {
        private readonly IRepository<EmailTemplate> _emailTemplateRepository;

        public EmailTemplateManager(IRepository<EmailTemplate> emailRepository)
        {
            _emailTemplateRepository = emailRepository;
        }

        public List<EmailTemplate> GetEmailTemplates(int pageIndex, string sort, string sortdir, string searchString, out int totalRecords)
        {
            var index = pageIndex;
            var pageSize = AppConfig.PageSize;
            var emailUsers =
                _emailTemplateRepository.Filter(x => x.EmailTemplateName.Trim().Contains(searchString) || String.IsNullOrEmpty(searchString)
                                                 || x.EmailTemplateRefId.Contains(searchString) || String.IsNullOrEmpty(searchString));
            totalRecords = emailUsers.Count();
            switch (sort)
            {
                case "EmailUserName":
                    switch (sortdir)
                    {
                        case "DESC":
                            emailUsers = emailUsers
                                .OrderByDescending(r => r.EmailTemplateName)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            emailUsers = emailUsers
                                .OrderBy(r => r.EmailTemplateName)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                default:
                    emailUsers = emailUsers
                        .OrderByDescending(r => r.EmailTemplateId)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
            return emailUsers.ToList();

        }

        public EmailTemplate GetEmailTemplateById(int emailTemplateId)
        {
            return _emailTemplateRepository.FindOne(x => x.EmailTemplateId == emailTemplateId);
        }

        public string GetNewRefId()
        {

            var refId = _emailTemplateRepository.All().Max(x => x.EmailTemplateRefId) ?? "0";
            return refId.IncrementOne().PadZero(3);
        }

        public int EditEmailTemplate(EmailTemplate emailTemplate)
        {
            EmailTemplate model = _emailTemplateRepository.FindOne(x => x.EmailTemplateId == emailTemplate.EmailTemplateId);
            model.EmailTemplateName = emailTemplate.EmailTemplateName;

            return _emailTemplateRepository.Edit(model);
        }

        public int SaveEmailTemplate(EmailTemplate emailTemplate)
        {

            if (emailTemplate != null)
            {
                return _emailTemplateRepository.Save(emailTemplate);
            }
            throw new Exception(message: "Email Template Not Saved");
        }

        public List<EmailTemplate> GetAllEmailTemplates()
        {
            return _emailTemplateRepository.All().ToList();
        }
    }
}
