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
    [MetadataType(typeof(MauSacMetadata))]
    public partial class MauSac
    {
        internal sealed class MauSacMetadata
        {
            [Display(Name = "Mã màu")]
            public int MaMau { get; set; }

            [Display(Name = "Màu sắc")]
            public string Color { get; set; }

            //public SelectList MauSacList { get; set; }

        }
    }
}