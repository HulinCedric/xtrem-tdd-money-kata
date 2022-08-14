using System.Collections.Immutable;

namespace money_problem.Domain;

public class ConversionResults
{
    private readonly IReadOnlyList<ConversionResult> results;
    private readonly Currency toCurrency;

    internal ConversionResults(IEnumerable<ConversionResult> results, Currency toCurrency)
    {
        this.toCurrency = toCurrency;
        this.results = results.ToImmutableList();
    }

    public string Error
        => $"Missing exchange rate(s): {MissingExchangeRates}";

    private string MissingExchangeRates
        => GetMissingExchangeRates(results.Where(result => result.IsFailure).Select(failure => failure.Error).ToList());

    public Money Money
        => new(
            results
                .Where(result => result.IsSuccess)
                .Sum(result => result.Money.Amount),
            toCurrency);

    private static string GetMissingExchangeRates(IEnumerable<MissingExchangeRateException> missingExchangeRates)
        => missingExchangeRates
            .Select(missingExchangeRate => $"[{missingExchangeRate.Message}]")
            .Aggregate((ratesMessage, rateMessage) => $"{ratesMessage},{rateMessage}");
}