using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPosting.Domain.Common
{
    public abstract class EntityBase
    {
        public int Id { get; protected set; }
        public string JobCategory { get; set; }
        public string JobCategoryDescription { get; set; }
        
    }
}
