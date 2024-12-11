namespace QLBanHangCaPhe.EntitiesDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CHITIETHOADON")]
    public partial class CHITIETHOADON
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(5)]
        public string MAHOADON { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(5)]
        public string MASANPHAM { get; set; }

        public int? SOLUONG { get; set; }

        public virtual HOADON HOADON { get; set; }

        public virtual SANPHAM SANPHAM { get; set; }
    }
}
