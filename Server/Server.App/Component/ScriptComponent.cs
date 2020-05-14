using Giant.Core;
using Giant.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Server.App
{
    public class ScriptComponent : InitSystem, ILoadSystem
    {
        private string scriptPath = "Giant.Script.dll";
        private List<IBaseScript> scripts = new List<IBaseScript>();

        public IBattleCalculator BattleCalculator { get; private set; }

        public override void Init()
        {
            scripts.Clear();

            Load();
        }

        public void Load()
        {
            Assembly assembly = Assembly.LoadFrom(scriptPath);
            if (assembly == null)
            {
                Log.Error($"load assembly error path {scriptPath}");
                return;
            }

            Type scriptType = typeof(IBaseScript);

            var types = assembly.GetTypes();
            foreach (var kv in types)
            {
                if (scriptType.IsAssignableFrom(kv) && !kv.IsInterface && !kv.IsAbstract)
                {
                    scripts.Add(Activator.CreateInstance(kv) as IBaseScript);
                }
            }

            LoadBattleCaculator();
        }

        private void LoadBattleCaculator()
        {
            Type type = typeof(IBattleCalculator);
            BattleCalculator = scripts.Where(x => type.IsInstanceOfType(x)).FirstOrDefault() as IBattleCalculator;
        }
    }
}
