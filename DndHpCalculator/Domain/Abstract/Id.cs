namespace DND_HP_API.Domain.Abstract;

public class Id(long value, bool isTemporary = false)
{
    public long Value { get; } = value;
    public bool IsTemporary { get; } = isTemporary;

    public static Id NewTemporaryId(long? value = null)
    {
        return new Id(value ?? DateTime.Now.Ticks, true);
    }

    public static Id NewTemporaryId(string? value)
    {
        if (value != null && long.TryParse(value, out var parsedValue)) return new Id(parsedValue, true);
        throw new ArgumentException("Invalid temporary id");
    }

    public static Id NewTimestampedId()
    {
        return new Id(DateTime.Now.Ticks);
    }

    public override bool Equals(object? obj)
    {
        if (obj is Id otherId) return Value == otherId.Value;
        if (obj is string other)
            if (long.TryParse(other, out var otherValue))
                return Value == otherValue;
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Value, IsTemporary);
    }
}