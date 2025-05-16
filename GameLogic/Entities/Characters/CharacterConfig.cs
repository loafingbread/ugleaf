using System.Diagnostics.CodeAnalysis;

namespace GameLogic.Entities.Characters;

public interface ICharacterRecord
{
    public string Id { get; }
    public string Name { get; }
    public int Health { get; }
    public int Attack { get; }
    public int Defense { get; }
}

public record CharacterRecord : ICharacterRecord
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public int Health { get; init; }
    public int Attack { get; init; }
    public int Defense { get; init; }
}

public interface ICharacterConfig
{
    public string Id { get; init; }
    public string Name { get; init; }
    public int Health { get; init; }
    public int Attack { get; init; }
    public int Defense { get; init; }
}

public class CharacterConfig : ICharacterConfig
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public int Health { get; init; }
    public int Attack { get; init; }
    public int Defense { get; init; }

    [SetsRequiredMembers]
    public CharacterConfig(ICharacterRecord record)
    {
        this.Id = record.Id;
        this.Name = record.Name;
        this.Health = record.Health;
        this.Attack = record.Attack;
        this.Defense = record.Defense;
    }
}
