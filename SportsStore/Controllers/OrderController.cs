using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using System.Linq;

namespace SportsStore.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderRepository _repository;
        private readonly Cart _cart;

        public OrderController(IOrderRepository repService, Cart cartService)
        {
            _repository = repService;
            _cart = cartService;
        }
        
        public ViewResult Checkout() => View(new Order());

        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            if (!_cart.Lines.Any())
            {
                ModelState.AddModelError("", "Sorry, your cart is empty");
            }

            if (!ModelState.IsValid) return View();
            
            order.Lines = _cart.Lines.ToArray();
            _repository.SaveOrder(order);
            _cart.Clear();
            
            return RedirectToPage("/Completed", new {orderId = order.OrderID});

        }
    }
}