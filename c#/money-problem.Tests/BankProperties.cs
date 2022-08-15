using FsCheck;
using FsCheck.Xunit;
using LanguageExt;
using money_problem.Domain;
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
    public Property ConvertInSameCurrencyShouldReturnOriginalMoney(Money money)
        => (money == bank.Convert(money, money.Currency)).Label("Round-Tripping in same currency");
}