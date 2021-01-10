using System;
using System.IO;

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
                Console.WriteLine(ssf.TitleWasModified(ssf.AddInformativeTitle()));
            }
            Console.WriteLine("Done.");
        }
    }
}
