namespace Base_DAL.ContextDatabase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class list_order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public list_order()
        {
            order_detail = new HashSet<order_detail>();
        }

        [Key]
        public int oder_id { get; set; }

        [StringLength(100)]
        public string customer_name { get; set; }

        public string note { get; set; }

        public decimal total_price { get; set; }

        public short status { get; set; }

        public DateTime? created_at { get; set; }

        public DateTime? updated_at { get; set; }

        [StringLength(10)]
        public string customer_phone { get; set; }

        public string staff_feedback { get; set; }

        public int count_food { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<order_detail> order_detail { get; set; }
    }
}
