using GameLogic.Targeting;

namespace GameLogic.Actions.Effects;

using GameLogic.Registry;
using GameLogic.Utils;

public abstract class Effect 
{
    public EffectTemplate Template { get; set; }
    public EffectState State { get; set; }

    public Effect(EffectTemplate template, EffectState state)
    {
        this.Template = template;
        this.State = state;
    }
}