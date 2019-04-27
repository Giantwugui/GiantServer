using System;

namespace Giant.Model
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple =true)]
    public class BaseAttribute : Attribute
    {
        public Type AttributeType { get; }

        public BaseAttribute()
        {
            AttributeType = this.GetType();
        }
    }

    /// <summary>
    /// 系统驱动系统标识
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple =true)]
    public class ObjectSystemAttribute : BaseAttribute
    {
    }
}
