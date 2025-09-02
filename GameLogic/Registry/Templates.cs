namespace GameLogic.Registry;

public interface ITemplate
{
    public TemplateId Id { get; }
}

public abstract class TemplateBase : ITemplate
{
    public TemplateId Id { get; }

    protected TemplateBase(TemplateId id)
    {
        this.Id = id;
    }
}
