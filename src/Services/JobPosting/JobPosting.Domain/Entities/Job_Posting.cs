using JobPosting.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPosting.Domain.Entities
{
    public class Job_Posting : EntityBase
    {
        
        public string Title { get; set; }
        public string JobDescription { get; set; }
        public DateTime PostDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string JobLocation { get; set; }
        public decimal Salary { get; set; }
        public bool IsRemote { get; set; }
        public bool IsFullTime { get; set; }
        public string ContactEmail { get; set; }



        
    }
}
