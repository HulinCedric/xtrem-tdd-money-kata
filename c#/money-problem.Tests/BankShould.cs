using FluentAssertions;
using money_problem.Domain;
using Xunit;
using static money_problem.Domain.Currency;

namespace money_problem.Tests
{
    public class BankShould
    {
        private readonly Bank bank = Bank.WithExchangeRate(EUR, USD, 1.2);

        [Fact(DisplayName = "10 EUR -> USD = 12 USD")]
        public void ConvertEuroToUsd()
            => bank.Convert(new Money(10, EUR), USD)
                .Should()
                .Be(new Money(12, USD));

        [Fact(DisplayName = "10 EUR -> EUR = 10 EUR")]
        public void ConvertMoneyInSameCurrency()
            => bank.Convert(new Money(10, EUR), EUR)
                .Should()
                .Be(new Money(10, EUR));

        [Fact(DisplayName = "Throws a MissingExchangeRateException in case of missing exchange rates")]
        public void ConvertWithMissingExchangeRateShouldThrowException()
            => bank.Invoking(_ => _.Convert(new Money(10, EUR), KRW))
                .Should()
                .ThrowExactly<MissingExchangeRateException>()
                .WithMessage($"{EUR}->{KRW}");

        [Fact(DisplayName = "Conversion with different exchange rates EUR -> USD")]
        public void ConvertWithDifferentExchangeRates()
        {
            bank.Convert(new Money(10, EUR), USD)
                .Should()
                .Be(new Money(12, USD));

            bank.AddExchangeRate(EUR, USD, 1.3);

            bank.Convert(new Money(10, EUR), USD)
                .Should()
                .Be(new Money(13, USD));
        }
    }
}