using CSharpFunctionalExtensions;

namespace money_problem.Domain;

public readonly struct ConversionResult
{
    private readonly Result<Money, string> result;

    private ConversionResult(Money money)
        => result = Result.Success<Money, string>(money);

    private ConversionResult(string reason)
        => result = Result.Failure<Money, string>(reason);

    internal bool IsSuccess
        => result.IsSuccess;

    internal bool IsFailure
        => result.IsFailure;

    public string Error
        => result.Error;

    internal Money Money
        => result.Value;

    internal static ConversionResult Success(Money money)
        => new(money);

    internal static ConversionResult Failure(string reason)
        => new(reason);
}