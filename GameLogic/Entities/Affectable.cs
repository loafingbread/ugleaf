namespace GameLogic.Entities;

public interface IAffectable
{
    int ChangeHealth(int amount);
    int ChangeResource(string resourceId, int amount);
    // void ApplyStatus(string statusId, int duration);
    // void ApplyBuff(string stat, int amount, int duration);
}

public class Affectable : IAffectable
{
    private StatBlock _statBlock;

    public Affectable(StatBlock statBlock)
    {
        this._statBlock = statBlock;
    }

    public int ChangeHealth(int amount)
    {
        return 0;
    }

    public int ChangeResource(string resourceId, int amount)
    {
        return 0;
    }
}