using System;
using Eventuous;

using static Pay.Prepaid.Domain.PrepaidAccounts.Events;
using Pay.Prepaid.Domain.Shared;

namespace Pay.Prepaid.Domain.PrepaidAccounts
{
    public class PrepaidAccount: Aggregate<PrepaidAccountState, PrepaidAccountId>
    {
        public void CreatePrepaidAccount(
            PrepaidAccountId accountId, 
            CustomerId customerId, 
            Currency currency)
        {
            Apply(new V1.PrepaidAccountCreated(
                accountId,
                customerId,
                currency
            ));
        }
        public void CreditAccount(
            PrepaidAccountId accountId,
            Funds amount)
        {
            Apply(new V1.PrepaidAccountCredited(
                accountId,
                amount.Amount,
                amount.Currency
            ));
        }
        public void DebitAccount(
            PrepaidAccountId accountId,
            Funds amount)
        {
            if (State.Balance < amount )
                throw new InsufficientFundsException("Insufficient funds in the account.");

            Apply(new V1.PrepaidAccountDebited(
                accountId,
                amount.Amount,
                amount.Currency
            ));
        }

    }

    public record PrepaidAccountState : AggregateState<PrepaidAccountState, PrepaidAccountId>
    {
        public enum AccountStatus {
            Active = 0
        }
        public CustomerId CustomerId { get; init; }
        public Currency Currency { get; init; }
        public Money Balance { get; init; }
        public AccountStatus Status { get; init; }
        public override PrepaidAccountState When(object @event)
            => @event switch {
                V1.PrepaidAccountCreated created => this with {
                    Id = new PrepaidAccountId(created.PrepaidAccountId),
                    CustomerId = new CustomerId(created.CustomerId),
                    Currency = new Currency { CurrencyCode = created.CurrencyCode},
                    Balance = new Funds(0, created.CurrencyCode),
                    Status = AccountStatus.Active
                },
                V1.PrepaidAccountCredited credited => this with {
                    Balance = Balance + new Funds(credited.Amount, credited.CurrencyCode)
                },
                V1.PrepaidAccountDebited debited => this with {
                    Balance = Balance - new Funds(debited.Amount, debited.CurrencyCode)
                },
                _ => this
            };
    }
}
