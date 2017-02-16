using System;

namespace Dorkari.Helpers.Data.Transactions
{
    public interface ITransactionManager
    {
        void Transact(Action action, Action onRollback = null);
        T Transact<T>(Func<T> func, T defaultvalue, Action onRollback = null);
    }
}
