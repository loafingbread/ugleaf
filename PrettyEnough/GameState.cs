namespace PrettyEnough;

using GameLogic.Entities.Stats;

public class GameState
{
    public StatBlock? StatBlock { get; set; }

    // Future additions:
    // public List<Character> Characters { get; set; } = new();
    // public Combat? CurrentCombat { get; set; }
    // public List<Skill> AvailableSkills { get; set; } = new();
}
