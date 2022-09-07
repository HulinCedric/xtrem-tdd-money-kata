<?php

declare(strict_types=1);

namespace Tests;

use MoneyKata\MoneyCalculator;
use MoneyKata\Currency;
use PHPUnit\Framework\TestCase;

class MoneyCalculatorTest extends TestCase
{
    /**
     * @testdox 5 USD + 10 USD = 15 USD
     */
    public function testAddInUsd(): void
    {
        $this->assertNotNull(MoneyCalculator::Add(5, Currency::USD, 10));
    }

    /**
     * @testdox 10 EUR x 2 = 20 EUR
     */
    public function testMultiplyInEuros(): void
    {
        $this->assertEquals(
            20,
            MoneyCalculator::Times(10, Currency::EUR, 2));
    }

    /**
     * @testdox 4002 KRW / 4 = 1000.5 KRW
     */
    public function testDivideInKoreanWons(): void
    {
        $this->assertEquals(
            1000.5,
            MoneyCalculator::Divide(4002, Currency::KRW, 4));
    }
}