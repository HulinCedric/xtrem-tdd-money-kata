﻿using money_problem.Domain;

namespace money_problem.Tests;

public static class MoneyExtensions
{
    public static Money Euros(this int amount)
        => new(amount, Currency.EUR);

    public static Money KoreanWons(this int amount)
        => new(amount, Currency.KRW);

    public static Money Dollars(this int amount)
        => new(amount, Currency.USD);
}