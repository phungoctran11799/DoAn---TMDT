namespace WebsiteBanGiay.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Log
    {
        public int ID { get; set; }

        public DateTime? Time { get; set; }

        public string Email { get; set; }

        public string Message { get; set; }
    }
}
