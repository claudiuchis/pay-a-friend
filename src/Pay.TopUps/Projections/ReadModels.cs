using Eventuous.Projections.MongoDB.Tools;

namespace Pay.TopUps.Projections
{
    public static class ReadModels
    {
        public record TopUpDetails(string TopUpId, decimal Amount, string CurrencyCode, string TransactionId, string Outcome, string FailReason) : ProjectedDocument(TopUpId);
    }

    public class TopUpOutcome
    {
        private TopUpOutcome(string value) => Value = value;
        public string Value { get; private set; }
        public static TopUpOutcome Success { get { return new TopUpOutcome("Success"); }}
        public static TopUpOutcome Fail { get { return new TopUpOutcome("Fail"); }}
        public static implicit operator string(TopUpOutcome self) => self.Value;
    }
}