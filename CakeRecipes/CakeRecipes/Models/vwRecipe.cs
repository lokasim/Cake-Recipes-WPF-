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
    
    public partial class vwRecipe
    {
        public int RecipeID { get; set; }
        public string RecipeName { get; set; }
        public string RecipeType { get; set; }
        public int NoPeople { get; set; }
        public string RecipeDescription { get; set; }
        public System.DateTime CreationDate { get; set; }
        public int UserID { get; set; }
        public string Changed { get; set; }
        public int Expr1 { get; set; }
        public string FirstLastName { get; set; }
        public string Username { get; set; }
        public string UserPassword { get; set; }
        public int IngredientID { get; set; }
        public string IngredientName { get; set; }
        public int IngredientAmountID { get; set; }
        public int Amount { get; set; }
        public int Expr2 { get; set; }
        public int Expr3 { get; set; }
    }
}