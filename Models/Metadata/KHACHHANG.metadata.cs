using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using 2 thư viện thiết kế class metadata
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebsiteBanGiaySneaker.Models
{
    [MetadataType(typeof(KHACHHANGMetadata))]
    public partial class KHACHHANG
    {
        internal sealed class KHACHHANGMetadata
        {
            [Display(Name = "Mã khách hàng")]
            public int MAKH { get; set; }

            [Required(ErrorMessage = "{0} không được để trống")]
            [Display(Name = "Họ tên")]
            public string TenKH { get; set; }

            [Required(ErrorMessage = "{0} không được để trống")]
            [Display(Name = "Giới tính")]
            public string GioiTinh { get; set; }

            [Required(ErrorMessage = "{0} không được để trống")]
            [Display(Name = "Địa chỉ")]
            public string DiaChi { get; set; }

            [Required(ErrorMessage = "{0} không được để trống")]
            [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Vui lòng nhập đúng đinh dạng là Email")]
            [DataType(DataType.EmailAddress)]
            public string Email { get; set; }

            [Required(ErrorMessage = "{0} không được để trống")]
            [Display(Name = "Số điện thoại")]
            [DataType(DataType.PhoneNumber)]
            [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Vui lòng nhập đúng đinh dạng là SĐT")]
            public string Sdt { get; set; }

            [Required(ErrorMessage = "{0} không được để trống")]
            [Display(Name = "Mật khẩu")]
            public string MatKhau { get; set; }
        }
    }
}