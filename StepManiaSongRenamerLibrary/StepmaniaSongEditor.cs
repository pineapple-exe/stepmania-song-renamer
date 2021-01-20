using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace StepManiaSongRenamerLibrary
{
    public static class StepmaniaSongEditor
    {
        public static async Task FindSongFolders(string path)
        {
            string[] smFilesPaths = Directory.GetFiles(path, "*.sm", SearchOption.AllDirectories);

            IEnumerable<Task> tasks = smFilesPaths.Select(async file =>
            {
                StepmaniaSongFile ssf = new StepmaniaSongFile(file);
                await ssf.Load();
                Console.WriteLine(ssf.TitleWasModified(await ssf.AddInformativeTitle()));
            }
            );
            await Task.WhenAll(tasks);
            Console.WriteLine("Done.");
        }
    }
}
