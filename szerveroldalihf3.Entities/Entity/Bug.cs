using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using szerveroldalihf3.Entities.Helpers;

namespace szerveroldalihf3.Entities.Entity
{
    public class Bug : IIdEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [StringLength(250)]
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }

        [NotMapped]
        public virtual AppUser? AppUser { get; set; }
    }
}
