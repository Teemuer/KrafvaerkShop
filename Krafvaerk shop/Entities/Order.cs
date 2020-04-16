using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Krafvaerk_shop.DBModels
{
    public enum ORDER_STATUS { Pending = 1, Ordered = 2, Completed = 3 }
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public ICollection<OrderRow> OrderRows { get; set; }

        [DataType(DataType.Date)]
        public DateTime Created { get; set; }
        public ORDER_STATUS Status { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
