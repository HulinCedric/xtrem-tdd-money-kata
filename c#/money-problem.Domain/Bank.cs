using LanguageExt;

namespace money_problem.Domain
{
    public sealed class Bank
    {
        private readonly Seq<ExchangeRate> exchangeRates;

        private Bank(Seq<ExchangeRate> exchangeRates)
            => this.exchangeRates = exchangeRates;

        public static Bank WithExchangeRate(ExchangeRate exchangeRate)
            => new Bank(Seq<ExchangeRate>.Empty).AddExchangeRate(exchangeRate);

        public static Bank WithExchangeRates(params ExchangeRate[] exchangeRates)
            => new(exchangeRates.ToSeq());

        public Bank AddExchangeRate(ExchangeRate exchangeRate)
        {
            var newExchangeRates = exchangeRates
                .Filter(er => er.IsNotSameExchange(exchangeRate))
                .Add(exchangeRate);

            return new Bank(newExchangeRates);
        }

        public Either<string, Money> Convert(Money from, Currency toCurrency)
        {
            if (toCurrency == from.Currency)
                return from;

            var conversionExchangeRate = ExchangeRate.Default(from.Currency, toCurrency);

            return exchangeRates
                .Find(exchangeRate => exchangeRate.IsSameExchange(conversionExchangeRate))
                .Map(exchangeRate => new Money(from.Amount * exchangeRate.Rate, toCurrency))
                .ToEither(conversionExchangeRate.ExchangeName);
        }
    }
}