namespace WebsiteBanGiay.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Giay")]
    public partial class Giay
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Giay()
        {
            ChiTietDonHangs = new HashSet<ChiTietDonHang>();
        }

        [Key]
        public int MaGiay { get; set; }

        [StringLength(50)]
        public string TenGiay { get; set; }

        public decimal? GiaBan { get; set; }

        public string MoTa { get; set; }

        [StringLength(50)]
        public string AnhBia { get; set; }

        public DateTime? NgayCapNhat { get; set; }

        public int? SoLuongTon { get; set; }

        public int? MaNCC { get; set; }

        public int? MaLoai { get; set; }

        public int? Moi { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; }

        public virtual Loai Loai { get; set; }

        public virtual NhaCungCap NhaCungCap { get; set; }

    }
}
