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
            
            const int githubIndexCount = 11;
            var githubIndex = gitUrl.LastIndexOf("github.com");
            var resultSubstring = gitUrl.Substring(githubIndex + githubIndexCount);
            var resultSplited = resultSubstring.Split('/');
            var directoryName = resultSplited.FirstOrDefault().ToLower();

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

    }
}
