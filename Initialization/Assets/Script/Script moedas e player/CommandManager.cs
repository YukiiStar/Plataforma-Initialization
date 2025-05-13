using System.Collections.Generic;
using UnityEngine;

public class CommandManager
{
    public List<ICommand> commands;

    public CommandManager()
    {
        commands = new List<ICommand>();
    }

    public void AddCommand(ICommand command)
    {
        commands.Add(command);
    }

    public void DoCommand()
    {
        commands[^1].Do();
    }
}
