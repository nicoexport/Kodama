using System.Collections.Generic;
using Utility;
using World_Level;

namespace Architecture
{
    public class Context : Singleton<Context>
    {
        private readonly List<IContextManager> contextManagers = new();

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
            for (var i = contextManagers.Count - 1; i >= 0; i--) contextManagers[i].OnGameModeStarted();
        }

        public void RequestLevelLoad(LevelObject levelObject)
        {
        }
    }
}