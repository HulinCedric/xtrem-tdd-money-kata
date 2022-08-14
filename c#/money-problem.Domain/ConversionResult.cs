using CSharpFunctionalExtensions;

namespace money_problem.Domain;

public readonly struct ConversionResult
{
    private readonly Result<Money, MissingExchangeRateException> result;

    private ConversionResult(Money money)
        => result = Result.Success<Money, MissingExchangeRateException>(money);

    private ConversionResult(MissingExchangeRateException exception)
        => result = Result.Failure<Money, MissingExchangeRateException>(exception);
    
    internal bool IsSuccess
        => result.IsSuccess;

    internal bool IsFailure
        => result.IsFailure;

    public string Error
        => result.Error.Message;

    internal Money Money
        => result.Value;

    internal static ConversionResult Success(Money money)
        => new(money);

    internal static ConversionResult Failure(MissingExchangeRateException exception)
        => new(exception);
}