using Propses;

namespace MedivalTycoon
{
    public interface IPropsPool
    {
        IProps Spawn();
        void Despawn(IProps prop);
    }
}