using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GunayKfzSystems.Shell
{
    internal static class ModuleHandler
    {
        #region Private Fields

        private static readonly List<string> AlreadyLoadedDlls = new List<string>();


        #endregion Private Fields

        #region Internal Methods

        internal static List<string> GetNotLoadedFiles(string subDir)
        {

            var dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, subDir);

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            var dlls = Directory.GetFiles(dir, "*.dll", SearchOption.TopDirectoryOnly);

            var notAddedDlls = new List<string>();
            foreach (var dll in dlls)
            {
                var fInfo = new FileInfo(dll);
                if (AlreadyLoadedDlls.Contains(fInfo.Name))
                {
                    continue;
                }
                notAddedDlls.Add(dll);
                AlreadyLoadedDlls.Add(fInfo.Name);
            }

            return notAddedDlls;
        }

        #endregion Internal Methods
    }
}
