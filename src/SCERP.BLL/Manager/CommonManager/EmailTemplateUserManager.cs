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
    public class EmailTemplateUserManager : IEmailTemplateUserManager
    {
        private readonly IRepository<EmailTemplateUser> _emailTmpalteUseRepository;

        public EmailTemplateUserManager(IRepository<EmailTemplateUser> emailTmpalteUseRepository)
        {
            _emailTmpalteUseRepository = emailTmpalteUseRepository;
        }

        public List<EmailTemplateUser> GetEmailTemplateUserUsers(int pageIndex, string sort, string sortdir, string searchString, out int totalRecords)
        {
            var index = pageIndex;
            var pageSize = AppConfig.PageSize;
            var emailUsers =
                _emailTmpalteUseRepository.GetWithInclude(x => x.EmailTemplate.EmailTemplateName.Trim().Contains(searchString) || String.IsNullOrEmpty(searchString)
                                                 || x.EmailUser.EmailUserName.Contains(searchString) || String.IsNullOrEmpty(searchString), "EmailTemplate", "EmailUser");
            totalRecords = emailUsers.Count();
            switch (sort)
            {
                case "EmailTemplate.EmailTemplateName":
                    switch (sortdir)
                    {
                        case "DESC":
                            emailUsers = emailUsers
                                .OrderByDescending(r => r.EmailTemplate.EmailTemplateName)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            emailUsers = emailUsers
                                .OrderBy(r => r.EmailTemplate.EmailTemplateName)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                default:
                    emailUsers = emailUsers
                        .OrderByDescending(r => r.EmailTamplateUserId)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
            return emailUsers.ToList();
        }

        public EmailTemplateUser GetEmailTemplateUseById(int emailTamplateUserId)
        {
            return _emailTmpalteUseRepository.FindOne(x => x.EmailTamplateUserId == emailTamplateUserId);
        }

        public int EditEmailTemplateUser(EmailTemplateUser model)
        {
            EmailTemplateUser emailTemplateUser = _emailTmpalteUseRepository.FindOne(x => x.EmailTamplateUserId == model.EmailTamplateUserId);
            emailTemplateUser.EmailUserId = model.EmailUserId;
            emailTemplateUser.EailTamplateId = model.EailTamplateId;
            emailTemplateUser.SendingType = model.SendingType;
            return _emailTmpalteUseRepository.Edit(emailTemplateUser);
        }

        public int SaveEmailTamplateUser(EmailTemplateUser emailTemplateUser)
        {
            if (emailTemplateUser != null)
            {
                return _emailTmpalteUseRepository.Save(emailTemplateUser);
            }
            throw new Exception(message: "Email Template User Not Saved");
        }

        public List<string> GetEmailTemplateUsersPhoneNumbers(string templateRefId, string compId)
        {
            IQueryable<EmailTemplateUser> templateUsers = _emailTmpalteUseRepository.GetWithInclude(x => x.CompId == compId, "EmailTemplate", "EmailUser");
            var  phonList = templateUsers.Where(x => x.EmailTemplate.EmailTemplateRefId == templateRefId).Select(x => x.EmailUser.Phone).ToList();
            return phonList;
        }
    }
}
