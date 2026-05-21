using GozbichkaWebApp.Models;
using System.Collections.Generic;

namespace GozbichkaWebApp.Models.ViewModels
{
    public class RefrigeratorViewModel
    {
        public int UserId { get; set; }
        public List<Product> AllProducts { get; set; } = new List<Product>();
        public List<Recipe> MatchingRecipes { get; set; } = new List<Recipe>();
        public List<int> FridgeProductIds { get; set; } = new List<int>();
        public List<int> ActiveSearchProductIds { get; set; } = new List<int>();
    }
}