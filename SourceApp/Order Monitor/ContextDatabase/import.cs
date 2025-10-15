using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Order_Monitor.ContextDatabase
{
    [Table("import")]
    public partial class import
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public import()
        {
            import_detail = new HashSet<import_detail>();
        }

        [Key]
        public int import_id { get; set; }

        public DateTime? create_at { get; set; }

        public decimal total_value { get; set; }

        public int? created_by { get; set; }

        public int import_status { get; set; }

        public virtual account account { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<import_detail> import_detail { get; set; }
    }
}
