using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using 2 thư viện thiết kế class metadata
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace WebsiteBanGiaySneaker.Models
{
    [MetadataType(typeof(SizeMetadata))]
    public partial class Size
    {
        internal sealed class SizeMetadata
        {
            [Display(Name = "Mã size")]
            public int MaSize { get; set; }

            [Display(Name = "Size")]
            public int Size1 { get; set; }

        }
    }
}