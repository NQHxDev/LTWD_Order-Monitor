namespace DoAn_LTW.ContextDatabase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("export")]
    public partial class export
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public export()
        {
            export_detail = new HashSet<export_detail>();
        }

        [Key]
        public int export_id { get; set; }

        public DateTime? export_date { get; set; }

        [StringLength(255)]
        public string purpose { get; set; }

        public int? created_by { get; set; }

        public virtual account account { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<export_detail> export_detail { get; set; }
    }
}
