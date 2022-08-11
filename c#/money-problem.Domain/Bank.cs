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

        public double Convert(Money money, Currency to)
            => Convert(money.Amount, money.Currency, to);

        public double Convert(double amount, Currency from, Currency to)
            => CanConvert(from, to)
                   ? ConvertSafely(amount, from, to)
                   : throw new MissingExchangeRateException(from, to);

        private double ConvertSafely(double amount, Currency from, Currency to)
            => to == from
                   ? amount
                   : amount * exchangeRates[KeyFor(from, to)];

        private bool CanConvert(Currency from, Currency to)
            => from == to || exchangeRates.ContainsKey(KeyFor(from, to));
    }
}