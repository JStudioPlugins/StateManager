using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace StateManager
{
    public class Main : RocketPlugin<Config>
    {
        public static Main Instance { get; private set; }

        public static Config Config { get => Instance.Configuration.Instance; }

        public delegate void OnLoadHandler();
        public OnLoadHandler OnLoad;

        public delegate void OnUnloadHandler();
        public OnUnloadHandler OnUnload;

        protected override void Load()
        {
            Instance = this;
            Logger.Log($"StateManager {Assembly.GetName().Version} is now loaded.");
            OnLoad?.Invoke();
        }

        protected override void Unload()
        {
            OnUnload?.Invoke();
            Logger.Log($"StateManager is now unloaded.");
        }
    }

    public abstract class Eventual
    {
        public Eventual()
        {
            Main.Instance.OnUnload += CleanUp;
        }

        public abstract void CleanUp();
    }
}
