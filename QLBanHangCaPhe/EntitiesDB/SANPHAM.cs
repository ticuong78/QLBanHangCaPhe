namespace QLBanHangCaPhe.EntitiesDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SANPHAM")]
    public partial class SANPHAM
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SANPHAM()
        {
            CHITIETHOADON = new HashSet<CHITIETHOADON>();
        }

        [Key]
        [StringLength(5)]
        public string MASANPHAM { get; set; }

        [StringLength(50)]
        public string TENSANPHAM { get; set; }

        [StringLength(5)]
        public string MALOAISP { get; set; }

        public decimal? GIABAN { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CHITIETHOADON> CHITIETHOADON { get; set; }

        public virtual LOAISANPHAM LOAISANPHAM { get; set; }
    }
}
