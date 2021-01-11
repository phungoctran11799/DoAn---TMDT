namespace WebsiteBanGiay.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ChiTietDonHang")]
    public partial class ChiTietDonHang
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MaDonHang { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MaGiay { get; set; }

        public int? SoLuong { get; set; }

        [StringLength(10)]
        public string DonGia { get; set; }

        public virtual DonHang DonHang { get; set; }

        public virtual Giay Giay { get; set; }
    }
}
