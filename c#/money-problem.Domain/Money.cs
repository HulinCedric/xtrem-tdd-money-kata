namespace money_problem.Domain;

public record Money(double Amount, Currency Currency)
{
    private const double Tolerance = 0.1d;

    public const int MaxDigits = 5;
    public const double MaxAmount = 1_000_000_000d;
    public const double MinAmount = -1_000_000_000d;

    public virtual bool Equals(Money? other)
        => other is not null &&
           AreAmountEquals(Amount, other.Amount) &&
           Currency == other.Currency;

    public override int GetHashCode()
        => HashCode.Combine(Amount, (int)Currency);

    public Money Times(int times)
        => this with { Amount = Amount * times };

    public Money Divide(int divisor)
        => this with { Amount = Amount / divisor };

    private static bool AreAmountEquals(double firstAmount, double secondAmount)
    {
        var amountDifference = Math.Abs(firstAmount - secondAmount);

        return amountDifference <= GetTolerance(firstAmount);
    }

    private static double GetTolerance(double firstAmount)
        => Math.Abs(firstAmount * Tolerance);
}