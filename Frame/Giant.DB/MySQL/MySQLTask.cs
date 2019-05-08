namespace Giant.DB.MySQL
{
    public abstract class MySQLTask<T> : DBTask<T>
    {
        public string TableName { get; set; }

        public MySQLService Service
        {
            get { return this.DBService.Service as MySQLService; }
        }
    }
}
