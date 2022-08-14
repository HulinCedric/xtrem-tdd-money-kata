using System.Collections.Immutable;

namespace money_problem.Domain;

internal class ConversionResults
{
    private readonly IReadOnlyList<ConversionResult> results;
    private readonly Currency toCurrency;

    public ConversionResults(IEnumerable<ConversionResult> results, Currency toCurrency)
    {
        this.toCurrency = toCurrency;
        this.results = results.ToImmutableList();
    }

    public MissingExchangeRatesException Error
        => new(
            results
                .Where(result => result.IsFailure)
                .Select(failure => failure.Error)
                .ToList());

    public bool IsFailure
        => results.Any(result => result.IsFailure);

    public Money Money
        => new(
            results
                .Where(result => result.IsSuccess)
                .Sum(result => result.Money.Amount),
            toCurrency);
}