using System.Collections.Generic;
using UnityEngine;

public class CommandManager
{
    public List<ICommand> commands; 

    public CommandManager()
    {
        commands = new List<ICommand>();
    }

    public void AddCommand(ICommand command) // recebe um comando e adiciona na lista
    {
        commands.Add(command);
    }
    public void UndoCommand() // ela desfaz outro comando e remove da lista
    {
        ICommand command = commands[^1];
        commands.RemoveAt(commands.Count - 1); // remove o ultimo elemento da lista
        command.Undo(); // depois executa eese elemento
    }
}
