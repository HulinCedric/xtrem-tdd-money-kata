<?php

namespace MoneyKata;

final class Bank
{
    private array $exchangeRates;

    public function __construct(array $exchangeRates)
    {
        $this->exchangeRates = $exchangeRates;
    }

    public static function WithExchangeRate(Currency $from, Currency $to, float $rate): Bank
    {
        return new Bank(["$from->name->$to->name" => $rate]);
    }

    public function Convert(float $amount, Currency $from, Currency $to): float
    {
        return $amount * $this->exchangeRates["$from->name->$to->name"];
    }
}