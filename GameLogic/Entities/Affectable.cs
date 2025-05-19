namespace GameLogic.Entities;

public interface IAffectable
{
    int ChangeHealth(int amount);
    // void ApplyStatus(string statusId, int duration);
    // void ApplyBuff(string stat, int amount, int duration);
}
