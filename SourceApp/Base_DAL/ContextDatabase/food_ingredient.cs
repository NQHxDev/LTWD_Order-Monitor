namespace Base_DAL.ContextDatabase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class food_ingredient
    {
        [Key]
        public int fo_ingr_id { get; set; }

        public int food_id { get; set; }

        public int item_id { get; set; }

        public decimal quantity { get; set; }

        public virtual food food { get; set; }

        public virtual item item { get; set; }
    }
}
