namespace GameLogic.Registry;

public interface IReferenceUnion
{
    public ReferenceMetadata ReferenceMetadata { get; set; }
}

public interface ITemplate<TOverride>
{
    public ReferenceMetadata ReferenceMetadata { get; set; }
    public TOverride? TemplateOverride { get; set; }
}

// public abstract class TemplateBase : ITemplate
// {
//     public ReferenceMetadata ReferenceMetadata { get; set; }

//     protected TemplateBase(ReferenceMetadata referenceMetadata)
//     {
//         this.ReferenceMetadata = referenceMetadata;
//     }
// }
