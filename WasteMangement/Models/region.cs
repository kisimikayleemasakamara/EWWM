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
    
    public partial class region
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public region()
        {
            this.districts = new HashSet<district>();
        }

        public int regionId { get; set; }
        public int countryId { get; set; }
        public string region_name { get; set; }
        public string description { get; set; }
        public int isDeleted { get; set; }
        public string name { get; set; }
        public virtual country country { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<district> districts { get; set; }
    }
}
