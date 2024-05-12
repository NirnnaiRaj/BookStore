using BookStore.Business.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Business.ViewModels
{
    public class CheckOutModel : DomainModel
    {
        public List<OrderModel> BookLists { set; get; }
    }
}

