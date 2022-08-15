using FsCheck;
using money_problem.Domain;

namespace money_problem.Tests;

public static class MoneyGenerator
{
    public static Arbitrary<Money> Generate()
        => Arb.From(
            from amount in Arb.Generate<double>()
            from currency in Arb.Generate<Currency>()
            where amount is <= Money.MaxAmount and >= Money.MinAmount
            select new Money(amount, currency));
}