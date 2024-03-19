
using DND_HP_API.Domain;

namespace DND_HP_API.Infrastructure;

public class CharacterSheetRepository(IHpModifierRepository hpModifierRepository) : ICharacterSheetRepository
{
    private readonly List<CharacterSheet> _characterSheets = [];
    private readonly IHpModifierRepository _hpModifierRepository = hpModifierRepository;

    public ICollection<CharacterSheet> GetAll()
    {
        return _characterSheets;
    }

    public CharacterSheet? Get(int id)
    {
        return _characterSheets.FirstOrDefault(cs => cs.Id.Value == id);
    }

    public Id Add(Domain.CharacterSheet characterSheet)
    {
        if (characterSheet.Id.IsTemporary)
        {
            characterSheet.Id = new Id(_characterSheets.Count + 1);
            _characterSheets.Add(characterSheet);
        }
        foreach (var hpModifier in characterSheet.HitPoints.HpModifiers)
        {
            _hpModifierRepository.Add(hpModifier);
        }
        return characterSheet.Id;
    }

    public bool Delete(int id)
    {
        var removedCount = _characterSheets.RemoveAll(m => m.Id.Value == id);
        return removedCount > 0;
    }
}