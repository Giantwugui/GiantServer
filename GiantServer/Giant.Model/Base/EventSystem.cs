using System;
using System.Collections.Generic;
using System.Reflection;

namespace Giant.Model
{
    public enum DllType
    {
        Model,
        Hotfix
    }

    /// <summary>
    /// 驱动系统
    /// </summary>
    public sealed class EventSystem
    {
        private Dictionary<DllType, Assembly> assemblies = new Dictionary<DllType, Assembly>();

        private Dictionary<Type, Type> types = new Dictionary<Type, Type>();

        private Dictionary<Type, IAwakeSystem> awakesystems = new Dictionary<Type, IAwakeSystem>();


        public void Add(DllType dllType, Assembly assembly)
        {
            this.assemblies[dllType] = assembly;
        }
    }
}
