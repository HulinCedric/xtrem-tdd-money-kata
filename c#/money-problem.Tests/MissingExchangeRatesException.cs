using System;
using System.Collections.Generic;
using System.Linq;
using money_problem.Domain;

namespace money_problem.Tests;

public class MissingExchangeRatesException : Exception
{
    public MissingExchangeRatesException(List<MissingExchangeRateException> missingExchangeRates) : base(
        string.Join(",", missingExchangeRates.Select(e=>e.Message)))
    {
    }
}