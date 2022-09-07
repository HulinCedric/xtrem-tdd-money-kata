<?php

namespace Tests;

use MoneyKata\Bank;
use MoneyKata\Currency;
use PHPUnit\Framework\TestCase;

class BankTest extends TestCase
{
    private Bank $bank;

    public function setUp(): void
    {
        $this->bank = Bank::WithExchangeRate(Currency::EUR, Currency::USD, 1.2);
    }

    public function testConvertEuroToUsd()
    {
        $this->assertEquals(12, $this->bank->Convert(10, Currency::EUR, Currency::USD));
    }
}
