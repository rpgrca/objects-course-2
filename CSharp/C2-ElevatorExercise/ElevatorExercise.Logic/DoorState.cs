namespace ElevatorExercise.Logic
{
    public interface DoorState
    {
        bool IsClosed();

        bool IsClosing();

        bool IsOpened();

        bool IsOpening();
        void Open();
        void OnClosed();
        void Close();
    }
}