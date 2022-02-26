using UnityEngine;
using System;

public static class App
{

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void InitializeApp()
    {
        var app = UnityEngine.Object.Instantiate(Resources.Load("App")) as GameObject;
        if (app == null) throw new ApplicationException();

        UnityEngine.Object.DontDestroyOnLoad(app);
    }

}