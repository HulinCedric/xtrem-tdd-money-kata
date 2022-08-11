using System.Collections.Generic;
using FluentAssertions;
using money_problem.Domain;
using Xunit;

namespace money_problem.Tests;

public class PortfolioShould
{
    private readonly Bank bank;

    public PortfolioShould()
    {
        bank = Bank.WithExchangeRate(Currency.EUR, Currency.USD, 1.2);
        bank.AddExchangeRate(Currency.USD, Currency.KRW, 1100);
    }

    [Fact(DisplayName = "5 USD + 10 EUR = 17 USD")]
    public void AddMoneyInDollarAndEuro()
    {
        // Arrange
        var portfolio = new Portfolio();
        portfolio.Add(5, Currency.USD);
        portfolio.Add(10, Currency.EUR);

        // Act
        var evaluation = portfolio.Evaluate(bank, Currency.USD);

        // Assert
        evaluation.Should().Be(17);
    }

    [Fact(DisplayName = "1 USD + 1100 KRW = 2200 KRW")]
    public void AddMoneyInDollarAndKoreanWons()
    {
        // Arrange
        var portfolio = new Portfolio();
        portfolio.Add(1, Currency.USD);
        portfolio.Add(1100, Currency.KRW);

        // Act
        var evaluation = portfolio.Evaluate(bank, Currency.KRW);

        // Assert
        evaluation.Should().Be(2200);
    }
}

public class Portfolio
{
    private readonly Dictionary<Currency, double> moneys;

    public Portfolio()
        => moneys = new Dictionary<Currency, double>();

    public void Add(double amount, Currency currency)
        => moneys.Add(currency, amount);

    public double Evaluate(Bank bank, Currency currency)
    {
        var totalAmount = 0d;
        foreach (var (moneyCurrency, moneyAmount) in moneys)
        {
            var convertedAmount = bank.Convert(moneyAmount, moneyCurrency, currency);

            totalAmount = MoneyCalculator.Add(totalAmount, currency, convertedAmount);
        }

        return totalAmount;
    }
}