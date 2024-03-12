using System.Collections;
using Microsoft.AspNetCore.Mvc;
namespace DND_HP_API.HpCalculator;


// public API for this module 
[ApiController]
[Route("[controller]")]
public class HpCalculatorController:ControllerBase
{
    private readonly List<HpPoll> _hpPolls = [
        new HpPoll() { tempHp = 0, MaxHp = 10, CurrentHp = 10 },
        new HpPoll() { tempHp = 5, MaxHp = 10, CurrentHp = 15 },
    ];
    //endpoint to get current HP
    [HttpGet]
    public IEnumerable<HpPoll> GetHpCalculator()
    {
        return _hpPolls;
    }
    //endpoint to get all HP modifier 
    //endpoint to add new HP modifier 
}

public record HpPoll
{
    public int MaxHp { get; set; }
    public int CurrentHp { get; set; }
    public int tempHp { get; set; }
}

public record HpModifier
{
    //public HpModifierType Type { get; set; }
    public int Amount { get; set; }
    //public 
}

