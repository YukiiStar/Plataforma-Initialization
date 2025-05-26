public interface ICommand
{
    void Execute();
    void Undo();
    void Do();
}