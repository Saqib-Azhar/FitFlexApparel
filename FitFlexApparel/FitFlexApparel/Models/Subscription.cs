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
    
    public partial class Subscription
    {
        public int Id { get; set; }
        public string Email_Id { get; set; }
        public Nullable<System.DateTime> Subscribed_On { get; set; }
        public Nullable<bool> Is_Deleted { get; set; }
        public Nullable<System.DateTime> Updated_At { get; set; }
    }
}