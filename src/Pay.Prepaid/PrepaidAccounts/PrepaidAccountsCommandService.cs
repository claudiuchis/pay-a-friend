using Eventuous;

using Pay.Prepaid.Domain.Shared;
using Pay.Prepaid.Domain.PrepaidAccounts;
using Pay.Prepaid.Domain.PrepaidTransactions;
using static Pay.Prepaid.PrepaidAccounts.Commands;

namespace Pay.Prepaid.PrepaidAccounts
{
    public class PrepaidAccountsCommandService 
        : ApplicationService<PrepaidAccount, PrepaidAccountState, PrepaidAccountId>
    {
        public PrepaidAccountsCommandService(
            IAggregateStore store,
            ICurrencyLookup currencyLookup
        ) : base (store)
        {
            OnNew<V1.CreatePrepaidAccount>(
                cmd => new PrepaidAccountId(cmd.PrepaidAccountId),
                (prepaidAccount, cmd)
                    => prepaidAccount.CreatePrepaidAccount(
                        new PrepaidAccountId(cmd.PrepaidAccountId),
                        new CustomerId(cmd.CustomerId),
                        cmd.CountryCode,
                        currencyLookup)
            );

            OnExisting<V1.CreditPrepaidAccount>(
                cmd => new PrepaidAccountId(cmd.PrepaidAccountId),
                (prepaidAccount, cmd)
                    => prepaidAccount.CreditAccount(
                        new PrepaidTransaction(
                            new PrepaidTransactionId(cmd.TransactionId),
                            PrepaidTransactionType.FromString(cmd.TransactionType)
                        ),
                        Funds.FromDecimal(cmd.Amount, cmd.CurrencyCode, currencyLookup)
                    )
            );

            OnExisting<V1.PlaceHoldOnPrepaidAccount>(
                cmd => new PrepaidAccountId(cmd.PrepaidAccountId),
                (prepaidAccount, cmd)
                    => prepaidAccount.PlaceHold(
                        new PrepaidTransaction(
                            new PrepaidTransactionId(cmd.TransactionId),
                            PrepaidTransactionType.FromString(cmd.TransactionType)
                        ),
                        Funds.FromDecimal(cmd.Amount, cmd.CurrencyCode, currencyLookup)
                    )
            );

            OnExisting<V1.ReleaseHoldOnPrepaidAccount>(
                cmd => new PrepaidAccountId(cmd.PrepaidAccountId),
                (prepaidAccount, cmd)
                    => prepaidAccount.ReleaseHold(
                        new PrepaidTransactionId(cmd.TransactionId),
                        cmd.Reason
                    )
            );


            OnExisting<V1.DebitPrepaidAccount>(
                cmd => new PrepaidAccountId(cmd.PrepaidAccountId),
                (prepaidAccount, cmd)
                    => prepaidAccount.DebitAccount(
                        new PrepaidTransaction(
                            new PrepaidTransactionId(cmd.TransactionId),
                            PrepaidTransactionType.FromString(cmd.TransactionType)
                        ),
                        Funds.FromDecimal(cmd.Amount, cmd.CurrencyCode, currencyLookup)
                    )
            );
        }
    }
}