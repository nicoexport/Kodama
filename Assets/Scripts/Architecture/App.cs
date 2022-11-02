using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Architecture {
    public static class App {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InitializeApp() {
            var app = Object.Instantiate(Resources.Load("P_App")) as GameObject;
            if (app == null) {
                throw new ApplicationException();
            }

            Object.DontDestroyOnLoad(app);
        }
    }
}