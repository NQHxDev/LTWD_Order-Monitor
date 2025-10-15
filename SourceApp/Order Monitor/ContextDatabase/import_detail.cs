namespace Order_Monitor.ContextDatabase
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class import_detail
    {
        [Key]
        public int im_detl_id { get; set; }

        public int import_id { get; set; }

        public int item_id { get; set; }

        public decimal quantity { get; set; }

        public decimal unit_price { get; set; }

        public DateTime? import_date { get; set; }

        public int? import_by { get; set; }

        public virtual import import { get; set; }

        public virtual item item { get; set; }

        public virtual account account { get; set; }
    }
}
