using System;

public static class StaticEventChannel
{
    public static Action<string> OnButtonPressed;

    public static void RaiseButtonPressed(string buttonID)
    {
        OnButtonPressed?.Invoke(buttonID);
    }
}
