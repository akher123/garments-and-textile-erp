using System;
using System.Transactions;

namespace SCERP.BLL.Manager
{
    public abstract class BaseManager
    {
        /// <summary>
        /// Create a new transaction boundary
        /// </summary>
        /// <returns>new TransactionScope</returns>
        protected TransactionScope CreateTransaction()
        {
            return new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(1, 0, 0));
        }
    }
}
