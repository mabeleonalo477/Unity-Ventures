using System.ComponentModel.DataAnnotations;
using UnityVentures.Models;
namespace UnityVentures.ViewModels
{
    public class DashboardViewModel
    {
        [Required(ErrorMessage = "Business name is required")]
        public string BusinessName { get; set; } = null!;
        public decimal Balance { get; set; }
        public List<Transaction> Transactions { get; set; } = new();
    }
}