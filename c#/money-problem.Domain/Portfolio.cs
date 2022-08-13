﻿using CSharpFunctionalExtensions;

namespace money_problem.Domain;

public class Portfolio
{
    private readonly List<Money> moneys;

    public Portfolio()
        => moneys = new List<Money>();

    public void Add(Money money)
        => moneys.Add(money);

    public Money Evaluate(Bank bank, Currency currency)
    {
        var conversionResults = ConvertMoneys(bank, currency);
        if (IsFailure(conversionResults))
            throw Error(conversionResults);
        return Money(conversionResults, currency);
    }

    private static Money Money(IEnumerable<ConversionResult> conversionResults, Currency currency)
    {
        var totalAmount = conversionResults
            .Select(result => result.Money)
            .Select(money => money.Amount)
            .Sum();

        return new Money(totalAmount, currency);
    }

    private static MissingExchangeRatesException Error(IEnumerable<ConversionResult> conversionResults)
    {
        var missingExchangeRates = conversionResults
            .Where(result => result.IsFailure)
            .Select(failure => failure.Error)
            .ToList();

        return new MissingExchangeRatesException(missingExchangeRates);
    }

    private static bool IsFailure(IEnumerable<ConversionResult> conversionResults)
        => conversionResults.Any(result => result.IsFailure);

    private List<ConversionResult> ConvertMoneys(Bank bank, Currency currency)
        => moneys
            .Select(money => Convert(bank, currency, money))
            .ToList();

    private static ConversionResult Convert(Bank bank, Currency currency, Money money)
    {
        try
        {
            var convertedMoney = bank.Convert(money, currency);

            return ConversionResult.Success(convertedMoney);
        }
        catch (MissingExchangeRateException missingExchangeRate)
        {
            return ConversionResult.Failure(missingExchangeRate);
        }
    }
}

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