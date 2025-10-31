namespace Base_DAL.ContextDatabase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

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

        public decimal total_price { get; set; }

        public int? created_by { get; set; }

        public int import_status { get; set; }

        public int? update_by { get; set; }

        public int total_item { get; set; }

        [StringLength(500)]
        public string reason { get; set; }

        public DateTime? update_at { get; set; }

        public virtual account account { get; set; }

        public virtual account account1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<import_detail> import_detail { get; set; }
    }
}
