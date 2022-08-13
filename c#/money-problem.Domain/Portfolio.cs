using CSharpFunctionalExtensions;

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
        var conversionResults = moneys
            .Select(money => Convert(bank, currency, money))
            .ToList();

        var missingExchangeRates = conversionResults
            .Where(result => result.IsFailure)
            .Select(failure => failure.Error)
            .ToList();

        if (missingExchangeRates.Any())
            throw new MissingExchangeRatesException(missingExchangeRates);

        var totalAmount = conversionResults
            .Select(result => result.Value)
            .Select(money => money.Amount)
            .Sum();

        return new Money(totalAmount, currency);
    }

    private static Result<Money, MissingExchangeRateException> Convert(Bank bank, Currency currency, Money money)
    {
        try
        {
            var convertedMoney = bank.Convert(money, currency);

            return Result.Success<Money, MissingExchangeRateException>(convertedMoney);
        }
        catch (MissingExchangeRateException missingExchangeRate)
        {
            return Result.Failure<Money, MissingExchangeRateException>(missingExchangeRate);
        }
    }
}