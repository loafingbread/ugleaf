namespace GameLogic.Actions.Effects;

using GameLogic.Registry;

public class EffectResolver
{
    public static Effect ToEffect(EffectStateRef effectStateRef)
    {
        return new Effect(effectStateRef.EffectState);
    }
}