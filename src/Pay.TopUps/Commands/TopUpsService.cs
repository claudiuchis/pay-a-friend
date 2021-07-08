using System;
using Eventuous;
using Pay.TopUps.Domain;
using static Pay.TopUps.Commands.Commands;

namespace Pay.TopUps.Commands
{
    public class TopUpsService : ApplicationService<TopUp, TopUpState, TopUpId>
    {
        public TopUpsService(
            IAggregateStore store,
            ICurrencyLookup currencyLookup,
            IPaymentService paymentService
        ) : base(store)
        {
            OnNew<V1.SubmitTopUp>(
                cmd => new TopUpId(cmd.TopUpId),
                (topUp, cmd)
                    => topUp.SubmitTopUp(
                        paymentService,
                        new TopUpId(cmd.TopUpId), 
                        new CardDetails(
                            cmd.Name,
                            cmd.Number,
                            cmd.ExpMonth,
                            cmd.ExpYear,
                            cmd.Cvc
                        ),
                        new BillingDetails(
                            cmd.AddressCity,
                            cmd.AddressCountry,
                            cmd.AddressLine1,
                            cmd.AddressLine2,
                            cmd.AddressState,
                            cmd.AddressZip
                        ),
                        TopUpAmount.FromDecimal(
                            cmd.Amount,
                            cmd.CurrencyCode,
                            currencyLookup
                        )
                    )
            );
        }
    }
}
