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
        portfolio.Add(5d.Dollars());
        portfolio.Add(10d.Euros());

        // Act
        var evaluation = portfolio.Evaluate(bank, USD);

        // Assert
        evaluation.Should().Be(17d.Dollars());
    }

    [Fact(DisplayName = "1 USD + 1100 KRW = 2200 KRW")]
    public void AddMoneyInDollarAndKoreanWons()
    {
        // Arrange
        var portfolio = new Portfolio();
        portfolio.Add(1d.Dollars());
        portfolio.Add(1100d.KoreanWons());

        // Act
        var evaluation = portfolio.Evaluate(bank, KRW);

        // Assert
        evaluation.Should().Be(2200d.KoreanWons());
    }

    [Fact(DisplayName = "5 USD + 10 EUR + 4 EUR = 21.8 USD")]
    public void AddMoneyInDollarAndMultipleInEuros()
    {
        // Arrange
        var portfolio = new Portfolio();
        portfolio.Add(5d.Dollars());
        portfolio.Add(10d.Euros());
        portfolio.Add(4d.Euros());

        // Act
        var evaluation = portfolio.Evaluate(bank, USD);

        // Assert
        evaluation.Should().Be(21.8d.Dollars());
    }

    [Fact(DisplayName = "5 USD + 10 USD = 15 USD")]
    public void AddMoneyInSameCurrency()
    {
        // Arrange
        var portfolio = new Portfolio();
        portfolio.Add(5d.Dollars());
        portfolio.Add(10d.Dollars());

        // Act
        var evaluation = portfolio.Evaluate(bank, USD);

        // Assert
        evaluation.Should().Be(15d.Dollars());
    }

    [Fact(DisplayName = "Throws a MissingExchangeRatesException in case of missing exchange rates")]
    public void ThrowAMissingExchangeRatesException()
    {
        // Arrange
        var portfolio = new Portfolio();
        portfolio.Add(1d.Euros());
        portfolio.Add(1d.Dollars());
        portfolio.Add(1d.KoreanWons());

        // Act
        var act = () => portfolio.Evaluate(bank, EUR);

        // Assert
        act.Should()
            .ThrowExactly<MissingExchangeRatesException>()
            .WithMessage("Missing exchange rate(s): [USD->EUR],[KRW->EUR]");
    }

    [Fact(DisplayName = "Empty = 0 USD")]
    public void ReturnZeroDollarWhenEmpty()
    {
        // Arrange
        var portfolio = new Portfolio();

        // Act
        var evaluation = portfolio.Evaluate(bank, USD);

        // Assert
        evaluation.Should().Be(0d.Dollars());
    }
}