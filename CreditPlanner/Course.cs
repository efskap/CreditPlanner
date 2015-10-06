using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreditPlanner
{
    public class Course
    {
        public string name { get; set; }
        public int credits { get; set; }
        public int year { get; set; }
        public string[] flags { get; set;  }
        [Newtonsoft.Json.JsonIgnore]
        public bool selected { get; set; }
         [Newtonsoft.Json.JsonIgnore]
        public bool upperLevel
        {
            get
            {
                return int.Parse(name.Split(' ')[1]) >= 300;
            }
        }
        
    }
}
