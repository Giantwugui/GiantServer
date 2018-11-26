using System;
using MongoDB.Bson;

namespace OODB
{
    class OODBValueType
    {
        public static bool IsValueType(Type type)
        {  
            switch (type.Name)
            {
                case "String": 
                case "Int64":
                case "Int32":
                case "UInt32":
                case "Single":
                case "Byte[]":
                    return true;
                default:
                    return type.IsEnum?true:false;
            }
        }

        public static bool IsValueGroupType(Type type)
        {
            return typeof(OOValueGroup).IsAssignableFrom(type);
        }

        public static BsonValue ToBsonValue(Object obj)
        {
            Type tp = obj.GetType();
            if(tp.IsEnum) 
                tp = Enum.GetUnderlyingType(tp);//取得基础类型
 
            switch (tp.Name)
            {
                case "String":
                    return (BsonValue)(string)obj;
                case "Int64":
                    return (BsonValue) Convert.ToInt64(obj);
                case "Int32":
                    return (BsonValue)Convert.ToInt32(obj);
                case "UInt32":
                    return (BsonValue)Convert.ToUInt32(obj);
                case "Single":
                    return (BsonValue)(double)Convert.ToSingle(obj);
                case "Byte[]":
                    return (BsonValue)(byte[])obj;
                default:
                    return null;
            };      
        }

        public static object FromBsonValue(Type typeType,BsonValue bv)
        {
            Type tp =typeType;
            if (tp.IsEnum)
                tp = Enum.GetUnderlyingType(tp);//取得基础类型

            switch (tp.Name)
            {
                case "String": 
                        return  (String)bv; 
                case "Int64":
                    return  (long)bv;
                case "Int32":
                    return  (int)bv;
                case "UInt32":
                    return  (UInt32)bv;
                case "Single":
                    return  (float)(double)bv;
                case "Byte[]":
                    return (byte[])bv;
                default:
                    return null;
            };
        }

        public static Object GetFieldValue(
                string fieldName, 
                Type TValueType,
                MemValues<string> strFields,
                MemValues<long>  longFields,
                MemValues<Int32> int32Fields,
                MemValues<UInt32> uint32Fields,
                MemValues<float> singleFields,
                MemValues<byte[]> byteArrayFields
            )
        {
             Type tp = TValueType;
             if (tp.IsEnum) tp = Enum.GetUnderlyingType(tp);//取得基础类型

             switch (tp.Name)
            {
                case "String":
                    {
                        if(strFields._FieldValues.ContainsKey(fieldName))
                            return strFields._FieldValues[fieldName];
                    }
                    break;
                case "Int64":
                    {
                        if (longFields._FieldValues.ContainsKey(fieldName))
                            return longFields._FieldValues[fieldName]; 
                    }
                    break;
                case "Int32":
                    {
                        if (int32Fields._FieldValues.ContainsKey(fieldName))
                        return int32Fields._FieldValues[fieldName]; 
                    }
                    break;
                case "UInt32":
                    {
                        if (uint32Fields._FieldValues.ContainsKey(fieldName))
                        return uint32Fields._FieldValues[fieldName];  
                    }
                    break;
                case "Single":
                    {
                        if (singleFields._FieldValues.ContainsKey(fieldName))
                        return singleFields._FieldValues[fieldName]; 
                    }
                    break;
                case "Byte[]":
                    {
                         if (byteArrayFields._FieldValues.ContainsKey(fieldName))
                             return byteArrayFields._FieldValues[fieldName]; 
                    }
                    break;
                default:
                    break;
            };

            return null;
        }

        public static void SetNeedChangedField(
                string fieldName,
                object v,
                MemValues<string> strFields,
                MemValues< long>  longFields,
                MemValues< Int32> int32Fields,
                MemValues< UInt32> uint32Fields,
                MemValues< float> singleFields,
                MemValues<byte[]> byteArrayFields
            )
        {
            Type tp = v.GetType();
            if (tp.IsEnum) tp = Enum.GetUnderlyingType(tp);//取得基础类型

            switch (tp.Name)
            {
                case "String":
                    {
                        if (OODBInstance.__WriteRecord&&!strFields._NeedUpdate.Contains(fieldName))
                            strFields._NeedUpdate.Add(fieldName);

                        string vv = (string)v;
                        if (strFields._FieldValues.ContainsKey(fieldName))
                            strFields._FieldValues[fieldName] = vv;
                        else
                            strFields._FieldValues.Add(fieldName, vv);
                    }
                    break;
                case "Int64":
                    {

                        if (OODBInstance.__WriteRecord && !longFields._NeedUpdate.Contains(fieldName))
                            longFields._NeedUpdate.Add(fieldName);

                        long vv = Convert.ToInt64(v);
                        if (longFields._FieldValues.ContainsKey(fieldName))
                            longFields._FieldValues[fieldName] = vv;
                        else
                            longFields._FieldValues.Add(fieldName, vv);
                    }
                    break;
                case "Int32":
                    {
                        if (OODBInstance.__WriteRecord && !int32Fields._NeedUpdate.Contains(fieldName))
                            int32Fields._NeedUpdate.Add(fieldName);

                        int vv = Convert.ToInt32(v);
                        if (int32Fields._FieldValues.ContainsKey(fieldName))
                            int32Fields._FieldValues[fieldName] = vv;
                        else
                            int32Fields._FieldValues.Add(fieldName, vv);
                    }
                    break;
                case "UInt32":
                    {
                        if (OODBInstance.__WriteRecord && !uint32Fields._NeedUpdate.Contains(fieldName))
                            uint32Fields._NeedUpdate.Add(fieldName);

                        uint vv = Convert.ToUInt32(v);
                        if (uint32Fields._FieldValues.ContainsKey(fieldName))
                            uint32Fields._FieldValues[fieldName] = vv;
                        else
                            uint32Fields._FieldValues.Add(fieldName, vv);
                    }
                    break;
                case "Single":
                    {
                        if (OODBInstance.__WriteRecord && !singleFields._NeedUpdate.Contains(fieldName))
                            singleFields._NeedUpdate.Add(fieldName);

                        float vv = Convert.ToSingle(v);
                        if (singleFields._FieldValues.ContainsKey(fieldName))
                            singleFields._FieldValues[fieldName] = vv;
                        else
                            singleFields._FieldValues.Add(fieldName, vv);
                    }
                    break;
                case "Byte[]":
                    {
                        if (OODBInstance.__WriteRecord && !byteArrayFields._NeedUpdate.Contains(fieldName))
                            byteArrayFields._NeedUpdate.Add(fieldName);

                        var vv = (byte[])v;
                        if (byteArrayFields._FieldValues.ContainsKey(fieldName))
                            byteArrayFields._FieldValues[fieldName] = vv;
                        else
                            byteArrayFields._FieldValues.Add(fieldName, vv); 
                    }
                    break;
            };
        }

        public static Object FromStringValue(Type valueType,String v)
        {
            Type tp = valueType;
            if (tp.IsEnum) tp = Enum.GetUnderlyingType(tp);//取得基础类型

            switch (tp.Name)
            {
                case "String":
                    return v;
                case "Int64":
                    return long.Parse(v);//DateTime.FromFileTime(long.Parse(v)); //((DateTime)obj).ToFileTime().ToString();
                case "Int32":
                    return int.Parse(v);
                case "UInt32":
                    return uint.Parse(v) ;
                case "Single":
                    return float.Parse(v);
                default:
                    return null;
            }
        }

        public static String ToStringValue(Object obj)
        {
            Type tp = obj.GetType();
            if (tp.IsEnum) tp = Enum.GetUnderlyingType(tp);//取得基础类型

            switch (tp.Name)
            {
                case "String":
                    return (String)obj;
                case "Int64":
                    return  Convert.ToInt64(obj).ToString();//((DateTime)obj).ToFileTime().ToString();
                case "Int32":
                    return Convert.ToInt32(obj).ToString();
                case "UInt32":
                    return Convert.ToUInt32(obj).ToString();
                case "Single":
                    return Convert.ToSingle(obj).ToString();
                default:
                    return null;
            }
        }
         
    }
}
