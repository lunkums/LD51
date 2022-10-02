namespace LD51
{
    public interface IEntity : IGoodDrawable, IGoodUpdateable
    {
        uint Id { get; }
        void Despawn();
    }
}
