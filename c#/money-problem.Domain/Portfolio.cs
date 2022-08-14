using System.Collections.Immutable;
using LanguageExt;

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

    private List<Either<string, Money>> ConvertMoneys(Bank bank, Currency currency)
        => moneys
            .Select(money => bank.Convert(money, currency))
            .ToList();

    public Either<string, Money> Evaluate(Bank bank, Currency currency)
    {
        var conversionResult = ConvertMoneys(bank, currency);
        return ContainsFailure(conversionResult)
                   ? Either<string, Money>.Left(ToFailure(conversionResult))
                   : Either<string, Money>.Right(ToSuccess(conversionResult, currency));
    }

    private static bool ContainsFailure(IEnumerable<Either<string, Money>> results)
        => results.Any(result => result.IsLeft);

    private static string GetMissingExchangeRates(IEnumerable<string> missingExchangeRates)
        => missingExchangeRates
            .Select(missingExchangeRate => $"[{missingExchangeRate}]")
            .Aggregate((ratesMessage, rateMessage) => $"{ratesMessage},{rateMessage}");

    private static string ToFailure(IEnumerable<Either<string, Money>> results)
        => $"Missing exchange rate(s): {GetMissingExchangeRates(results.Lefts())}";

    private static Money ToSuccess(IEnumerable<Either<string, Money>> results, Currency toCurrency)
        => new(
            results.Rights().Sum(money => money.Amount),
            toCurrency);
}