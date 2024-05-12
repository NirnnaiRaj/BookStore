using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Business.ViewModels
{
    public class OrderModel : DomainModel
    {
        public UserModel User { set; get; }
        public BooksModel Book { set; get; }
        public int Quantity { set; get; }
        public DateTime OrderDate { set; get; }
        public decimal TotalPrice { set; get; }
    }
}
