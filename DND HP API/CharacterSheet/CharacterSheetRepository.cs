
namespace DND_HP_API.CharacterSheet;
using DND_HP_API.Domain;

public class CharacterSheetRepository : ICharacterSheetRepository
{
    private readonly List<CharacterSheet> _characterSheets = [];

    public ICollection<CharacterSheet> GetAll()
    {
        return _characterSheets;
    }

    public CharacterSheet? Get(int id)
    {
        return _characterSheets.FirstOrDefault(cs => cs.Id == id);
    }

    public void Add(Domain.CharacterSheet characterSheet)
    {
        characterSheet.Id = _characterSheets.Count + 1;
        _characterSheets.Add(characterSheet);
    }

    public bool Delete(int id)
    {
        var removedCount = _characterSheets.RemoveAll(m => m.Id == id);
        return removedCount > 0;
    }
}