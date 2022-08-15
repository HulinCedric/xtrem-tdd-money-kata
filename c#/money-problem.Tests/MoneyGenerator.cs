using System;
using FsCheck;
using money_problem.Domain;

namespace money_problem.Tests;

public static class MoneyGenerator
{
    public static Arbitrary<Money> Generate()
        => Arb.From(
            from amount in GetAmountGenerator()
            from currency in Arb.Generate<Currency>()
            select new Money(amount, currency));
    
    private static Gen<double> GetAmountGenerator()
        => Arb.Default.Float()
            .MapFilter(
                amount => Math.Round(amount, Money.MaxDigits),
                amount => amount is <= Money.MaxAmount and >= Money.MinAmount)
            .Generator;
}