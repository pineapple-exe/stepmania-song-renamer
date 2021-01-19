using System.IO;
using System.Linq;

namespace StepManiaSongRenamerLibrary
{
    class TextFile
    {
        protected string Path { get; set; }
        protected string[] Content { get; set; }

        public TextFile(string path)
        {
            Path = path;
            Content = File.ReadAllLines(path).ToArray();
        }
    }
}
