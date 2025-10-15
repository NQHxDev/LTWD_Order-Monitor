namespace Order_Monitor.ContextDatabase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("food")]
    public partial class food
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public food()
        {
            food_ingredient = new HashSet<food_ingredient>();
            order_detail = new HashSet<order_detail>();
        }

        [Key]
        public int food_id { get; set; }

        [Required]
        [StringLength(100)]
        public string name { get; set; }

        public string description { get; set; }

        public int price { get; set; }

        public short status { get; set; }

        public DateTime? created_at { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<food_ingredient> food_ingredient { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<order_detail> order_detail { get; set; }
    }

    public class StatusItem
    {
        public string Text { get; set; }
        public int Value { get; set; }
    }

}
