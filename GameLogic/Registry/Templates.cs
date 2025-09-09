namespace GameLogic.Registry;

public interface IReferenceUnion
{
    public ReferenceUnionMetadata ReferenceMetadata { get; set; }
}

public interface ITemplate
{
}

public abstract class TemplateBase : ITemplate
{
    public ReferenceUnionMetadata ReferenceMetadata { get; set; }

    protected TemplateBase(ReferenceUnionMetadata referenceMetadata)
    {
        this.ReferenceMetadata = referenceMetadata;
    }
}
