using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demo_homework
{
    public interface IGitTester
    {
        HomeworkTestResult GetTestResult(string url);
        HomeworkTestResult GetTestResult(string url, string projectPath);
    }

    public class GitTestResult : IGitTester
    {
        /// <summary>
        /// Get result from test
        /// </summary>
        /// <param name="url">url for clone project</param>
        /// <returns>Result from test</returns>
        public HomeworkTestResult GetTestResult(string url)
        {
            const string projectPath = @"Training.TestDrivenDevelopement/TDD.TestProject";
            return GetTestResult(url, projectPath);
        }

        /// <summary>
        /// Get result from test
        /// </summary>
        /// <param name="url">url for clone project</param>
        /// <param name="projectPath">Project test path</param>
        /// <returns>Result from test</returns>
        public HomeworkTestResult GetTestResult(string url, string projectPath)
        {
            // Validate url
            var validateUrl = !string.IsNullOrEmpty(url) && !string.IsNullOrWhiteSpace(url);
            if (!validateUrl)
            {
                Console.WriteLine("Url cannot be empty.");
                return new HomeworkTestResult();
            }

            // Validate ProjectPath
            var validateProjectPath = !string.IsNullOrEmpty(projectPath) && !string.IsNullOrWhiteSpace(projectPath);
            if (!validateProjectPath)
            {
                Console.WriteLine("Project path cannot be empty.");
                return new HomeworkTestResult();
            }

            // Finding Directory's name and FullDirectory's name from url
            Console.WriteLine("> Creating directory's name. Please wait.");
            var urlSplited = url.ToLower().Split('/');
            var directoryIndex = urlSplited.Count();
            var directoryProjectNameIndex = directoryIndex - 1;
            var directoryNameIndex = directoryIndex - 2;

            var directoryName = urlSplited[directoryNameIndex];
            var fullDirectoryName = $"{urlSplited[directoryNameIndex]}/{urlSplited[directoryProjectNameIndex]}";
            Console.WriteLine("> Create directory's name was completed.");

            // Prepare Git clone commands
            var createDirectoryCommand = $"md {directoryName}";
            var navigateToDirectoryWasCreated = $"cd {directoryName}";
            var cloneCommand = $"git clone {url}";
            var commands =
                $"{createDirectoryCommand}" +
                $"&{navigateToDirectoryWasCreated}" +
                $"&{cloneCommand}";

            // Process Git clone commands
            Console.WriteLine($"> Process Git clone project {directoryName} is starting. Please wait.");
            Process.Start(new ProcessStartInfo("cmd", $"/c {commands}") { UseShellExecute = false }).WaitForExit();
            Console.WriteLine("> Process Git clone is end. Git clone was completed.");

            // Prepare test commands
            var navigateToTestProjectCommand = $"cd {fullDirectoryName}/{projectPath}";
            commands =
                $"{navigateToTestProjectCommand}" +
                $"&dotnet test";

            // Process test command
            Console.WriteLine("> Process analysis is starting. Please wait.");
            var process = new Process
            {
                StartInfo = new ProcessStartInfo("cmd", $"/c {commands}") { UseShellExecute = false, RedirectStandardOutput = true }
            };
            process.Start();

            // Get data from test
            var data = process.StandardOutput.ReadToEnd();
            Console.WriteLine("> Process analysis was completed.");

            // Convert to HomeworkTestResult model
            var convertor = new HomeworkResultConverter();
            var result = convertor.GetHomeworkResult(directoryName, data);
            return result;
        }
    }
}
