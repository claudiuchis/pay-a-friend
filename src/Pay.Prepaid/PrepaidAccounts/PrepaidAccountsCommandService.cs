using Eventuous;

using Pay.Prepaid.Domain.Shared;
using Pay.Prepaid.Domain.PrepaidAccounts;
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
                        new Currency{ CurrencyCode = cmd.CurrencyCode}
                    )
            );

            OnExisting<V1.CreditPrepaidAccount>(
                cmd => new PrepaidAccountId(cmd.PrepaidAccountId),
                (prepaidAccount, cmd)
                    => prepaidAccount.CreditAccount(
                        new PrepaidAccountId(cmd.PrepaidAccountId),
                        Funds.FromDecimal(cmd.Amount, cmd.CurrencyCode, currencyLookup)
                    )
            );

            OnExisting<V1.DebitPrepaidAccount>(
                cmd => new PrepaidAccountId(cmd.PrepaidAccountId),
                (prepaidAccount, cmd)
                    => prepaidAccount.DebitAccount(
                        new PrepaidAccountId(cmd.PrepaidAccountId),
                        Funds.FromDecimal(cmd.Amount, cmd.CurrencyCode, currencyLookup)
                    )
            );
        }
    }
}