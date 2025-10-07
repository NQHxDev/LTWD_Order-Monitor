namespace DoAn_LTW.ContextDatabase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class import_detail
    {
        [Key]
        public int im_detl_id { get; set; }

        public int import_id { get; set; }

        public int item_id { get; set; }

        public decimal quantity { get; set; }

        public decimal unit_price { get; set; }

        public virtual import import { get; set; }

        public virtual item item { get; set; }
    }
}
