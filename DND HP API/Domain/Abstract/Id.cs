namespace DND_HP_API.Domain.Abstract;

public record Id(int Value, bool IsTemporary = false)
{
    public int Value { get; set; } = Value;
    public bool IsTemporary { get; set; } = IsTemporary;

    public static Id NewTemporaryId(int value=-1)
    {
        return new Id(value, true);
    }
}