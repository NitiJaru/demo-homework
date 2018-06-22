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
            var result = TestResult();
            Console.WriteLine(
                $"Username: {result.Username}" +
                $"\nTest: {result.TotalTest}" +
                $"\nPass:{result.Pass}" +
                $"\nFail: {result.Fail}" +
                $"\nSkip: {result.Skip}");
        }

        private static void GitClone()
        {
            const string gitUrl = @"https://github.com/taninpong/DemoTestforAcademy-true";

            // Find Directory name from URL
            const int githubIndexCount = 11;
            var githubIndex = gitUrl.LastIndexOf("github.com");
            var resultSubstring = gitUrl.Substring(githubIndex + githubIndexCount);
            var resultSplited = resultSubstring.Split('/');
            var directoryName = resultSplited.FirstOrDefault().ToLower();

            // TODO: Crate Directory to local (must change later before Using Azure Function App)
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

            Console.WriteLine($"Process is starting to clone project {directoryName}\n");
            Process.Start(new ProcessStartInfo("cmd", $"/c {command}") { UseShellExecute = false }).WaitForExit();
            Console.WriteLine("\nProcess is end. Clone was completed");
        }

        private static HomeworkTestResult TestResult()
        {
            var command =
                $"cd /" +
                $"&cd Git/taninpong/DemoTestforAcademy-true/Training.TestDrivenDevelopement/TDD.TestProject" +
                $"&dotnet test";

            var process = new Process
            {
                StartInfo = new ProcessStartInfo("cmd", $"/c {command}") { UseShellExecute = false, RedirectStandardOutput = true }
            };
            process.Start();
            var data = process.StandardOutput.ReadToEnd();

            // HACK: Fix username
            const string username = "taninpong";
            var convertor = new HomeworkResultConverter();
            var result = convertor.GetHomeworkResult(username, data);

            return result;
        }

    }
}
