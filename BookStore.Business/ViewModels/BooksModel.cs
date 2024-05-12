using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Business.ViewModels
{
    public class BooksModel: DomainModel
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public decimal Price { get; set; }
        [DataType(DataType.Date)]
        public DateTime PublishDate { get; set; }
        public bool Status { get; set; }
        public string Description { get; set; }
        public string Image { set; get; }
    }
}
