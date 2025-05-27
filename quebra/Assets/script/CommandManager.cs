using System.Collections.Generic;
using UnityEngine;

public class CommandManager : MonoBehaviour
{
    public static CommandManager Instance;

    private Stack<ICommand> history = new Stack<ICommand>();
    private List<ICommand> replayList = new List<ICommand>();

    void Awake()
    {
        Instance = this;
    }

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
            ICommand command = history.Pop();
            command.Undo();
        }
    }

    public void Replay(System.Action onComplete)
    {
        StartCoroutine(ReplayCoroutine(onComplete));
    }

    private IEnumerator<WaitForSeconds> ReplayCoroutine(System.Action onComplete)
    {
        foreach (var cmd in replayList)
        {
            cmd.Execute();
            yield return new WaitForSeconds(1f);
        }
        onComplete?.Invoke();
    }

    public void SkipReplay()
    {
        foreach (var cmd in replayList)
            cmd.Execute();
    }

    public void Clear()
    {
        history.Clear();
        replayList.Clear();
    }
}

