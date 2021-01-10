using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

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
                    return double.Parse(BPMsInitial[1].Remove(BPMsInitial.Length - 1), new CultureInfo("en-US"));
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

        public bool AddInformativeTitle()
        {
            bool titleModified = false;

            for (int i = 0; i < Content.Length; i++)
            {
                if (Content[i].StartsWith("#TITLE:"))
                {
                    Content[i] = Content[i].Replace(Content[i], $"#TITLE:[{ Difficulty }] [{ BPM }] { Title };");
                    File.WriteAllLines(Path, Content);
                    titleModified = true;
                    break;
                }
            }
            return titleModified;
        }

        public string TitleWasModified(bool yes)
        {
            if (yes)
            {
                return Title;
            }
            else
            {
                return "Failed to add informative title.";
            }
        }
    }
}
