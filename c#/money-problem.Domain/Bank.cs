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
            => CanConvert(from.Currency, toCurrency) ?
                   Either<string, Money>.Right(ConvertSafely(from, toCurrency)) :
                   Either<string, Money>.Left(KeyFor(from.Currency, toCurrency));

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