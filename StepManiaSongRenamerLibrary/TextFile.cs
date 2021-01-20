using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace StepManiaSongRenamerLibrary
{
    class TextFile
    {
        protected string Path { get; set; }
        protected string[] Content { get; set; }

        public TextFile(string path)
        {
            Path = path;
        }

        public async Task Load()
        {
            Content = await File.ReadAllLinesAsync(Path);
        }
    }
}
