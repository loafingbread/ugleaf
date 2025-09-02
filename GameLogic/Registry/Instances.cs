namespace GameLogic.Registry;

public interface IInstanceRegistry
{
    public IInstance GetInstance(InstanceId id);
}

public interface IInstance
{
    public InstanceId Id { get; }
    public TemplateId TemplateId { get; }
}

public abstract class InstanceBase : IInstance
{
    public InstanceId Id { get; } = Ids.Instance();
    public TemplateId TemplateId { get; }

    protected InstanceBase(TemplateId templateId)
    {
        this.TemplateId = templateId;
    }
}
