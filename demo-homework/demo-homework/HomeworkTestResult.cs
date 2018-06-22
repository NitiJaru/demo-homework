using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demo_homework
{
    public class HomeworkTestResult
    {
        public string Username { get; set; }
        public bool IsHaveTest { get; set; }
        public int TotalTest { get; set; }
        public int Pass { get; set; }
        public int Fail { get; set; }
        public int Skip { get; set; }
    }
}
