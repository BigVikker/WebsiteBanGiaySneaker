using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using 2 thư viện thiết kế class metadata
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebsiteBanGiaySneaker.Models
{
    [MetadataType(typeof(NSXMetadata))]
    public partial class NSX
    {
        internal sealed class NSXMetadata
        {

            [Display(Name = "Mã thương hiệu")]
            public int MaNSX { get; set; }

            [Display(Name = "Tên thương hiệu")]
            public string TenNSX { get; set; }

            [Display(Name = "Địa chỉ")]
            public string DiaChi { get; set; }

            public string Email { get; set; }

            [Display(Name = "Số điện thoại")]
            public string Sdt { get; set; }
        }
    }
}