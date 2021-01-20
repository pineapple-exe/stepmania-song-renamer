using StepManiaSongRenamerLibrary;
using System.Threading.Tasks;

namespace StepManiaSongRenamer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await StepmaniaSongEditor.FindSongFolders(@"c:\Games\StepMania 5\Songs\Mandodo's Winter Love Pack\");
        }
    }
}
