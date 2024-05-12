using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Models;
using BookStore.Business.Filters;
using BookStore.Business.Interface;
using BookStore.Business.ViewModels;

namespace BookStore.Controllers
{
    [AuthorizationFilterAttribute]
    public class BooksController : Controller
    {
        private readonly IBookStoreBusiness bookStoreBuisness;
        public BooksController(IBookStoreBusiness bookStoreBuisness)
        {
            this.bookStoreBuisness = bookStoreBuisness;
        }
        public async Task<ActionResult> Index()
        {
            List<BooksModel> books = await bookStoreBuisness.GetAllBooks();

            return View(books);
        }
        public async Task<IActionResult> BookDetails(int id)
        {
            BooksModel bookDetail = await bookStoreBuisness.GetBookByID(id);

            return View(bookDetail);
        }
        [HttpPost]
        public async Task<IActionResult> AddToCart(int id)
        {
            var result = await bookStoreBuisness.AddBookToCart(id);

            if (result)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }

        public async Task<IActionResult> ViewCart()
        {
            List<CartModel> cartList = await bookStoreBuisness.GetCartDetails();

            return View(cartList);
        }
        [HttpPost]
        public async Task<IActionResult> RemoveFromCart([FromBody]CartModel cartModel)
        {
            var result = await bookStoreBuisness.RemoveFromCart(cartModel);

            if (result)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }
        public async Task<IActionResult> ProceedToCheckout([FromBody]CheckOutModel checkOutModel)
        {
            var result = await bookStoreBuisness.ProceedToCheckout(checkOutModel);
            if (result)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }
        public async Task<IActionResult> PreviousOrders()
        {
            List<CartModel> orderList = await bookStoreBuisness.PreviousOrders();

            return View(orderList);
        }
        
    }
}
