using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using 2 thư viện thiết kế class metadata
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebsiteBanGiaySneaker.Models
{
    [MetadataType(typeof(CHITIETPNMetadata))]
    public partial class CHITIETPN
    {
        internal sealed class CHITIETPNMetadata
        {
            [Display(Name = "Mã phiếu nhập")]
            public int MaPN { get; set; }

            [Display(Name = "Mã sản phẩm")]
            public int MaSP { get; set; }

            [Display(Name = "Số lượng")]
            public Nullable<int> SoLuong { get; set; }

            [Display(Name = "Giá")]
            public Nullable<decimal> Gia { get; set; }

            [Display(Name = "Mã Size")]
            public int MaSize { get; set; }

            [Display(Name = "Mã Màu")]
            public int MaMau { get; set; }

            [Display(Name = "Thành tiền")]
            public Nullable<decimal> ThanhTien { get; set; }
        }
    }
}