namespace money_problem.Domain;

public class Portfolio
{
    private readonly List<Money> moneys;

    public Portfolio()
        => moneys = new List<Money>();

    public void Add(Money money)
        => moneys.Add(money);

    public Money Evaluate(Bank bank, Currency currency)
    {
        var results = ConvertMoneys(bank, currency);
        return results.IsFailure
                   ? throw results.Error
                   : results.Money;
    }

    private ConversionResults ConvertMoneys(Bank bank, Currency currency)
        => new(
            moneys
                .Select(money => Convert(bank, currency, money)),
            currency);

    private static ConversionResult Convert(Bank bank, Currency currency, Money money)
    {
        try
        {
            var convertedMoney = bank.Convert(money, currency);

            return ConversionResult.Success(convertedMoney);
        }
        catch (MissingExchangeRateException missingExchangeRate)
        {
            return ConversionResult.Failure(missingExchangeRate);
        }
    }
}