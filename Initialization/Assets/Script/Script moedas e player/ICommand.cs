public partial interface ICommand
{
    void Execute();
    void Undo();
}