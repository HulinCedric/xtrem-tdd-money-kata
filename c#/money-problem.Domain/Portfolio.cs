﻿namespace money_problem.Domain;

public class Portfolio
{
    private readonly List<Money> moneys;

    public Portfolio()
        => moneys = new List<Money>();

    public void Add(Money money)
        => moneys.Add(money);

    public Money Evaluate(Bank bank, Currency currency)
    {
        var totalAmount = 0d;
        var missingExchangeRates = new List<MissingExchangeRateException>();

        foreach (var money in moneys)
            try
            {
                var convertedMoney = bank.Convert(money, currency);

                totalAmount += convertedMoney.Amount;
            }
            catch (MissingExchangeRateException missingExchangeRate)
            {
                missingExchangeRates.Add(missingExchangeRate);
            }

        if (missingExchangeRates.Any())
            throw new MissingExchangeRatesException(missingExchangeRates);

        return new Money(totalAmount, currency);
    }
}