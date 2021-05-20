using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using 2 thư viện thiết kế class metadata
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebsiteBanGiaySneaker.Models
{
    [MetadataType(typeof(DONHANGMetadata))]
    public partial class DONHANG
    {
        internal sealed class DONHANGMetadata
        {
            [Display(Name = "Mã đơn hàng")]
            public int MaDH { get; set; }

            [Display(Name = "Mã khách hàng")]
            public Nullable<int> MaKH { get; set; }

            [Display(Name = "Mã nhân  viên")]
            public Nullable<int> MaNV { get; set; }

            [Display(Name = "Ngày đặt")]
            public Nullable<System.DateTime> NgayDat { get; set; }


            [Display(Name = "Ngày giao")]
            public Nullable<System.DateTime> NgayGiao { get; set; }

            [Required(ErrorMessage = "{0} không được để trống")]
            [Display(Name = "Địa chỉ giao")]
            public string DCGiao { get; set; }

            [Display(Name = "Tổng tiền")]
            public Nullable<decimal> TongTien { get; set; }

            [Display(Name = "Thanh toán")]
            public string ThanhToan { get; set; }

            [Display(Name = "Tình trạng")]
            public string TinhTrang { get; set; }

            [Required(ErrorMessage = "{0} không được để trống")]
            [Display(Name = "Họ tên")]
            public string HoTen { get; set; }

            [Required(ErrorMessage = "{0} không được để trống")]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required(ErrorMessage = "{0} không được để trống")]
            [Display(Name = "Số điện thoại")]
            public string SDT { get; set; }
        }
    }
}