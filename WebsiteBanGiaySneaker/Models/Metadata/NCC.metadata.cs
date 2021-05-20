using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using 2 thư viện thiết kế class metadata
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebsiteBanGiaySneaker.Models
{
    [MetadataType(typeof(NCCMetadata))]
    public partial class NCC
    {
        internal sealed class NCCMetadata
        {
            [Display(Name = "Mã nhà cung cấp")]
            public int MaNCC { get; set; }

            [Display(Name = "Tên nhà cung cấp")]
            public string TenNCC { get; set; }

            [Display(Name = "Địa chỉ")]
            public string DiaChi { get; set; }

            public string Email { get; set; }

            [Display(Name = "Số điện thoại")]
            public string Sdt { get; set; }
        }
    }
}