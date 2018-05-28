using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

public class TransactionExtensions
{

}

public static class Transaction
{    

    public static void Scope(Func<bool> action)
    {
        using (TransactionScope transaction = Default)
        {
            transaction.Complete();
        }
    }


    /// <summary>
    /// TransactionScopeOption = Required, IsolationLevel = RepeatableRead
    /// </summary>
    public static TransactionScope Default
    {
        get
        {
            return new TransactionScope(TransactionScopeOption.Required,
            new TransactionOptions
            {
                IsolationLevel = IsolationLevel.RepeatableRead
            });
        }
    }
}
