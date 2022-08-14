using FluentAssertions;
using money_problem.Domain;
using Xunit;
using static money_problem.Domain.Currency;

namespace money_problem.Tests;

public class PortfolioShould
{
    private readonly Bank bank = Bank
        .WithExchangeRate(EUR, USD, 1.2)
        .AddExchangeRate(USD, KRW, 1100);

    [Fact(DisplayName = "5 USD + 10 EUR = 17 USD")]
    public void AddMoneyInDollarAndEuro()
    {
        // Arrange
        var portfolio = Portfolio.Empty
            .Add(5d.Dollars())
            .Add(10d.Euros());

        // Act
        var evaluation = portfolio.Evaluate(bank, USD);

        // Assert
        evaluation.Money.Should().Be(17d.Dollars());
    }

    [Fact(DisplayName = "1 USD + 1100 KRW = 2200 KRW")]
    public void AddMoneyInDollarAndKoreanWons()
    {
        // Arrange
        var portfolio = Portfolio.Empty
            .Add(1d.Dollars())
            .Add(1100d.KoreanWons());

        // Act
        var evaluation = portfolio.Evaluate(bank, KRW);

        // Assert
        evaluation.Money.Should().Be(2200d.KoreanWons());
    }

    [Fact(DisplayName = "5 USD + 10 EUR + 4 EUR = 21.8 USD")]
    public void AddMoneyInDollarAndMultipleInEuros()
    {
        // Arrange
        var portfolio = Portfolio.WithMoneys(
            5d.Dollars(),
            10d.Euros(),
            4d.Euros());

        // Act
        var evaluation = portfolio.Evaluate(bank, USD);

        // Assert
        evaluation.Money.Should().Be(21.8d.Dollars());
    }

    [Fact(DisplayName = "5 USD + 10 USD = 15 USD")]
    public void AddMoneyInSameCurrency()
    {
        // Arrange
        var portfolio = Portfolio.WithMoneys(
            5d.Dollars(),
            10d.Dollars());

        // Act
        var evaluation = portfolio.Evaluate(bank, USD);

        // Assert
        evaluation.Money.Should().Be(15d.Dollars());
    }

    [Fact(DisplayName = "Return missing exchange rates failure")]
    public void ReturnsMissingExchangeRatesFailure()
    {
        // Arrange
        var portfolio = Portfolio.WithMoneys(
            1d.Euros(),
            1d.Dollars(),
            1d.KoreanWons());

        // Act
        var evaluation = portfolio.Evaluate(bank, EUR);

        // Assert
        evaluation.Error.Should().Be("Missing exchange rate(s): [USD->EUR],[KRW->EUR]");
    }

    [Fact(DisplayName = "Empty = 0 USD")]
    public void ReturnZeroDollarWhenEmpty()
    {
        // Arrange
        var portfolio = Portfolio.Empty;

        // Act
        var evaluation = portfolio.Evaluate(bank, USD);

        // Assert
        evaluation.Money.Should().Be(0d.Dollars());
    }
}