//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FitFlexApparel.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ContributionRequest
    {
        public int Id { get; set; }
        public Nullable<System.DateTime> Requested_At { get; set; }
        public Nullable<bool> Is_Read { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Contact_No { get; set; }
        public string Country { get; set; }
        public string Message { get; set; }
        public Nullable<int> Request_Type_Id { get; set; }
    
        public virtual ContributionRequestsType ContributionRequestsType { get; set; }
    }
}