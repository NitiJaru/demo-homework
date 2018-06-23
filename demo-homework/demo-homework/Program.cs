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
            // Example: Total Test: 10, Pass: 10, Fail: 0, Skip: 0
            //const string gitUrl = @"https://github.com/taninpong/DemoTestforAcademy-true";

            // Example: Total Test: 10, Pass: 1, Fail: 9, Skip: 0
            //const string gitUrl = @"https://github.com/taninpong/DemoTestforAcademy-fail";

            // Example: Total Test: 0, Pass: 0, Fail: 0, Skip: 0
            const string gitUrl = @"https://github.com/taninpong/DemoTestforAcademy-notest";

            var tester = new GitTestResult();
            var result = tester.GetTestResult(gitUrl);

            //var result = GetTestResult(gitUrl);
            Console.WriteLine("-------------------------------------");
            Console.WriteLine(
                $"[Result]" +
                $"\nUsername: {result.GitUsername}" +
                $"\nTotal Test: {result.TotalTest}" +
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
        /// <param name="githubUrl">GitHub url</param>
        /// <param name="testProjectPath">Test project path</param>
        /// <returns>result from test</returns>
        private static HomeworkTestResult GetTestResult(string githubUrl, string testProjectPath = "")
        {
            // Validate GitHub's url
            if (string.IsNullOrEmpty(githubUrl) || string.IsNullOrWhiteSpace(githubUrl))
            {
                Console.WriteLine("Can not find any url. did you missing?");
                return new HomeworkTestResult();
            }

            // Finding Directory's name and FullDirectory's name from url
            Console.WriteLine("> Creating directory's name. Please wait.");
            var urlSplited = githubUrl.ToLower().Split('/');
            var directoryIndex = urlSplited.Count();
            var directoryProjectNameIndex = directoryIndex - 1;
            var directoryNameIndex = directoryIndex - 2;

            var directoryName = urlSplited[directoryNameIndex];
            var fullDirectoryName = $"{urlSplited[directoryNameIndex]}/{urlSplited[directoryProjectNameIndex]}";
            Console.WriteLine("> Create directory's name was completed.");

            // Prepare Git clone commands
            var createDirectoryCommand = $"md {directoryName}";
            var navigateToDirectoryWasCreated = $"cd {directoryName}";
            var cloneCommand = $"git clone {githubUrl}";

            var commands =
                $"{createDirectoryCommand}" +
                $"&{navigateToDirectoryWasCreated}" +
                $"&{cloneCommand}";

            // Process Git clone commands
            Console.WriteLine($"> Process Git clone project {directoryName} is starting. Please wait.");
            Process.Start(new ProcessStartInfo("cmd", $"/c {commands}") { UseShellExecute = false }).WaitForExit();
            Console.WriteLine("> Process Git clone is end. Git clone was completed.");

            // Prepare test commands
            var TestProjectUrl = !(string.IsNullOrEmpty(testProjectPath) || string.IsNullOrWhiteSpace(testProjectPath)) ?
                 testProjectPath : "Training.TestDrivenDevelopement/TDD.TestProject";
            var navigateToTestProjectCommand = $"cd {fullDirectoryName}/{TestProjectUrl}";
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
