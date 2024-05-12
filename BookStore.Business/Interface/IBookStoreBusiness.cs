using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.Business.ViewModels;

namespace BookStore.Business.Interface
{
    public interface IBookStoreBusiness
    {
        Task<List<BooksModel>> GetAllBooks();
        Task<BooksModel> GetBookByID(int bookID);
        Task<bool> AddBookToCart(int bookID);
        Task<List<CartModel>> GetCartDetails();
        Task<bool> RemoveFromCart(CartModel cartModel);
        Task<bool> ProceedToCheckout(CheckOutModel checkOutModel);
        Task<List<CartModel>> PreviousOrders();

    }
}
