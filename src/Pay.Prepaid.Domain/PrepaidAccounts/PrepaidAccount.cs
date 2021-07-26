using System;
using System.Linq;
using System.Collections.Generic;
using Eventuous;

using static Pay.Prepaid.Domain.PrepaidAccounts.Events;
using Pay.Prepaid.Domain.Shared;
using Pay.Prepaid.Domain.PrepaidTransactions;

namespace Pay.Prepaid.Domain.PrepaidAccounts
{
    public class PrepaidAccount: Aggregate<PrepaidAccountState, PrepaidAccountId>
    {
        public void CreatePrepaidAccount(
            PrepaidAccountId accountId, 
            CustomerId customerId, 
            string countryCode,
            ICurrencyLookup currencyLookup)
        {
            var currency = currencyLookup.FindCurrencyByCountry(countryCode);
            
            Apply(new V1.PrepaidAccountCreated(
                accountId,
                customerId,
                currency
            ));
        }
        public void CreditAccount(
            PrepaidTransaction transaction,
            Funds amount)
        {
            Apply(new V1.PrepaidAccountCredited(
                this.GetId(),
                amount.Amount,
                amount.Currency,
                transaction.TransactionType,
                transaction.TransactionId.TransactionId
            ));
        }

        public void PlaceHold(
            PrepaidTransaction transaction,
            Funds amount
        )
        {
            if (State.Available < amount )
                throw new InsufficientFundsException("Insufficient funds in the account.");

            Apply(new V1.PrepaidAccountHoldPlaced(
                this.GetId(),
                amount.Amount,
                amount.Currency,
                transaction.TransactionType,
                transaction.TransactionId.TransactionId
            ));
        }

        public void ReleaseHold(
            PrepaidTransactionId transactionId,
            string reason
        )
        {
            var hold = State.MoneyHolds.Where( hold => hold.TransferOrderId == transactionId).FirstOrDefault();
            if (hold == null)
                throw new ArgumentException("there is no hold for this money on this account");

            Apply(new V1.PrepaidAccountHoldReleased(
                this.GetId(),
                transactionId,
                hold.Amount,
                hold.CurrencyCode,
                reason
            ));
        }

        public void DebitAccount(
            PrepaidTransaction transaction,
            Funds amount)
        {
            var hold = State.MoneyHolds.Where( hold => hold.TransferOrderId == transaction.TransactionId).FirstOrDefault();
            if (hold == null)
                throw new ArgumentException("there is no hold for this money on this account");

            Apply(new V1.PrepaidAccountDebited(
                this.GetId(),
                amount.Amount,
                amount.Currency,
                transaction.TransactionType,
                transaction.TransactionId
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
        public Money Available { get; init; }
        public AccountStatus Status { get; init; }
        public IEnumerable<MoneyHold> MoneyHolds { get; init; }
        public override PrepaidAccountState When(object @event)
            => @event switch {
                V1.PrepaidAccountCreated created => this with {
                    Id = new PrepaidAccountId(created.PrepaidAccountId),
                    CustomerId = new CustomerId(created.CustomerId),
                    Currency = new Currency { CurrencyCode = created.CurrencyCode},
                    Balance = new Funds(0, created.CurrencyCode),
                    Available = new Funds(0, created.CurrencyCode),
                    Status = AccountStatus.Active,
                    MoneyHolds = System.Array.Empty<MoneyHold>()
                },
                V1.PrepaidAccountCredited credited => this with {
                    Balance = Balance + new Funds(credited.Amount, credited.CurrencyCode)
                },
                V1.PrepaidAccountDebited debited => this with {
                    Balance = Balance - new Funds(debited.Amount, debited.CurrencyCode),
                    MoneyHolds = MoneyHolds.Where( hold => !hold.TransferOrderId.Equals(debited.TransactionId))
                },
                V1.PrepaidAccountHoldPlaced placed => this with {
                    MoneyHolds = MoneyHolds.AsEnumerable().Append(
                        new MoneyHold(placed.TransactionId, placed.Amount, placed.CurrencyCode)),
                    Available = Available - new Funds(placed.Amount, placed.CurrencyCode)
                },
                V1.PrepaidAccountHoldReleased released => this with {
                    MoneyHolds = MoneyHolds.Where( hold => !hold.TransferOrderId.Equals(released.TransactionId)),
                    Available = Available + new Funds(released.Amount, released.CurrencyCode)
                },
                _ => this
            };
    }

    public record MoneyHold(string TransferOrderId, decimal Amount, string CurrencyCode);
}
