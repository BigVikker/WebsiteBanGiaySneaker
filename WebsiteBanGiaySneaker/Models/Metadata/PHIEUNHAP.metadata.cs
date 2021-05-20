using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using 2 thư viện thiết kế class metadata
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebsiteBanGiaySneaker.Models
{
    [MetadataType(typeof(PHIEUNHAPMetadata))]
    public partial class PHIEUNHAP
    {
        internal sealed class PHIEUNHAPMetadata
        {
            [Display(Name = "Mã phiếu nhập")]
            public int MaPN { get; set; }

            [Display(Name = "Ngày nhập")]
            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
            public System.DateTime NgayNhap { get; set; }

            [Display(Name = "Mã nhân viên")]
            public Nullable<int> MaNV { get; set; }

            [Display(Name = "Mã nhà cung cấp")]
            public Nullable<int> MaNCC { get; set; }

            [Display(Name = "Tổng tiền")]
            public Nullable<int> TongTien { get; set; }

            [Display(Name = "Tình trạng")]
            public string TinhTrang { get; set; }
        }
    }
}