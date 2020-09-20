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

        public IActionResult OnPost(long productId, string returnUrl)
        {
            Product product = _repository.Products
                .FirstOrDefault(p => p.ProductID == productId);
            Cart.AddItem(product,1);
            return RedirectToPage(new {returnUrl});
        }


        public IActionResult OnPostRemove(long productId, string resultUrl)
        {
            Cart.RemoveLine(Cart.Lines.First(cl => 
                cl.Product.ProductID == productId).Product);
            return RedirectToPage(new {resultUrl = resultUrl});
        }
    }
}