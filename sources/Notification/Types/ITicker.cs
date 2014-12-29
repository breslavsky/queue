namespace Queue.Notification
{
    public interface ITicker
    {
        void SetTicker(string ticker);

        void SetSpeed(int speed);

        void Start();

        void Stop();
    }
}