using FluentAssertions.LanguageExt;
using FsCheck;
using FsCheck.Xunit;
using LanguageExt;
using money_problem.Domain;
using Xunit;
using static money_problem.Domain.Currency;

namespace money_problem.Tests;

[Properties(Arbitrary = new[] { typeof(MoneyGenerator) })]
public class BankProperties
{
    private readonly Bank bank;

    private readonly Seq<ExchangeRate> exchangeRates = Seq<ExchangeRate>.Empty
        .Add(new ExchangeRate(EUR, USD, 1.2))
        .Add(new ExchangeRate(USD, EUR, 0.82))
        .Add(new ExchangeRate(USD, KRW, 1100))
        .Add(new ExchangeRate(KRW, USD, 0.0009))
        .Add(new ExchangeRate(EUR, KRW, 1344))
        .Add(new ExchangeRate(KRW, EUR, 0.00073));

    public BankProperties()
        => bank = Bank.WithExchangeRates(exchangeRates.ToArray());

    [Property(DisplayName = "Round-Tripping in same currency")]
    public Property ConvertInSameCurrencyShouldReturnOriginalMoney(Money originalMoney)
        => (originalMoney == bank.Convert(originalMoney, originalMoney.Currency))
            .Label("Round-Tripping in same currency");

    [Property(DisplayName = "Round-Tripping in random currency")]
    public Property RoundTripConversionShouldReturnOriginalMoney(Money originalMoney, Currency currency)
        => (originalMoney ==
            RoundTripConversion(originalMoney, currency))
            .Label("Round-Tripping in random currency");

    private Either<string, Money> RoundTripConversion(Money originalMoney, Currency currency)
        => bank.Convert(originalMoney, currency)
            .Bind(convertedMoney => bank.Convert(convertedMoney, originalMoney.Currency));

    [Fact(DisplayName = "?")]
    public void RoundTripFailure()
    {
        // Arrange
        var originalMoney = new Money(-0.22350680544256452, EUR);
        var currency = KRW;

        // Act
        var roundTripMoney = RoundTripConversion(originalMoney, currency);

        // Assert
        roundTripMoney.Should().Be(originalMoney);
    }
}