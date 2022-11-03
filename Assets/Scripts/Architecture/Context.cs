using System.Collections.Generic;
using Kodama.Utility;
using Kodama.World_Level;

namespace Kodama.Architecture {
    public class Context : Singleton<Context> {
        private readonly List<IContextManager> contextManagers = new();

        public void RegisterContextManager(IContextManager manager) {
            if (!contextManagers.Contains(manager)) {
                contextManagers.Add(manager);
            }
        }

        public void UnRegisterContextManager(IContextManager manager) {
            if (contextManagers.Contains(manager)) {
                contextManagers.Remove(manager);
            }
        }

        public void OnGameModeStarted() {
            for (int i = contextManagers.Count - 1; i >= 0; i--) {
                contextManagers[i].OnGameModeStarted();
            }
        }

        public void RequestLevelLoad(LevelObject levelObject) {
        }
    }
}