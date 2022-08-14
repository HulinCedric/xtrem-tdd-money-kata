using LanguageExt;

namespace money_problem.Domain;

public class Portfolio
{
    private readonly Seq<Money> moneys;

    private Portfolio() : this(Seq<Money>.Empty)
    {
    }

    private Portfolio(Seq<Money> moneys)
        => this.moneys = moneys;

    public static Portfolio Empty
        => new();

    public static Portfolio WithMoneys(params Money[] moneys)
        => new(moneys.ToSeq());

    public Portfolio Add(Money money)
        => new(moneys.Add(money));

    public Either<string, Money> Evaluate(Bank bank, Currency currency)
    {
        var conversionResult = ConvertMoneys(bank, currency);
        return ContainsFailure(conversionResult)
                   ? Either<string, Money>.Left(ToFailure(conversionResult))
                   : Either<string, Money>.Right(ToSuccess(conversionResult, currency));
    }

    private List<Either<string, Money>> ConvertMoneys(Bank bank, Currency currency)
        => moneys
            .Select(money => bank.Convert(money, currency))
            .ToList();

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