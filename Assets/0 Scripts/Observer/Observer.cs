using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer : MonoBehaviour
{
    static Dictionary<string, List<Action>> listeners = new Dictionary<string, List<Action>>();
    public static void AddObserver(string name, Action callback)
    {
        if (!listeners.ContainsKey(name))
        {
            listeners.Add(name, new List<Action>());
        }

        listeners[name].Add(callback);
    }

    public static void RemoveObserver(string name, Action callback)
    {
        if (!listeners.ContainsKey(name))
        {
            return;
        }
        listeners[name].Remove(callback);
    }

    public static void Notify(string name)
    {
        if (!listeners.ContainsKey(name))
        {
            return;
        }

        foreach (var action in listeners[name])
        {
            try
            {
                action?.Invoke();

            }catch (Exception ex)
            {
                Debug.LogError( "Error on invoke: "+ex);
            }
        }
    }
}
