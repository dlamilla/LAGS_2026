using System;
using UnityEngine;

public class EventBus
{
    public static Action OnFishCapturedEvent;

    public static void OnFishCaptured()
    {
        OnFishCapturedEvent?.Invoke();
    }
}
