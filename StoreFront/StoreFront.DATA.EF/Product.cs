//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace StoreFront.DATA.EF
{
    using System;
    using System.Collections.Generic;
    
    public partial class Product
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public Nullable<short> UnitsInStock { get; set; }
        public Nullable<short> UnitsOnOrder { get; set; }
        public Nullable<int> BlendID { get; set; }
        public string Description { get; set; }
        public Nullable<int> ProductStatusID { get; set; }
        public byte[] Image { get; set; }
        public int CategoryID { get; set; }
    
        public virtual Blend Blend { get; set; }
        public virtual Category Category { get; set; }
        public virtual ProductStatus ProductStatus { get; set; }
    }
}
