namespace DoAn_LTW.ContextDatabase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class order_detail
    {
        [Key]
        public int ord_delt_id { get; set; }

        public int order_id { get; set; }

        public int food_id { get; set; }

        public int quantity { get; set; }

        public decimal price { get; set; }

        public virtual food food { get; set; }

        public virtual list_order list_order { get; set; }
    }
}
