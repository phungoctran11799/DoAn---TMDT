namespace WebsiteBanGiay.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NhaCungCap")]
    public partial class NhaCungCap
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NhaCungCap()
        {
            Giays = new HashSet<Giay>();
        }

        [Key]
        public int MaNCC { get; set; }

        [StringLength(50)]
        public string TenNCC { get; set; }

        [StringLength(200)]
        public string DiaChi { get; set; }

        [StringLength(50)]
        public string SoDienThoai { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Giay> Giays { get; set; }
    }
}
