//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WasteMangement.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Discussion
    {
        public int id { get; set; }
        public string UserId { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string discussion_content { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
    }
}