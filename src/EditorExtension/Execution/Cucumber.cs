﻿using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Meerkatalyst.Lonestar.EditorExtension.ResultAdapter;
using Meerkatalyst.Lonestar.EditorExtension.ResultAdapter.ResultModels;

namespace Meerkatalyst.Lonestar.EditorExtension.Execution
{
    public class Cucumber : IResultsProvider
    {
        private const string FORMATTER_FILE_NAME = "basic_info.rb";
        private const string FORMATTERS_DIRECTORY = "Formatters";
        private const string FORMATTER_CLASS_NAME = "Meerkatalyst::Lonestar::BasicInfo";
        public string FeatureFile { get; set; }

        public string StatusMessage
        {
            get { return string.Format("Executing Cucumber for file {0}", FeatureFile); }
        }

        public Cucumber(string featureFile)
        {
            FeatureFile = featureFile;
        }

        public string Execute()
        {
            string result = Start(GetRubyInterpreter(), GetArguments());

            return result;
        }

        public List<FeatureResult> ConvertResult(string result)
        {
            ConvertOutputToObjectModel converter = new ConvertOutputToObjectModel();
            return converter.Convert(result);
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
            return string.Format("{0} {1} {2}", GetCucumberCommand(), GetFormatter(), GetRequirePathAndFeatureFile());
        }

        private string GetCucumberCommand()
        {
            return @"C:\Ruby\bin\cucumber";
        }

        private string GetFormatter()
        {
            string directory = Path.GetDirectoryName(Assembly.GetAssembly(typeof(Cucumber)).Location);
            string fullPathToFormatter = Path.Combine(directory, Path.Combine(FORMATTERS_DIRECTORY, FORMATTER_FILE_NAME));

            return string.Format("--require \"{0}\" --format {1}", fullPathToFormatter, FORMATTER_CLASS_NAME);
        }

        private string GetRequirePathAndFeatureFile()
        {
            string directoryName = Path.GetDirectoryName(FeatureFile);
            return string.Format("--require \"{0}\" \"{1}\"", directoryName, FeatureFile);
        }
    }
}
