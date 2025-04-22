namespace GameLogic.Entities;

public interface ICharacterData
{
    public string Id { get; }
    public string Name { get; }
    public int Health { get; }
    public int Attack { get; }
    public int Defense { get; }
}

public record CharacterConfig : ICharacterData
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public int Health { get; init; }
    public int Attack { get; init; }
    public int Defense { get; init; }
}
