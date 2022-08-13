namespace money_problem.Domain;

internal class ConversionResults
{
    private readonly List<ConversionResult> results;
    private readonly Currency toCurrency;

    public ConversionResults(IEnumerable<ConversionResult> results, Currency toCurrency)
    {
        this.toCurrency = toCurrency;
        this.results = results.ToList();
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
                .Select(result => result.Money)
                .Select(money => money.Amount)
                .Sum(),
            toCurrency);
}