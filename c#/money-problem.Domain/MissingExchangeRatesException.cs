namespace money_problem.Domain;

public sealed class MissingExchangeRatesException : Exception
{
    public MissingExchangeRatesException(IEnumerable<MissingExchangeRateException> missingExchangeRates)
        : base($"Missing exchange rate(s): {GetMissingExchangeRates(missingExchangeRates)}")
    {
    }

    private static string GetMissingExchangeRates(IEnumerable<MissingExchangeRateException> missingExchangeRates)
        => missingExchangeRates
            .Select(missingExchangeRate => $"[{missingExchangeRate.Message}]")
            .Aggregate((ratesMessage, rateMessage) => $"{ratesMessage},{rateMessage}");
}