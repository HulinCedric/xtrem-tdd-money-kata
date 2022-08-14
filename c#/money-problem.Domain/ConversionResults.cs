using System.Collections.Immutable;
using LanguageExt;

namespace money_problem.Domain;

internal class ConversionResults
{
    private readonly IReadOnlyList<Either<string, Money>> results;
    private readonly Currency toCurrency;

    internal ConversionResults(IEnumerable<Either<string, Money>> results, Currency toCurrency)
    {
        this.toCurrency = toCurrency;
        this.results = results.ToImmutableList();
    }

    internal string Error
        => $"Missing exchange rate(s): {MissingExchangeRates}";

    internal bool IsFailure
        => results.Lefts().Any();

    private string MissingExchangeRates
        => GetMissingExchangeRates(results.Lefts().ToList());

    internal Money Money
        => new(
            results
                .Rights()
                .Sum(money => money.Amount),
            toCurrency);

    private static string GetMissingExchangeRates(IEnumerable<string> missingExchangeRates)
        => missingExchangeRates
            .Select(missingExchangeRate => $"[{missingExchangeRate}]")
            .Aggregate((ratesMessage, rateMessage) => $"{ratesMessage},{rateMessage}");
}