using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Krafvaerk_shop.DBModels
{
    public enum LOG_TYPE { Error = 1, Transaction = 2, Order = 3 }
    public class Log
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime Timestamp { get; set; }
        
        public LOG_TYPE Logtype { get; set; }
        
        public string Message { get; set; }
    }
}
