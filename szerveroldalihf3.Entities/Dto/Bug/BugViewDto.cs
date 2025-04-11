using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace szerveroldalihf3.Entities.Dto.Bug
{
    public class BugViewDto
    {
        public string Id { get; set; } = "";
        public string Description { get; set; } = "";
        public DateTime Date { get; set; } = DateTime.MinValue;
    }
}
