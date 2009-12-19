using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Meerkatalyst.Lonestar.EditorExtension.Execution
{
    public class Cucumber
    {
        private const string PATH_TO_FORMATTER = "basic_info.rb";
        private const string FORMATTER_NAME = "Meerkatalyst::Lonestar::BasicInfo";
        public string FeatureFile { get; set; }

        public Cucumber(string featureFile)
        {
            FeatureFile = featureFile;
        }

        public string Execute()
        {
            string result = Start(@"C:\Ruby\bin\cucumber.bat", GetArguments());

            return result;
        }

        private string Start(string command, string arguments)
        {
            Process process = CreateProcess(command, arguments);
            process.Start();
            
            string result = GetResult(process);
            return result;
        }

        private string GetResult(Process process)
        {
            string result;
            using (StreamReader reader = process.StandardOutput)
            {
                process.WaitForExit();
                result = reader.ReadToEnd();
            }
            return result;
        }

        private Process CreateProcess(string command, string arguments)
        {
            Process process = new Process();
            process.StartInfo = new ProcessStartInfo(command, arguments);
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            return process;
        }

        public string GetArguments()
        {
            string directory = Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName;
            string fullPathToFormatter = Path.Combine(directory, PATH_TO_FORMATTER);
            string command = "--require \"{0}\" --format {1} \"{2}\"";

            return string.Format(command, fullPathToFormatter, FORMATTER_NAME, FeatureFile);
        }
    }
}
