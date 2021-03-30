using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApIForms
{
    class StudentData
    {
        public int Id { set; get; }


        public string Name { set; get; }

        public int Age { set; get; }

        public string Email { set; get; }
        
        public string Password { set; get; }
        public int? DeptId { get; set; }


        public  Department Department { set; get; }
        }
}
