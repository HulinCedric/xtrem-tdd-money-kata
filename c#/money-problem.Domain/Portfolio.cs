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
        var conversionResults = ConvertMoneys(bank, currency);
        if (conversionResults.IsFailure())
            throw conversionResults.Error();
        return conversionResults.Money();
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

internal class ConversionResults
{
    private readonly List<ConversionResult> results;
    private readonly Currency toCurrency;

    public ConversionResults(IEnumerable<ConversionResult> results, Currency toCurrency)
    {
        this.toCurrency = toCurrency;
        this.results = results.ToList();
    }

    public Money Money()
        => new(
            results
                .Select(result => result.Money)
                .Select(money => money.Amount)
                .Sum(),
            toCurrency);

    public bool IsFailure()
        => results.Any(result => result.IsFailure);

    public MissingExchangeRatesException Error()
        => new(
            results
                .Where(result => result.IsFailure)
                .Select(failure => failure.Error)
                .ToList());
}

public readonly struct ConversionResult
{
    private readonly Result<Money, MissingExchangeRateException> result;

    private ConversionResult(Money money)
        => result = Result.Success<Money, MissingExchangeRateException>(money);

    private ConversionResult(MissingExchangeRateException exception)
        => result = Result.Failure<Money, MissingExchangeRateException>(exception);

    public MissingExchangeRateException Error
        => result.Error;

    public bool IsFailure
        => result.IsFailure;

    public Money Money
        => result.Value;

    public static ConversionResult Success(Money money)
        => new(money);

    public static ConversionResult Failure(MissingExchangeRateException exception)
        => new(exception);
}