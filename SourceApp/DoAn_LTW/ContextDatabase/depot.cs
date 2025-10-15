namespace DoAn_LTW.ContextDatabase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("depot")]
    public partial class depot
    {
        [Key]
        public int stt { get; set; }

        public int item_id { get; set; }

        public decimal quantity { get; set; }

        public DateTime? last_updated { get; set; }

        public virtual item item { get; set; }
    }
}
