using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Service.Data.Core
{
    public class FormattedDbEntityValidationException:Exception
    {

        public FormattedDbEntityValidationException(DbEntityValidationException innerException) :
            base(null, innerException)
        {
        }

        public override string Message
        {
            get
            {
                var innerException = InnerException as DbEntityValidationException;
                if (innerException != null)
                {
                    var sb = new StringBuilder();

                    sb.AppendLine();
                    sb.AppendLine();
                    foreach (var eve in innerException.EntityValidationErrors)
                    {
                        sb.AppendLine(string.Format("- Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().FullName, eve.Entry.State));
                        //If entity is Detached then eve.Entry.CurrentValues threw exception
                        if (eve.Entry.State != EntityState.Detached &&
                            eve.Entry.State != EntityState.Unchanged)
                        {
                            foreach (var ve in eve.ValidationErrors)
                            {
                                sb.AppendLine(string.Format("-- Property: \"{0}\", Value: \"{1}\", Error: \"{2}\"",
                                   ve.PropertyName,
                                   eve.Entry.CurrentValues.GetValue<object>(ve.PropertyName),
                                   ve.ErrorMessage));

                            }
                        }
                        else
                        {
                            sb.AppendLine("Error is in Entity Validation.Please see the last error for more details.");
                        }
                    }
                    sb.AppendLine();

                    return sb.ToString();
                }

                return base.Message;
            }
        }
    }
}
