using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demo_homework
{
    class Program
    {
        static void Main(string[] args)
        {
            const string gitUrl = @"https://github.com/taninpong/DemoTestforAcademy-true";

            var result = GetTestResult(gitUrl);
            Console.WriteLine(
                $"Username: {result.Username}" +
                $"\nTest: {result.TotalTest}" +
                $"\nPass:{result.Pass}" +
                $"\tFail: {result.Fail}" +
                $"\tSkip: {result.Skip}");
        }

        /// <summary>
        /// Git clone (Example)
        /// </summary>
        private static void GitClone()
        {
            // Prepare url
            const string gitUrl = @"https://github.com/taninpong/DemoTestforAcademy-true";

            // Find Directory name from URL
            const int githubIndexCount = 11;
            var githubIndex = gitUrl.LastIndexOf("github.com");
            var resultSubstring = gitUrl.Substring(githubIndex + githubIndexCount);
            var resultSplited = resultSubstring.Split('/');
            var directoryName = resultSplited.FirstOrDefault().ToLower();

            // Prepare commands
            var navigateToRootCommand = "cd /";
            var navigateToGitCommand = "cd git";
            var createDirectoryCommand = $"md {directoryName}";
            var navigateToDirectoryWasCreated = $"cd {directoryName}";
            var cloneCommand = $"git clone {gitUrl}";
            var command =
                $"{navigateToRootCommand}" +
                $"&{navigateToGitCommand}" +
                $"&{createDirectoryCommand}" +
                $"&{navigateToDirectoryWasCreated}" +
                $"&{cloneCommand}";

            // Process git clone commands
            Console.WriteLine($"Process is starting to clone project {directoryName}\n");
            Process.Start(new ProcessStartInfo("cmd", $"/c {command}") { UseShellExecute = false }).WaitForExit();
            Console.WriteLine("\nProcess is end. Clone was completed");
        }

        /// <summary>
        /// Read data from test (Example)
        /// </summary>
        /// <returns>result from test</returns>
        private static HomeworkTestResult TestResult()
        {
            // Prepare commands
            var navigateToRootCommand = "cd /";
            var navigateToGitCommand = "cd Git/taninpong/DemoTestforAcademy-true/Training.TestDrivenDevelopement/TDD.TestProject";
            var command =
                $"{navigateToRootCommand}" +
                $"&{navigateToGitCommand}" +
                $"&dotnet test";

            // process test command
            var process = new Process
            {
                StartInfo = new ProcessStartInfo("cmd", $"/c {command}") { UseShellExecute = false, RedirectStandardOutput = true }
            };
            process.Start();

            // Get data from test
            var data = process.StandardOutput.ReadToEnd();

            // HACK: Fix username
            const string username = "taninpong";

            // Convert to HomeworkTestResult model
            var convertor = new HomeworkResultConverter();
            var result = convertor.GetHomeworkResult(username, data);
            return result;
        }

        /// <summary>
        /// Get data from test
        /// </summary>
        /// <param name="url">github url</param>
        /// <returns>result from test</returns>
        private static HomeworkTestResult GetTestResult(string url)
        {
            // Find Directory's name from URL
            const int githubIndexCount = 11;
            var githubIndex = url.LastIndexOf("github.com");
            var resultSubstring = url.Substring(githubIndex + githubIndexCount);
            var resultSplited = resultSubstring.Split('/');
            var directoryName = resultSplited.FirstOrDefault().ToLower();
            
            // Prepare Git clone commands
            var navigateToRootCommand = "cd /";
            var navigateToGitCommand = "cd git";
            var createDirectoryCommand = $"md {directoryName}";
            var navigateToDirectoryWasCreated = $"cd {directoryName}";
            var cloneCommand = $"git clone {url}";

            var commands =
                $"{navigateToRootCommand}" +
                $"&{navigateToGitCommand}" +
                $"&{createDirectoryCommand}" +
                $"&{navigateToDirectoryWasCreated}" +
                $"&{cloneCommand}";

            // Process Git clone commands
            Console.WriteLine($"Process is starting to clone project {directoryName}\n");
            Process.Start(new ProcessStartInfo("cmd", $"/c {commands}") { UseShellExecute = false }).WaitForExit();
            Console.WriteLine("\nProcess is end. Clone was completed");
            
            // Prepare test commands
            var navigateToTestProjectCommand = $"cd Git/{directoryName}/DemoTestforAcademy-true/Training.TestDrivenDevelopement/TDD.TestProject";
            commands =
                $"{navigateToRootCommand}" +
                $"&{navigateToTestProjectCommand}" +
                $"&dotnet test";

            // Process test command
            var process = new Process
            {
                StartInfo = new ProcessStartInfo("cmd", $"/c {commands}") { UseShellExecute = false, RedirectStandardOutput = true }
            };
            process.Start();

            // Get data from test
            var data = process.StandardOutput.ReadToEnd();
            
            // Convert to HomeworkTestResult model
            var convertor = new HomeworkResultConverter();
            var result = convertor.GetHomeworkResult(directoryName, data);
            return result;

        }

    }
}
