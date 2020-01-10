namespace SqlGenerator
{
    public enum GeneratorType
    {
        Colum2Class = 1,
        Insert = 2,
        Update = 3,
    }

    public abstract class BaseGenerator
    {
        public abstract void GenerateCode(ColumInfo columInfo);
    }
}
