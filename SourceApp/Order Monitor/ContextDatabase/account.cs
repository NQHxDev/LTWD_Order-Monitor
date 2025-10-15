namespace Order_Monitor.ContextDatabase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("account")]
    public partial class account
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public account()
        {
            export = new HashSet<export>();
            import = new HashSet<import>();
        }

        [Key]
        public int ac_id { get; set; }

        [Required]
        [StringLength(50)]
        public string username { get; set; }

        [Required]
        [StringLength(255)]
        public string password { get; set; }

        public int role { get; set; }

        public DateTime? created_at { get; set; }

        public string name { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<export> export { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<import> import { get; set; }

        public virtual ICollection<import_detail> import_detail { get; set; }
    }
}
