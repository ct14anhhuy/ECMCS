#define DEVELOPMENT

namespace ECMCS.Utilities
{
    public static class SystemParams
    {
#if DEVELOPMENT
        public const string FILE_PATH_ROOT = @"C:\FileCS\";
        public const string FILE_PATH_MONITOR = @"Monitor\";
        public const string FILE_PATH_LOG = @"logs\";
        public const string DRM_LOG_FILE_X64 = @"C:\Program Files (x86)\Fasoo DRM\Log\f_5500000.log";
        public const string DRM_LOG_FILE_X32 = @"C:\Program Files\Fasoo DRM\Log\f_5500000.log";
        public const string SYNC_FILE_PATH = @"C:\Sync-To-ECM";
        public const string JSON_FILES = "data.json";
        public const string JSON_USERS = "users.json";
        public const string PROTOCOL_NAME = "ECMProtocol";
        public const string API_URL = "https://localhost:44372/api";
        public const string WEB_URL = "https://localhost:44372";
#else
        public const string FILE_PATH_ROOT = @"C:\FileCS\";
        public const string FILE_PATH_MONITOR = @"Monitor\";
        public const string FILE_PATH_LOG = @"logs\";
        public const string DRM_LOG_FILE_X64 = @"C:\Program Files (x86)\Fasoo DRM\Log\f_5500000.log";
        public const string DRM_LOG_FILE_X32 = @"C:\Program Files\Fasoo DRM\Log\f_5500000.log";
        public const string SYNC_FILE_PATH = @"C:\Sync-To-ECM";
        public const string JSON_FILES = "data.json";
        public const string JSON_USERS = "users.json";
        public const string PROTOCOL_NAME = "ECMProtocol";
        public const string API_URL = "http://172.25.216.127:8081/api";
        public const string WEB_URL = "http://172.25.216.127:8081";
#endif
    }
}