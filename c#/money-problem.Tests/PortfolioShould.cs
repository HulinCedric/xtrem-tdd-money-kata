using FluentAssertions;
using money_problem.Domain;
using Xunit;
using static money_problem.Domain.Currency;

namespace money_problem.Tests;

public class PortfolioShould
{
    private readonly Bank _bank;

    public PortfolioShould()
    {
        _bank = Bank.WithExchangeRate(EUR, USD, 1.2);
        _bank.AddExchangeRate(USD, KRW, 1100);
    }

    [Fact(DisplayName = "5 USD + 10 EUR = 17 USD")]
    public void todo()
    {
        var portfolio = new Portoflio(_bank);

        portfolio.Add(5, USD);
        portfolio.Add(10, EUR);

        portfolio.GetTotalAmount(USD).Should().Be(17d);
    }

    [Fact(DisplayName = "1 USD + 1100 KRW = 2200 KRW")]
    public void todo2()
    {
        var portfolio = new Portoflio(_bank);

        portfolio.Add(1, USD);
        portfolio.Add(1100, KRW);

        portfolio.GetTotalAmount(KRW).Should().Be(2200);
    }

    [Fact(DisplayName = "5 USD + 10 EUR + 4 EUR = 21.8 USD")]
    public void todo3()
    {
        var portfolio = new Portoflio(_bank);

        portfolio.Add(5, USD);
        portfolio.Add(10, EUR);
        portfolio.Add(4, EUR);

        portfolio.GetTotalAmount(USD).Should().Be(21.8);
    }

    [Fact(DisplayName = "1 EUR + 1 USD + 1 KRW = ? EUR Throw MissingExchangeRatesException: USD->EUR,KRW->EUR")]
    public void todo4()
    {
        var portfolio = new Portoflio(_bank);

        portfolio.Add(1, EUR);
        portfolio.Add(1, USD);
        portfolio.Add(1, KRW);

        portfolio.Invoking(_ => _.GetTotalAmount(EUR))
            .Should()
            .ThrowExactly<MissingExchangeRatesException>()
            .WithMessage("USD->EUR,KRW->EUR");
    }
}