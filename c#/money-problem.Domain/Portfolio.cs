using System.Collections.Immutable;

namespace money_problem.Domain;

public class Portfolio
{
    private readonly IReadOnlyList<Money> moneys;

    private Portfolio() : this(new List<Money>())
    {
    }

    private Portfolio(IEnumerable<Money> moneys)
        => this.moneys = moneys.ToImmutableList();

    public static Portfolio Empty
        => new();

    public static Portfolio WithMoneys(params Money[] moneys)
        => new(moneys);

    public Portfolio Add(Money money)
    {
        var newMoneys = new List<Money>(moneys) { money };
        return new Portfolio(newMoneys);
    }

    public ConversionResults Evaluate(Bank bank, Currency currency)
        => ConvertMoneys(bank, currency);

    public Money EvaluateWithException(Bank bank, Currency currency)
    {
        var results = Evaluate(bank, currency);
        return results.IsFailure
                   ? throw results.Exception
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