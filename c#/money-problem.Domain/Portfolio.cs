namespace money_problem.Domain;

public class Portfolio
{
    private readonly Dictionary<Currency, double> moneys;

    public Portfolio()
        => moneys = new Dictionary<Currency, double>();

    public void Add(Money money)
        => Add(money.Amount, money.Currency);

    public void Add(double amount, Currency currency)
    {
        if (!moneys.ContainsKey(currency))
            moneys.Add(currency, 0);

        moneys[currency] += amount;
    }

    public double Evaluate(Bank bank, Currency currency)
    {
        var totalAmount = 0d;
        var missingExchangeRates = new List<MissingExchangeRateException>();

        foreach (var (moneyCurrency, moneyAmount) in moneys)
        {
            try
            {
                var convertedAmount = bank.Convert(moneyAmount, moneyCurrency, currency);

                totalAmount += convertedAmount;
            }
            catch (MissingExchangeRateException missingExchangeRate)
            {
                missingExchangeRates.Add(missingExchangeRate);
            }
        }

        if (missingExchangeRates.Any())
            throw new MissingExchangeRatesException(missingExchangeRates);

        return totalAmount;
    }
}