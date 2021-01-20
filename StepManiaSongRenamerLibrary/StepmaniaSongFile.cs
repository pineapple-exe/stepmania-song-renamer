using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StepManiaSongRenamerLibrary
{
    enum Level
    {
        Beginner,
        Easy,
        Medium,
        Hard,
        Challenge
    }

    enum TitleStatus
    {
        Nothing,
        Modified,
        AlreadyProcessed
    }

    class StepmaniaSongFile : TextFile
    {
        private static Level lvl = Level.Challenge;
        private static readonly Regex difficultyPattern = new Regex($"(?<=#NOTES:\\s+dance-single:\\s+.*\\s+{lvl}:\\s+)\\d+(?=:)");

        private int Difficulty
        { 
            get
            {
                string contentJoined = String.Join("", Content);
                while (!difficultyPattern.IsMatch(contentJoined))
                {
                    lvl--;
                }
                return int.Parse(difficultyPattern.Match(contentJoined).Value);
            }
        }

        private double BPM 
        { 
            get
            {
                if (Content.Any(line => line.Contains("#DISPLAYBPM")))
                {
                    string displayBPMLine = Content.Where(s => s.StartsWith("#DISPLAYBPM")).First();
                    string displayBPMWithSemicolon = displayBPMLine.Split(':')[1];
                    return double.Parse(displayBPMWithSemicolon.Remove(displayBPMWithSemicolon.Length - 1), new CultureInfo("en-US"));
                }
                else
                {
                    string BPMsLine = Content.Where(s => s.StartsWith("#BPMS")).First();
                    string[] BPMsLineSplit = BPMsLine.Split(':', BPMsLine.Length - 7);
                    string[] BPMsShifts = BPMsLineSplit[1].Split(',');
                    string[] BPMsInitial = BPMsShifts[0].Split('=');
                    return double.Parse(BPMsInitial[1].Remove(BPMsInitial[1].Length - 1), new CultureInfo("en-US"));
                }
            }
        }

        private string Title
        {
            get
            {
                string titleLine = Content.Where(s => s.StartsWith("#TITLE")).First();
                string titleSemicolon = titleLine.Remove(0, 7);
                return titleSemicolon.Remove(titleSemicolon.Length - 1);
            }
        }

        public StepmaniaSongFile(string path) : base(path)
        {

        }

        public bool AlreadyProcessed(string titleRow)
        {
            if (titleRow[7..].StartsWith($"[{ Difficulty }] [{ BPM }]"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<TitleStatus> AddInformativeTitle()
        {
            TitleStatus state = TitleStatus.Nothing;

            for (int i = 0; i < Content.Length; i++)
            {
                if (Content[i].StartsWith("#TITLE:"))
                {
                    if (!AlreadyProcessed(Content[i]))
                    {
                        Content[i] = Content[i].Replace(Content[i], $"#TITLE:[{ Difficulty }] [{ BPM }] { Title };");
                        await File.WriteAllLinesAsync(Path, Content);
                        state = TitleStatus.Modified;
                        break;
                    }
                    else
                    {
                        state = TitleStatus.AlreadyProcessed;
                    }
                }
            }
            return state;
        }

        public string TitleWasModified(TitleStatus state)
        {
            if (state == TitleStatus.Nothing)
            {
                return "Failed to add informative title.";
            }
            else if (state == TitleStatus.Modified)
            {
                return Title;
            }
            else
            {
                return "This title has already been modified once.";
            }
        }
    }
}
