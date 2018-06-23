using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demo_homework
{
    public class HomeworkResultConverter
    {
        /// <summary>
        /// แปลงผลลัพย์ในการ test ให้อยู่ในรูป object HomeworkTestResult
        /// </summary>
        /// <param name="username"></param>
        /// <param name="testResult"></param>
        /// <returns></returns>
        public HomeworkTestResult GetHomeworkResult(string username, string testResult)
        {
            if (string.IsNullOrEmpty(username)) return null;
            if (string.IsNullOrEmpty(testResult)) return null;
            return GetHomeWorkResultWithTotalLine(username, GetTestSummaryLine(testResult));
        }

        /// <summary>
        /// ดึง text บรรทัดที่เป็นผลลัพย์ในการ test
        /// </summary>
        /// <param name="testResult"></param>
        /// <returns></returns>
        private string GetTestSummaryLine(string testResult)
        {
            if (string.IsNullOrEmpty(testResult)) return null;
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

        /// <summary>
        /// แปลง บรรทัดที่เป็นผลลัพย์ในการ test เป็น object HomeworkTestResult
        /// </summary>
        /// <param name="username"></param>
        /// <param name="totalLine"></param>
        /// <returns></returns>
        private HomeworkTestResult GetHomeWorkResultWithTotalLine(string username, string totalLine)
        {
            if (string.IsNullOrEmpty(username)) return null;
            if (totalLine == null)
            {
                return new HomeworkTestResult { GitUsername = username, IsHaveTest = false };
            }
            //ลบคำที่ไม่ใช้เพื่อให้เหลือค่าที่จะนำไปเก็บและคั่นด้วย ,
            var separator = ",";
            var textWithPointnly = totalLine.Replace("Total tests:", string.Empty)
                                             .Replace("Passed: ", separator)
                                             .Replace("Failed: ", separator)
                                             .Replace("Skipped: ", separator)
                                             .Replace(".", string.Empty)
                                             .Replace(" ", string.Empty);

            string[] separators = { separator };
            //ดึงค่าต่างๆที่ใช้ในการเก็บค่า
            var pointListText = textWithPointnly.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            var pointList = pointListText.Select(int.Parse)?.ToList();
            var result = new HomeworkTestResult
            {
                GitUsername = username,
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
