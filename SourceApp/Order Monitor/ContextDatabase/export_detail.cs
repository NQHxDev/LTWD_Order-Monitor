namespace Order_Monitor.ContextDatabase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class export_detail
    {
        [Key]
        public int ex_detl_id { get; set; }

        public int export_id { get; set; }

        public int item_id { get; set; }

        public decimal quantity { get; set; }

        public string note { get; set; }

        public virtual export export { get; set; }

        public virtual item item { get; set; }
    }
}
