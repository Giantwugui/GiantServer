using System;
using System.Reflection;
using System.Diagnostics;
using System.Threading;

namespace OODB
{
    public class ReflectionTools
    {
        public static string GetSetMethodName(int skipFrame)
        {
            MethodBase method = new StackFrame(skipFrame+1).GetMethod(); // 这里忽略1层堆栈，也就忽略了当前方法GetMethodName，这样拿到的就正好是外部调用GetMethodName的方法信息
            string methodName =  method.Name;
            

            if (methodName.Length < 4||
                methodName[0]!='s'||
               methodName[1]!='e'||
                methodName[2]!='t'||
                methodName[3]!='_'
                ) 
                return methodName;

            string re = methodName.Substring(4, methodName.Length - 4);
            return re; 
        }

        public static string GetGetMethodName(int skipFrame)
        {
            MethodBase method = new StackFrame(skipFrame + 1).GetMethod(); // 这里忽略1层堆栈，也就忽略了当前方法GetMethodName，这样拿到的就正好是外部调用GetMethodName的方法信息
            string methodName = method.Name;
 

            if (methodName.Length < 4 ||
                methodName[0] != 'g' ||
               methodName[1] != 'e' ||
                methodName[2] != 't' ||
                methodName[3] != '_'
                )
                return methodName;

            string re = methodName.Substring(4, methodName.Length - 4);
            return re;
        }

 
        public static object ChangeType(object obj, Type conversionType)
        {
            return ChangeType(obj, conversionType, Thread.CurrentThread.CurrentCulture);
        }

        public static object ChangeType(object obj, Type conversionType, IFormatProvider provider)
        { 
            Type nullableType = Nullable.GetUnderlyingType(conversionType);
            if (nullableType != null)
            {
                if (obj == null) return null; 

                return Convert.ChangeType(obj, nullableType, provider);
            } 

            if (typeof(System.Enum).IsAssignableFrom(conversionType)) 
                return Enum.Parse(conversionType, obj.ToString());

            return Convert.ChangeType(obj, conversionType, provider);
        } 
    }

}
