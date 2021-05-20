using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using 2 thư viện thiết kế class metadata
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebsiteBanGiaySneaker.Models
{
    [MetadataType(typeof(SANPHAMMetadata))]
    public partial class SANPHAM
    {
        internal  sealed class SANPHAMMetadata
        {
            [Required(ErrorMessage ="{0} không được để trống")]
            [Display(Name ="Mã sản phẩm")]
            public int MaSP { get; set; }

            [Required(ErrorMessage = "{0} không được để trống")]
            [Display(Name = "Tên sản phẩm")]
            public string TenSP { get; set; }

            [Display(Name = "Hình ảnh")]
            public string Anh { get; set; }

            [Display(Name = "Thương hiệu")]
            public int MaNSX { get; set; }

            [Required(ErrorMessage = "{0} không được để trống")]
            [Display(Name = "Số lượng tổng")]
            public int SoLuongTong { get; set; }

            [Required(ErrorMessage = "{0} không được để trống")]
            [Display(Name = "Giá bán")]
            public decimal DonGia { get; set; }

            [Display(Name = "Hình ảnh 2")]
            public string Anh2 { get; set; }

            [Display(Name = "Hình ảnh 3")]
            public string Anh3 { get; set; }

            [Required(ErrorMessage = "{0} không được để trống")]
            [Display(Name = "Mô tả")]
            public string MoTa { get; set; }

            [Required(ErrorMessage = "{0} không được để trống")]
            [Display(Name = "Ngày cập nhật")]
            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString ="{0:dd/MM/yyyy}",ApplyFormatInEditMode =true)]
            public Nullable<System.DateTime> NgayCapNhat { get; set; }
        }
    }
}