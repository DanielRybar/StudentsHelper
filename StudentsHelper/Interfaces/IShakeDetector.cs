namespace StudentsHelper.Interfaces
{
    public interface IShakeDetector
    {
        void Start();
        void Stop();

        event Action? OnShaken;
    }
}
