using System;
using System.Transactions;

namespace Dorkari.Helpers.Data.Transactions
{
    public class TransactionManager : ITransactionManager
    {
        public void Transact(Action action, Action onRollback = null)
        {
            using (TransactionScope transaction = new TransactionScope(
                                                    TransactionScopeOption.Required, //Default is Required anyway
                                                    new TransactionOptions
                                                    {
                                                        IsolationLevel = IsolationLevel.ReadCommitted,
                                                        Timeout = new TimeSpan(0, 15, 0)
                                                    }))
            {
                try
                {
                    action.Invoke();
                    transaction.Complete();
                }
                catch (Exception transactionException)
                {
                    if (onRollback != null) //call method when errored
                    {
                        transaction.Dispose(); //rolling back origincal transaction
                        try
                        {
                            onRollback.Invoke(); //if this fails, system will throw this exception rather than the original!
                        }
                        catch (Exception rollbackActionException)
                        {
                            //log and supress rollback action exception to allow throw of original transaction exception with stack
                        }
                    }
                    throw; //transaction will be rolled back (if not done already). throw original error!
                }
            }
        }

        public T Transact<T>(Func<T> func, T defaultvalue = default(T), Action onRollback = null)
        {
            T result = defaultvalue;
            Transact(() => { result = func(); }, onRollback);
            return result;
        }
    }
}
