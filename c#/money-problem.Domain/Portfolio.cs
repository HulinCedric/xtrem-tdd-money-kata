namespace money_problem.Domain;

public class Portfolio
{
    private readonly List<Money> moneys;

    public Portfolio()
        => moneys = new List<Money>();

    public void Add(Money money)
        => moneys.Add(money);

    public double Evaluate(Bank bank, Currency currency)
    {
        var totalAmount = 0d;
        var missingExchangeRates = new List<MissingExchangeRateException>();

        foreach (var (moneyAmount, moneyCurrency) in moneys)
            try
            {
                var convertedAmount = bank.Convert(moneyAmount, moneyCurrency, currency);

                totalAmount += convertedAmount;
            }
            catch (MissingExchangeRateException missingExchangeRate)
            {
                missingExchangeRates.Add(missingExchangeRate);
            }

        if (missingExchangeRates.Any())
            throw new MissingExchangeRatesException(missingExchangeRates);

        return totalAmount;
    }
}