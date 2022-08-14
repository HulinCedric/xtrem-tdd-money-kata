using System.Collections.Immutable;

namespace money_problem.Domain;

internal class ConversionResults
{
    private readonly IReadOnlyList<ConversionResult> results;
    private readonly Currency toCurrency;

    internal ConversionResults(IEnumerable<ConversionResult> results, Currency toCurrency)
    {
        this.toCurrency = toCurrency;
        this.results = results.ToImmutableList();
    }

    internal string Error
        => $"Missing exchange rate(s): {MissingExchangeRates}";

    internal bool IsFailure
        => results.Any(r => r.IsFailure);

    private string MissingExchangeRates
        => GetMissingExchangeRates(
            results
                .Where(result => result.IsFailure)
                .Select(failure => failure.Error)
                .ToList());

    internal Money Money
        => new(
            results
                .Where(result => result.IsSuccess)
                .Sum(result => result.Money.Amount),
            toCurrency);

    private static string GetMissingExchangeRates(IEnumerable<string> missingExchangeRates)
        => missingExchangeRates
            .Select(missingExchangeRate => $"[{missingExchangeRate}]")
            .Aggregate((ratesMessage, rateMessage) => $"{ratesMessage},{rateMessage}");
}