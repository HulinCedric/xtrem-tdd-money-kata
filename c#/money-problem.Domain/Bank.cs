using System.Collections.Immutable;

namespace money_problem.Domain
{
    public sealed class Bank
    {
        private readonly IReadOnlyDictionary<string, double> exchangeRates;

        private Bank(Dictionary<string, double> exchangeRates)
            => this.exchangeRates = exchangeRates.ToImmutableDictionary();

        public static Bank WithExchangeRate(Currency from, Currency to, double rate)
            => new Bank(new Dictionary<string, double>())
                .AddExchangeRate(from, to, rate);

        public Bank AddExchangeRate(Currency from, Currency to, double rate)
        {
            var newExchangeRates = new Dictionary<string, double>(exchangeRates)
            {
                [KeyFor(from, to)] = rate
            };

            return new Bank(newExchangeRates);
        }

        private static string KeyFor(Currency from, Currency to)
            => $"{from}->{to}";

        public Money Convert(Money from, Currency toCurrency)
            => CanConvert(from.Currency, toCurrency)
                   ? ConvertSafely(from, toCurrency)
                   : throw new MissingExchangeRateException(from.Currency, toCurrency);

        private Money ConvertSafely(Money from, Currency toCurrency)
        {
            if (toCurrency == from.Currency)
                return from;

            var exchangeRate = exchangeRates[KeyFor(from.Currency, toCurrency)];
            var toAmount = from.Amount * exchangeRate;

            return new Money(toAmount, toCurrency);
        }

        private bool CanConvert(Currency from, Currency to)
            => from == to || exchangeRates.ContainsKey(KeyFor(from, to));
    }
}