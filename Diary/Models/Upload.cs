//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace _17_33330_1_Mid_Lab.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Web;

    public partial class Upload
    {
        public int id { get; set; }
        [DisplayName("Upload File")]
        public string path { get; set; }

        public HttpPostedFileBase File { get; set; }
    }
}
