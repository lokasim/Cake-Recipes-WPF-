//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CakeRecipes.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblIngredientStorage
    {
        public int IngredientStorageID { get; set; }
        public int Amount { get; set; }
        public Nullable<int> UserID { get; set; }
        public int IngredientID { get; set; }
    
        public virtual tblIngredient tblIngredient { get; set; }
        public virtual tblUser tblUser { get; set; }
    }
}
