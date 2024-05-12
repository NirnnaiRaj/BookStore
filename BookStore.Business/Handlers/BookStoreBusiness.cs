using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.Business.DataAccess;
using BookStore.Business.Interface;
using BookStore.Business.ViewModels;
using Axobis.Restaurant.Core.Constants;

namespace BookStore.Business.Handlers
{
    public class BookStoreBusiness : IBookStoreBusiness
    {
        private readonly ISqlDataAccessHelper sqlDataAccessHelper;
        private readonly IHttpContextAccessor httpContextAccessor;
        public BookStoreBusiness(ISqlDataAccessHelper sqlDataAccessHelper, IHttpContextAccessor httpContextAccessor)
        {
            this.sqlDataAccessHelper = sqlDataAccessHelper;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<BooksModel>> GetAllBooks()
        {
            List<BooksModel> booksList = new List<BooksModel>();
            var userID = httpContextAccessor.HttpContext.Session.GetString("userID");
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "GetAllBooks";
                cmd.CommandType = CommandType.StoredProcedure;
                using (DataSet ds = await this.sqlDataAccessHelper.GetDataSet(cmd))
                {
                    if (ds != null && ds.Tables != null)
                    {
                        var taskListTable = ds.Tables[0];

                        booksList = (from DataRow row in taskListTable.Rows
                                     select new BooksModel
                                     {
                                         Id = Convert.ToInt32(row["Book_ID"]),
                                         Title = Convert.ToString(row["Title"]),
                                         Author = Convert.ToString(row["Author"]),
                                         Genre = Convert.ToString(row["Genre"]),
                                         Price = Convert.ToDecimal(row["Price"]),
                                         PublishDate = Convert.ToDateTime(row["Publish_Date"]),
                                         Status = Convert.ToBoolean(row["Status"])
                                     }).ToList();

                    }
                }
                return booksList;

            }
        }

        public async Task<BooksModel> GetBookByID(int bookID)
        {
            BooksModel bookData = new BooksModel();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "GetBookByID";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BookID", bookID);
            using (DataSet ds = await this.sqlDataAccessHelper.GetDataSet(cmd))
            {
                if (ds != null && ds.Tables != null)
                {
                    var dtSettings = ds.Tables[0];
                    if (dtSettings.Rows.Count > 0)
                    {
                        bookData.Id = Convert.ToInt32(dtSettings.Rows[0]["Book_ID"]);
                        bookData.Title = dtSettings.Rows[0]["Title"].ToString();
                        bookData.Author = dtSettings.Rows[0]["Author"].ToString();
                        bookData.Genre = dtSettings.Rows[0]["Genre"].ToString();
                        bookData.Price = Convert.ToDecimal(dtSettings.Rows[0]["Price"]);
                        bookData.PublishDate = Convert.ToDateTime(dtSettings.Rows[0]["Publish_Date"]);
                        bookData.Status = Convert.ToBoolean(dtSettings.Rows[0]["Status"]);
                        bookData.Description = dtSettings.Rows[0]["Description"].ToString();
                        bookData.Image = dtSettings.Rows[0]["Image"].ToString();
                    }
                }

            }
            return bookData;
        }
        public async Task<bool> AddBookToCart(int bookID)
        {
            var userID = httpContextAccessor.HttpContext.Session.GetString("userID");
            var cmd = new SqlCommand();
            cmd.CommandText = "BookAddToCart";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BookID", bookID);
            cmd.Parameters.AddWithValue("@UserID", userID);
            var status = await this.sqlDataAccessHelper.ExecuteNonQueryAsync(cmd) > 0;
            return status;
        }

        public async Task<List<CartModel>> GetCartDetails()
        {
            List<CartModel> cartList = new List<CartModel>();
            var userID = httpContextAccessor.HttpContext.Session.GetString("userID");
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "GetCartStatusByUser";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserID", userID);
                using (DataSet ds = await this.sqlDataAccessHelper.GetDataSet(cmd))
                {
                    if (ds != null && ds.Tables != null)
                    {
                        var taskListTable = ds.Tables[0];

                        cartList = (from DataRow row in taskListTable.Rows
                                    select new CartModel
                                    {
                                        Id = Convert.ToInt32(row["Order_ID"]),
                                        Books = new BooksModel { Id = Convert.ToInt32(row["Book_ID"]) },
                                        Users = new UserModel { Id = Convert.ToInt32(row["User_ID"]) },
                                        Title = Convert.ToString(row["Title"]),
                                        Quantity = Convert.ToInt32(row["Quantity"]),
                                        Price = Convert.ToDecimal(row["Price"])
                                    }).ToList();

                    }
                }
                return cartList;

            }
        }


        public async Task<bool> RemoveFromCart(CartModel cartModel)
        {
            var userID = httpContextAccessor.HttpContext.Session.GetString("userID");
            var cmd = new SqlCommand();
            cmd.CommandText = "RemoveFromCart";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BookID", cartModel.Books.Id);
            cmd.Parameters.AddWithValue("@OrderID", cartModel.Id);
            cmd.Parameters.AddWithValue("@Type", cartModel.Type);
            cmd.Parameters.AddWithValue("@UserID", userID);
            var status = await this.sqlDataAccessHelper.ExecuteNonQueryAsync(cmd) > 0;
            return status;
        }

        public async Task<bool> ProceedToCheckout(CheckOutModel checkOutModel)
        {
            var userID = httpContextAccessor.HttpContext.Session.GetString("userID");
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "UpdateCheckout";
            cmd.Parameters.AddWithValue("@UserID", userID);
            cmd.CommandType = CommandType.StoredProcedure;

            var dtTable = DataUtilities.CreateDataTable("UDT_OrderTable", SqlTableTypeColumns.Orders_Columns);

            foreach (OrderModel table in checkOutModel.BookLists)
            {
                var rowTable = dtTable.NewRow();
                rowTable["Order_ID"] = table.Id;
                rowTable["Book_ID"] = table.Book.Id;
                dtTable.Rows.Add(rowTable);
            }
            var tableParamSale = cmd.Parameters.AddWithValue("@UDT_OrderTable", dtTable);


            var result = await this.sqlDataAccessHelper.ExecuteNonQueryAsync(cmd);
            return result > 0;

        }
        public async Task<List<CartModel>> PreviousOrders()
        {
            List<CartModel> cartList = new List<CartModel>();
            var userID = httpContextAccessor.HttpContext.Session.GetString("userID");
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "GetPreviousOrders";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserID", userID);
                using (DataSet ds = await this.sqlDataAccessHelper.GetDataSet(cmd))
                {
                    if (ds != null && ds.Tables != null)
                    {
                        var taskListTable = ds.Tables[0];

                        cartList = (from DataRow row in taskListTable.Rows
                                    select new CartModel
                                    {
                                        Id = Convert.ToInt32(row["Order_ID"]),
                                        Books = new BooksModel { Id = Convert.ToInt32(row["Book_ID"]) },
                                        Users = new UserModel { Id = Convert.ToInt32(row["User_ID"]) },
                                        Title = Convert.ToString(row["Title"]),
                                        Quantity = Convert.ToInt32(row["Quantity"]),
                                        Price = Convert.ToDecimal(row["Price"])
                                    }).ToList();

                    }
                }
                return cartList;

            }
        }
    }
}
