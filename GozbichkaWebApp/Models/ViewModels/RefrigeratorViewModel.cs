using GozbichkaWebApp.Models;
using System.Collections.Generic;

namespace GozbichkaWebApp.Models.ViewModels
{
    public class RefrigeratorViewModel
    {
        public int UserId { get; set; }
        public List<Product> AllProducts { get; set; } = new List<Product>();
        public List<Recipe> MatchingRecipes { get; set; } = new List<Recipe>();

        // 1. Everything saved in their fridge (The White AND Orange chips)
        public List<int> FridgeProductIds { get; set; } = new List<int>();

        // 2. ONLY the items actively selected for this specific search (The Orange chips)
        public List<int> ActiveSearchProductIds { get; set; } = new List<int>();
    }
}