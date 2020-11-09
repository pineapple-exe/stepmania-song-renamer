using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace StepManiaSongRenamerLibrary
{
    public static class StepmaniaSongEditor
    {
        public static void FindSongFolders(string path)
        {
            string[] smFiles = Directory.GetFiles(path, "*.sm", SearchOption.AllDirectories);

            foreach (string file in smFiles)
            {
                StepmaniaSongFile ssf = new StepmaniaSongFile(file);
                ssf.PrintSong();
            }
        }
    }
}
