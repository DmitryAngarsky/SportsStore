using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportsStore.Infrastructure;
using SportsStore.Models;

namespace SportsStore.Pages
{
    public class CartModel : PageModel
    {
        private readonly IStoreRepository _repository;

        public CartModel(IStoreRepository repo, Cart cartService)
        {
            _repository = repo;
            Cart = cartService;
        }
        
        public Cart Cart { get; private set; }
        public string ReturnUrl { get; private set; }

        public void OnGet(string returnUrl)
        {
            ReturnUrl = returnUrl ?? "/";
        }

        public IActionResult OnPost(long productID, string returnUrl)
        {
            Product product = _repository.Products
                .FirstOrDefault(p => p.ProductID == productID);
            Cart.AddItem(product,1);
            return RedirectToPage(new {returnUrl});
        }
    }
}