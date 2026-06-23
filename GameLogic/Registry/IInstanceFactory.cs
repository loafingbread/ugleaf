namespace GameLogic.Registry;

public interface IInstanceFactory<TTemplate, TData, TInstance>
    where TInstance : IToInstanceData<TData>
{
    TInstance Create(TTemplate template, TData data);
}
