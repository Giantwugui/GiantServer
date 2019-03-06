using System;

namespace Giant.Model
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple =true)]
    class BaseAttribute : Attribute
    {
        public Type AttributeType
        {
            get { return this.GetType(); }
        }
    }

    /// <summary>
    /// 系统驱动系统标识
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple =true)]
    class ObjectSystemAttribute : BaseAttribute
    {
    }
}
