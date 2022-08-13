using CSharpFunctionalExtensions;

namespace money_problem.Domain;

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