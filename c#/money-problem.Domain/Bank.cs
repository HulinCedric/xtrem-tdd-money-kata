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

        public double Convert(Money from, Currency to)
            => CanConvert(from.Currency, to)
                   ? ConvertSafely(from, to)
                   : throw new MissingExchangeRateException(from.Currency, to);

        public double Convert(double amount, Currency from, Currency to)
            => Convert(new Money(amount, from), to);

        private double ConvertSafely(Money from, Currency to)
            => to == from.Currency
                   ? from.Amount
                   : from.Amount * exchangeRates[KeyFor(from.Currency, to)];

        private bool CanConvert(Currency from, Currency to)
            => from == to || exchangeRates.ContainsKey(KeyFor(from, to));
    }
}