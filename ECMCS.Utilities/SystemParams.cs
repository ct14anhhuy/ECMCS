namespace ECMCS.Utilities
{
    public static class SystemParams
    {
        public const string FILE_PATH_ROOT = @"C:\FileCS\";
        public const string FILE_PATH_MONITOR = @"Monitor\";
        public const string FILE_PATH_LOG = @"logs\";
        public const string DRM_LOG_FILE_X64 = @"C:\Program Files (x86)\Fasoo DRM\Log\f_5500000.log";
        public const string DRM_LOG_FILE_X32 = @"C:\Program Files\Fasoo DRM\Log\f_5500000.log";
        public const string SYNC_FILE_PATH = @"C:\Sync-To-ECM";
        public const string JSON_FILES = "data.json";
        public const string JSON_USERS = "users.json";
        public const string PROTOCOL_NAME = "ECMProtocol";
        public const string API_URL = Env.IS_DEVELOPMENT ? "https://localhost:44372/api" : "http://172.25.216.127:8082/api";
        public const string WEB_URL = Env.IS_DEVELOPMENT ? "http://localhost:3000" : "http://172.25.216.127:8081";
        public const bool HIDDEN_FOLDER = false;
    }
}