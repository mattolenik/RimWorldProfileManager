using System;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ProfileManager
{
    internal static class ProfileManager
    {
        private const string FileVersion = "Version 1";

        static ProfileManager()
        {
            var save = false;
            string version = null;
            try
            {
                var json = File.ReadAllText(ProfileStorePath);
                var index = json.IndexOf(Environment.NewLine, StringComparison.Ordinal);
                version = json.Substring(0, index);
                json = json.Substring(index + Environment.NewLine.Length);
                Profiles = JsonConvert.DeserializeObject<DataTable>(json, new DataTableConverter());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Profiles = new DataTable("");
                save = true;
            }
            // Version 0 to version 1.
            if (string.IsNullOrEmpty(version))
            {
                Profiles.Columns.Add("ProfileName", typeof (string));
                Profiles.Columns.Add("ProfileDir", typeof (string));
                Profiles.Columns.Add("RimWorldDir", typeof (string));
                Profiles.Columns.Add("RimWorldExe", typeof (string));
                save = true;
            }
            if (save) Save();
            Profiles.Constraints.Add(new UniqueConstraint(Profiles.Columns["ProfileName"]));
            Profiles.RowChanged += (o, args) => Save();
        }

        public static DataTable Profiles { get; }
        public static string ProfileStorePath => Path.Combine(KnownFolder.ProfilesDirectory, "profiles.dat");

        private static void Save()
        {
            var json = JsonConvert.SerializeObject(Profiles, new DataTableConverter());
            json = FileVersion + Environment.NewLine + json;
            File.WriteAllText(ProfileStorePath, json);
        }

        public static string CreateProfileDirFromName(string name)
        {
            var regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            var r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            return r.Replace(name, "");
        }
    }
}