using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProfileManager
{
    class DirectoryHelper
    {
        public static void CreateRecursive(string directory)
        {
            var di = new DirectoryInfo(directory);
            var parent = di.Parent;
            if (parent != null)
                CreateRecursive(parent.FullName);
            if (di.Exists) return;
            try
            {
                di.Create();
            }
            catch { }
        }
    }
}
