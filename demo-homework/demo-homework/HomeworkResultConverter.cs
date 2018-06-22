using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demo_homework
{
    public class HomeworkResultConverter
    {
        public HomeworkTestResult GetHomeworkResult(string username, string testResult)
        {
            return GetHomeWorkResultWithTotalLine(username, GetTotalLine(testResult));
        }

        private string GetTotalLine(string testResult)
        {
            string[] separators = { "\n", "\r" };
            var keyWordTotal = "Total tests:";
            var keyWordPass = "Passed:";
            var keyWordFail = "Failed:";
            var keyWordSkip = "Skipped:";
            var testResultListByLine = testResult.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            var result = testResultListByLine.FirstOrDefault(x =>
                                        x.Contains(keyWordTotal) &&
                                        x.Contains(keyWordPass) &&
                                        x.Contains(keyWordFail) &&
                                        x.Contains(keyWordSkip));
            return result;
        }

        private HomeworkTestResult GetHomeWorkResultWithTotalLine(string username, string totalLine)
        {
            if (totalLine == null)
            {
                return new HomeworkTestResult { Username = username, IsHaveTest = false };
            }

            var separator = ",";
            var textWithPointnly = totalLine.Replace("Total tests:", string.Empty)
                                             .Replace("Passed: ", separator)
                                             .Replace("Failed: ", separator)
                                             .Replace("Skipped: ", separator)
                                             .Replace(".", string.Empty)
                                             .Replace(" ", string.Empty);

            string[] separators = { separator };
            var pointListText = textWithPointnly.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            var pointList = pointListText.Select(int.Parse)?.ToList();
            var result = new HomeworkTestResult
            {
                Username = username,
                IsHaveTest = true,
                TotalTest = pointList[0],
                Pass = pointList[1],
                Fail = pointList[2],
                Skip = pointList[3]
            };

            return result;
        }
    }
}
