namespace DND_HP_API.Domain;

public enum DefenceType
{
    Immunity,
    Resistance
}

public static class DefenceTypeExtensions
{
    //Percent of damage to reduce
    public static double ReduceFactor(this DefenceType defenceType)
    {
        switch (defenceType)
        {
            case DefenceType.Immunity:
                return 1;
            case DefenceType.Resistance:
                return 0.5;
            default:
                throw new ArgumentOutOfRangeException(nameof(defenceType), defenceType, null);
        }
    }
}