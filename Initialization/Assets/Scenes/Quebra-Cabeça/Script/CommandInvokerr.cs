using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandInvokerr
{
    private Stack<ICommand> history = new Stack<ICommand>();
    private List<ICommand> replayList = new List<ICommand>();

    public void ExecuteCommand(ICommand command)
    {
        command.Execute();
        history.Push(command);
        replayList.Add(command);
    }

    public void Undo()
    {
        if (history.Count > 0)
        {
            var last = history.Pop();
            last.Undo();
        }
    }

    public IEnumerator Replay(float delay)
    {
        foreach (var command in replayList)
        {
            command.Execute();
            yield return new WaitForSeconds(delay);
        }
    }

    public void SkipReplay()
    {
        foreach (var command in replayList)
        {
            command.Execute();
        }
    }

    public void Reset()
    {
        history.Clear();
        replayList.Clear();
    }

    public class Instance
    {
        public static void Replay(float f)
        {
            throw new System.NotImplementedException();
        }

        public static void Undo()
        {
            throw new System.NotImplementedException();
        }

        public static void SkipReplay()
        {
            throw new System.NotImplementedException();
        }
    }
}
