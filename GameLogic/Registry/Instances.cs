namespace GameLogic.Registry;

// public interface IInstanceRegistry
// {
//     public IInstance GetInstance(InstanceId id);
// }

public interface IInstance<TInstanceState>
{
    public InstanceId InstanceId { get; set; }
    public TInstanceState? InstanceState { get; set; }
}

// public abstract class InstanceBase : IInstance
// {
//     public InstanceId InstanceId { get; } = Ids.Instance();

//     protected InstanceBase(ReferenceUnionMetadata referenceMetadata)
//     {
//         this.ReferenceMetadata = referenceMetadata;
//     }
// }
