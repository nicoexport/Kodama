using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Context : Singleton<Context>
{
    private List<IContextManager> contextManagers = new List<IContextManager>();

    public void RegisterContextManager(IContextManager manager)
    {
        if (!contextManagers.Contains(manager)) contextManagers.Add(manager);
    }

    public void UnRegisterContextManager(IContextManager manager)
    {
        if (contextManagers.Contains(manager)) contextManagers.Remove(manager);
    }

    public void OnGameModeStarted()
    {
        for (int i = contextManagers.Count - 1; i >= 0; i--) contextManagers[i].OnGameModeStarted();
    }
}