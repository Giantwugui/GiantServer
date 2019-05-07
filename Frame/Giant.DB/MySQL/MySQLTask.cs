namespace Giant.DB.MySQL
{
    public abstract class MySQLTask<T> : DBTask<T>
    {
        public string TableName { get; set; }
    }
}
