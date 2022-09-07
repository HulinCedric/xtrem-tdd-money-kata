<?php

declare(strict_types=1);

namespace MoneyKata;

final class MoneyCalculator
{
    public static function Add(float $amount, Currency $currency, float $addedAmount): float
    {
        return $amount + $addedAmount;
    }

    public static function Times(int $amount, Currency $currency, int $times): float
    {
        return $amount * $times;
    }

    public static function Divide(int $amount, Currency $currency, int $divisor): float
    {
        return $amount / $divisor;
    }
}
