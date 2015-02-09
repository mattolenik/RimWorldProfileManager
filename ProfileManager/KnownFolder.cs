using System;
using System.IO;
using System.Runtime.InteropServices;

namespace ProfileManager
{
    public static class KnownFolder
    {
        private static string _ludeonStudiosDirectory;
        private static string _profilesDirectory;

        public static string LudeonStudiosDirectory => _ludeonStudiosDirectory ??
                                                       (_ludeonStudiosDirectory =
                                                           Path.Combine(
                                                               GetKnownFolderPath(KnownFolders.AppDataLocalLow),
                                                               "Ludeon Studios"));

        public static string ProfilesDirectory => _profilesDirectory ??
                                                  (_profilesDirectory = Path.Combine(LudeonStudiosDirectory, "Profiles"))
            ;

        public static string GetKnownFolderPath(Guid knownFolderId)
        {
            var pszPath = IntPtr.Zero;
            try
            {
                var hr = SHGetKnownFolderPath(knownFolderId, 0, IntPtr.Zero, out pszPath);
                if (hr >= 0)
                    return Marshal.PtrToStringAuto(pszPath);
                throw Marshal.GetExceptionForHR(hr);
            }
            finally
            {
                if (pszPath != IntPtr.Zero)
                    Marshal.FreeCoTaskMem(pszPath);
            }
        }

        [DllImport("shell32.dll")]
        private static extern int SHGetKnownFolderPath([MarshalAs(UnmanagedType.LPStruct)] Guid rfid, uint dwFlags,
            IntPtr hToken, out IntPtr pszPath);

        public class KnownFolders
        {
            public static Guid AppDataLocalLow = new Guid("A520A1A4-1780-4FF6-BD18-167343C5AF16");
        }
    }
}