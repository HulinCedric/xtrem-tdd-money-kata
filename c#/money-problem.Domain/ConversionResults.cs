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

    internal MissingExchangeRatesException Error
        => new(
            results
                .Where(result => result.IsFailure)
                .Select(failure => failure.Error)
                .ToList());

    internal bool IsFailure
        => results.Any(result => result.IsFailure);

    internal Money Money
        => new(
            results
                .Where(result => result.IsSuccess)
                .Sum(result => result.Money.Amount),
            toCurrency);
}