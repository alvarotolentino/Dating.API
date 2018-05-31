namespace Dating.API.Repository
{
    public class BaseRepository
    {
        protected readonly string _connectionString;
        public BaseRepository()
        {
            _connectionString = "Data Source=dating.db";
        }
    }
}