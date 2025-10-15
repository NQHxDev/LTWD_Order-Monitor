namespace Order_Monitor.ContextDatabase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("item")]
    public partial class item
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public item()
        {
            depot = new HashSet<depot>();
            export_detail = new HashSet<export_detail>();
            food_ingredient = new HashSet<food_ingredient>();
            import_detail = new HashSet<import_detail>();
        }

        [Key]
        public int item_id { get; set; }

        [Required]
        [StringLength(100)]
        public string name { get; set; }

        public int unit_id { get; set; }

        public decimal import_price { get; set; }

        public bool is_active { get; set; }

        public int quantity { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<depot> depot { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<export_detail> export_detail { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<food_ingredient> food_ingredient { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<import_detail> import_detail { get; set; }

        public virtual unit unit { get; set; }
    }
}
