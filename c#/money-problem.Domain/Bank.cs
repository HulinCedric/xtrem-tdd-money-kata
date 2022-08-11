namespace money_problem.Domain
{
    public sealed class Bank
    {
        private readonly Dictionary<string, double> exchangeRates;

        private Bank(Dictionary<string, double> exchangeRates)
            => this.exchangeRates = exchangeRates;

        public static Bank WithExchangeRate(Currency from, Currency to, double rate)
        {
            var bank = new Bank(new Dictionary<string, double>());
            bank.AddExchangeRate(from, to, rate);

            return bank;
        }

        public void AddExchangeRate(Currency from, Currency to, double rate)
            => exchangeRates[KeyFor(from, to)] = rate;

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