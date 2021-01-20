using System;
using System.IO;
using System.Threading.Tasks;

namespace StepManiaSongRenamerLibrary
{
    public static class StepmaniaSongEditor
    {
        public static async Task FindSongFolders(string path)
        {
            string[] smFiles = Directory.GetFiles(path, "*.sm", SearchOption.AllDirectories);

            foreach (string file in smFiles)
            {
                StepmaniaSongFile ssf = new StepmaniaSongFile(file);
                await ssf.Load();
                Console.WriteLine(ssf.TitleWasModified(await ssf.AddInformativeTitle()));
            }
            Console.WriteLine("Done.");
        }
    }
}
