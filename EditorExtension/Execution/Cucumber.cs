using System.Data;
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
            string result = Start(GetRubyInterpreter(), GetArguments());

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
            StreamReader reader = process.StandardOutput;
            StreamReader errorReader = process.StandardError;
            process.WaitForExit();
            string result = reader.ReadToEnd();
            string error = errorReader.ReadToEnd();
            if(!string.IsNullOrEmpty(error))
                throw new EvaluateException(error);

            return result;
        }

        private Process CreateProcess(string command, string arguments)
        {
            Process process = new Process();
            process.StartInfo = new ProcessStartInfo(command, arguments);
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.UseShellExecute = false;
            return process;
        }


        private string GetRubyInterpreter()
        {
            return @"C:\Ruby\bin\ruby";
        }

        public string GetArguments()
        {
            string command = GetCucumberCommand() + " " + GetFormatter() + " " + GetRequirePathAndFeatureFile();

            return command;
        }

        private string GetCucumberCommand()
        {
            return @"C:\Ruby\bin\cucumber";
        }

        private string GetFormatter()
        {
            string directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string fullPathToFormatter = Path.Combine(directory, PATH_TO_FORMATTER);

            return "--require \"" + fullPathToFormatter + "\" --format " + FORMATTER_NAME;
        }

        private string GetRequirePathAndFeatureFile()
        {
            string path = Path.GetDirectoryName(FeatureFile);
            return string.Format("--require \"{0}\" \"{1}\"", path, FeatureFile);
        }
    }
}
