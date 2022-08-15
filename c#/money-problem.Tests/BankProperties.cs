using System.Collections.Generic;
using System.Linq;
using FsCheck;
using FsCheck.Xunit;
using money_problem.Domain;
using static money_problem.Domain.Currency;

namespace money_problem.Tests;

public static class MoneyGenerator
{
    public static Arbitrary<Money> Generate()
        => Arb.From(
            from amount in Arb.Generate<double>()
            from currency in Arb.Generate<Currency>()
            select new Money(amount, currency));
}

[Properties(Arbitrary = new[] { typeof(MoneyGenerator) })]
public class BankProperties
{
    private readonly Bank bank;

    private readonly Dictionary<(Currency From, Currency To), double> exchangeRates = new()
    {
        { (EUR, USD), 1.2 },
        { (USD, EUR), 0.82 },
        { (USD, KRW), 1100 },
        { (KRW, USD), 0.0009 },
        { (EUR, KRW), 1344 },
        { (KRW, EUR), 0.00073 }
    };

    public BankProperties()
        => bank = BuildBankWithExchangeRates(exchangeRates);

    private static Bank BuildBankWithExchangeRates(IDictionary<(Currency From, Currency To), double> exchangeRates)
        => exchangeRates.Aggregate(
            NewBank(exchangeRates),
            (bank, exchangeRate) => bank.AddExchangeRate(
                exchangeRate.Key.From,
                exchangeRate.Key.To,
                exchangeRate.Value));

    private static Bank NewBank(IDictionary<(Currency From, Currency To), double> exchangeRates)
    {
        var firstExchangeRate = exchangeRates.First();
        return Bank.WithExchangeRate(
            firstExchangeRate.Key.From,
            firstExchangeRate.Key.To,
            firstExchangeRate.Value);
    }

    [Property(DisplayName = "Round-Tripping in same currency")]
    public Property ConvertInSameCurrencyShouldReturnOriginalMoney(Money money)
        => (money == bank.Convert(money, money.Currency)).Label("Round-Tripping in same currency");
}