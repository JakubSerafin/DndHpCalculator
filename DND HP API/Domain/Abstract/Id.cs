namespace DND_HP_API.Domain.Abstract;

public record Id(long Value, bool IsTemporary = false)
{
    public long Value { get; set; } = Value;
    public bool IsTemporary { get; set; } = IsTemporary;

    public static Id NewTemporaryId(int value=-1)
    {
        return new Id(value, true);
    }
}