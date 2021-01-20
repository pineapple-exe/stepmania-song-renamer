using StepManiaSongRenamerLibrary;
using System;
using System.IO;
using System.Threading.Tasks;

namespace StepManiaSongRenamer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            bool noFolder = true;

            foreach(string path in args)
            {
                if (Directory.Exists(path))
                {
                    await StepmaniaSongEditor.FindSongFolders(path);
                    noFolder = false;
                }
            }
            if (noFolder)
            {
                Console.WriteLine("Takes only folders as input.");
            }
            Console.ReadKey();
        }
    }
}
