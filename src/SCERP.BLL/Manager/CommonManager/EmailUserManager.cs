using System;
using System.Collections.Generic;
using System.Linq;
using SCERP.BLL.IManager.ICommonManager;
using SCERP.Common;
using SCERP.DAL.IRepository;
using SCERP.Model.CommonModel;

namespace SCERP.BLL.Manager.CommonManager
{
    public class EmailUserManager : IEmailUserManager
    {
        private readonly IRepository<EmailUser> _emailUserRepository;
        public EmailUserManager(IRepository<EmailUser> emailUserRepository)
        {
            _emailUserRepository = emailUserRepository;
        }

        public List<EmailUser> GetEmailUsers(int pageIndex, string sort, string sortdir, string searchString, out int totalRecords)
        {
            var index = pageIndex;
            var pageSize = AppConfig.PageSize;
            var emailUsers =
                _emailUserRepository.Filter(x => x.EmailUserName.Trim().Contains(searchString) || String.IsNullOrEmpty(searchString)
                                            || x.EmailUserRefId.Contains(searchString) || String.IsNullOrEmpty(searchString));
            totalRecords = emailUsers.Count();
            switch (sort)
            {
                case "EmailUserName":
                    switch (sortdir)
                    {
                        case "DESC":
                            emailUsers = emailUsers
                                .OrderByDescending(r => r.EmailUserName)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            emailUsers = emailUsers
                                .OrderBy(r => r.EmailUserName)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                default:
                    emailUsers = emailUsers
                        .OrderByDescending(r => r.EmailUserId)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
            return emailUsers.ToList();

        }

        public EmailUser GetEmailUserById(int emailUserId)
        {
            return _emailUserRepository.FindOne(x => x.EmailUserId == emailUserId);
        }

        public string GetNewRefId(string compId)
        {
            var refId = _emailUserRepository.Filter(x => x.CompId == compId).Max(x => x.EmailUserRefId) ?? "0";
            return refId.IncrementOne().PadZero(3);
          
        }

        public int EditEmailUser(EmailUser emailUser)
        {
            EmailUser mailUser = _emailUserRepository.FindOne(x => x.EmailUserId == emailUser.EmailUserId);
            mailUser.EmailUserName = emailUser.EmailUserName;
            mailUser.EmailAddress = emailUser.EmailAddress;
            mailUser.Phone = emailUser.Phone;
            return _emailUserRepository.Edit(mailUser);
        }

        public int SaveEmailUser(EmailUser emailUser)
        {
            if (emailUser!=null)
            {
               return _emailUserRepository.Save(emailUser);
            }
            throw new Exception(message:"Email User Not Saved");
          
        }

        public List<EmailUser> GetAllEmailUsers(string compId)
        {
            return _emailUserRepository.Filter(x => x.CompId == compId).ToList();
        }
    }
}
