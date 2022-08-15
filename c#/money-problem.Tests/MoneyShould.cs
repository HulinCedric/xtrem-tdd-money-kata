using FluentAssertions;
using money_problem.Domain;
using Xunit;

namespace money_problem.Tests
{
    public class MoneyShould
    {
        [Fact(DisplayName = "10 EUR x 2 = 20 EUR")]
        public void MultiplyInEuros()
            => 10d.Euros()
                .Times(2)
                .Should()
                .Be(20d.Euros());

        [Fact(DisplayName = "4002 KRW / 4 = 1000.5 KRW")]
        public void DivideInKoreanWons()
            => 4002d.KoreanWons()
                .Divide(4)
                .Should()
                .Be(1000.5d.KoreanWons());

        [Theory(DisplayName = "Equals with relative tolerance of 0.1%")]
        [InlineData(10, 10.01)]
        [InlineData(100, 100.1)]
        [InlineData(1_000, 1_001)]
        public void BeEqualWithRelativeTolerance(double amountA, double amountB)
        {
            // Arrange
            var moneyA = amountA.Euros();
            var moneyB = amountB.Euros();

            // Assert
            moneyA.Should().Be(moneyB);
        }
    }
}