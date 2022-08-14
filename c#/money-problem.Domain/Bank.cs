using LanguageExt;

namespace money_problem.Domain
{
    public sealed class Bank
    {
        private readonly Map<string, double> exchangeRates;

        private Bank(Map<string, double> exchangeRates)
            => this.exchangeRates = exchangeRates;

        public static Bank WithExchangeRate(Currency from, Currency to, double rate)
            => new Bank(Map<string, double>.Empty)
                .AddExchangeRate(from, to, rate);

        public Bank AddExchangeRate(Currency from, Currency to, double rate)
        {
            var newExchangeRates = exchangeRates
                .AddOrUpdate(KeyFor(from, to), rate);

            return new Bank(newExchangeRates);
        }

        private static string KeyFor(Currency from, Currency to)
            => $"{from}->{to}";

        public Either<string, Money> Convert(Money from, Currency toCurrency)
        {
            if (toCurrency == from.Currency)
                return Either<string, Money>.Right(from);

            var conversionName = KeyFor(from.Currency, toCurrency);

            return exchangeRates
                .Find(conversionName)
                .Map(exchangeRate => new Money(from.Amount * exchangeRate, toCurrency))
                .ToEither(defaultLeftValue: conversionName);
        }
    }
}