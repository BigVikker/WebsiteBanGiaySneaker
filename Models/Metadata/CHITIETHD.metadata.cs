using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using 2 thư viện thiết kế class metadata
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebsiteBanGiaySneaker.Models
{
    [MetadataType(typeof(CHITIETHDMetadata))]
    public partial class CHITIETHD
    {
        internal sealed class CHITIETHDMetadata
        {
            [Display(Name = "Mã đơn hàng")]
            public int MaDH { get; set; }

            [Display(Name = "Mã sản phẩm")]
            public int MaSP { get; set; }

            [Display(Name = "Số lượng")]
            [Range(1, 10, ErrorMessage = ("Số lượng tối thiểu là 1 tối đa 10 sản phẩm"))]
            public Nullable<int> SoLuong { get; set; }

            [Display(Name = "Đơn giá")]
            public Nullable<decimal> DonGia { get; set; }

            [Display(Name = "Mã size")]
            public int MaSize { get; set; }

            [Display(Name = "Mã màu")]
            public int MaMau { get; set; }
        }
    }
}