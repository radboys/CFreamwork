namespace CFramework.Core
{
    public enum UpdatePhase
    {
        Input = 0,
        Logic = 1,
        Physics = 2,
        Render = 3
    }

    public interface IUpdatable
    {
        UpdatePhase Phase { get; }
        void OnUpdate();
    }

    public interface ILateUpdatable
    {
        void OnLateUpdate();
    }

    public interface IFixedUpdatable
    {
        void OnFixedUpdate();
    }
}



