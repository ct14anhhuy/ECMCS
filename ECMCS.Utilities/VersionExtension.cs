using System;

namespace ECMCS.Utilities
{
    public static class VersionExtension
    {
        public static Version IncrementMinor(this Version version)
        {
            int nextMinor = version.Minor + 1;
            if (nextMinor > 9)
            {
                return IncrementMajor(version.Major);
            }
            return new Version(version.Major, nextMinor);
        }

        private static Version IncrementMajor(int major)
        {
            return new Version(major + 1, 0);
        }
    }
}