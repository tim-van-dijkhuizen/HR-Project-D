namespace FitbyteServer.Base {

    public class DatabaseSettings : IDatabaseSettings {
        
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public bool EnableSsl { get; set; }

    }

    public interface IDatabaseSettings {

        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
        bool EnableSsl { get; set; }

    }

}
