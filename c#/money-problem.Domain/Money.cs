namespace money_problem.Domain;

public record Money(double Amount, Currency Currency)
{
    private const double Tolerance = 0.1;

    public virtual bool Equals(Money? other)
        => other is not null &&
           AmountAreEquals(Amount, other.Amount) &&
           Currency == other.Currency;

    public override int GetHashCode()
        => HashCode.Combine(Amount, (int)Currency);

    public Money Times(int times)
        => this with { Amount = Amount * times };

    public Money Divide(int divisor)
        => this with { Amount = Amount / divisor };

    private static bool AmountAreEquals(double firstAmount, double secondAmount)
        => EqualsWithTolerance(firstAmount, secondAmount, Tolerance);

    private static bool EqualsWithTolerance(double x, double y, double tolerance)
    {
        var difference = Math.Abs(x - y);
        return difference <= tolerance ||
               difference <= Math.Max(Math.Abs(x), Math.Abs(y)) * tolerance;
    }
}