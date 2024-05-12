using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Business.ViewModels
{
    public class CartModel: DomainModel
    {
        public BooksModel Books { set; get; }
        public UserModel Users { set; get; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int Type { get; set; }
    }
}
