using System;

namespace ECMCS.Utilities
{
    public static class VersionExtension
    {
        private const int MAX_MINOR = 9;

        public static Version IncrementMinor(this Version version)
        {
            if (version.Minor >= MAX_MINOR)
            {
                return IncrementMajor(version.Major);
            }
            return new Version(version.Major, version.Minor + 1);
        }

        private static Version IncrementMajor(int major)
        {
            return new Version(major + 1, 0);
        }
    }
}