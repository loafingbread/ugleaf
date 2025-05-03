namespace GameLogic.Usables;

using System.Security.Cryptography.X509Certificates;
using GameLogic.Entities;

public interface IUsable
{
    public UsableResult Use(Entity user, Entity target);
}

public class UsableResult
{
    public Entity User { get; }
    public Entity Target { get; }
    public bool DidCrit { get; }
    public bool DidHit { get; }
    public IUsable Usable { get; }
    public int Value { get; }

    public UsableResult(
        Entity user, Entity target,
        bool didCrit, bool didHit,
        IUsable usable, int value 
    ) {
        this.User = user;
        this.Target = target;
        this.DidCrit = didCrit;
        this.DidHit = didHit;
        this.Usable = usable;
        this.Value = value;
    }

}