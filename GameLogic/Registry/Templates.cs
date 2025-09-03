namespace GameLogic.Registry;

public interface ITemplate
{
    public TemplateId TemplateId { get; }
}

public abstract class TemplateBase : ITemplate
{
    public TemplateId TemplateId { get; }

    protected TemplateBase(TemplateId id)
    {
        this.TemplateId = id;
    }
}
