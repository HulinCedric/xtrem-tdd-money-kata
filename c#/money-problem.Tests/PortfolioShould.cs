using FluentAssertions;
using money_problem.Domain;
using Xunit;
using static money_problem.Domain.Currency;

namespace money_problem.Tests;

public class PortfolioShould
{
    private readonly Bank bank;

    public PortfolioShould()
    {
        bank = Bank.WithExchangeRate(EUR, USD, 1.2);
        bank.AddExchangeRate(USD, KRW, 1100);
    }

    [Fact(DisplayName = "5 USD + 10 EUR = 17 USD")]
    public void AddMoneyInDollarAndEuro()
    {
        // Arrange
        var portfolio = new Portfolio();
        portfolio.Add(5, USD);
        portfolio.Add(10, EUR);

        // Act
        var evaluation = portfolio.Evaluate(bank, USD);

        // Assert
        evaluation.Should().Be(17);
    }

    [Fact(DisplayName = "1 USD + 1100 KRW = 2200 KRW")]
    public void AddMoneyInDollarAndKoreanWons()
    {
        // Arrange
        var portfolio = new Portfolio();
        portfolio.Add(1, USD);
        portfolio.Add(1100, KRW);

        // Act
        var evaluation = portfolio.Evaluate(bank, KRW);

        // Assert
        evaluation.Should().Be(2200);
    }

    [Fact(DisplayName = "5 USD + 10 EUR + 4 EUR = 21.8 USD")]
    public void AddMoneyInDollarAndMultipleInEuros()
    {
        // Arrange
        var portfolio = new Portfolio();
        portfolio.Add(5, USD);
        portfolio.Add(10, EUR);
        portfolio.Add(4, EUR);

        // Act
        var evaluation = portfolio.Evaluate(bank, USD);

        // Assert
        evaluation.Should().Be(21.8);
    }

    [Fact(DisplayName = "5 USD + 10 USD = 15 USD")]
    public void AddMoneyInSameCurrency()
    {
        // Arrange
        var portfolio = new Portfolio();
        portfolio.Add(5, USD);
        portfolio.Add(10, USD);

        // Act
        var evaluation = portfolio.Evaluate(bank, USD);

        // Assert
        evaluation.Should().Be(15);
    }

    [Fact(DisplayName = "Throws a MissingExchangeRatesException in case of missing exchange rates")]
    public void ThrowAMissingExchangeRatesException()
    {
        // Arrange
        var portfolio = new Portfolio();
        portfolio.Add(1, EUR);
        portfolio.Add(1, USD);
        portfolio.Add(1, KRW);

        // Act
        var act = () => portfolio.Evaluate(bank, EUR);

        // Assert
        act.Should()
            .ThrowExactly<MissingExchangeRatesException>()
            .WithMessage("Missing exchange rate(s): [USD->EUR],[KRW->EUR]");
    }
}