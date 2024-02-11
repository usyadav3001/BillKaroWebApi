using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using StudioWebApi.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace StudioWebApi.Repository
{
    public class BookingRepository : IBookingRepository
    {
        private readonly string connectionString;
        Sql sql=new Sql();
        Result result = new Result();
        public BookingRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public Result GetAllBookings(Booking booking)
        {
            result = new Result();
            SqlCommand command = new SqlCommand(@"SELECT b.Id BookingId,c.FullName AS CustomerName,FORMAT(b.OrderDate,'dd-MM-yyy') OrderDate,b.TotalQuanity,(b.TotalDiscountAmount+b.GrandDiscountAmount) TotalDiscountAmount,b.GrandTotalAmount,b.CompanyCode,c.CreatedOn
            FROM tblBooking b 
            LEFT JOIN tblCustomer c ON b.CustomerId=c.Id
            WHERE b.CompanyCode=@CompanyCode AND c.CompanyCode=@CompanyCode
            ", sql.Connection);
            command.Parameters.AddWithValue("@CompanyCode", booking.CompanyCode);

            DataTable dt = sql.ExecuteSqlCommand<DataTable>(command);
            var arrBooking = dt.AsEnumerable().Select(row => new
            {
                BookingId = row.Field<int>("BookingId"),
                CustomerName = row.Field<string>("CustomerName"),
                OrderDate = row.Field<string>("OrderDate"),
                TotalQuanity = row.Field<int>("TotalQuanity"),
                TotalDiscountAmount = row.Field<decimal>("TotalDiscountAmount"),
                GrandTotalAmount = row.Field<decimal>("GrandTotalAmount"),
                CompanyCode = row.Field<string>("CompanyCode"),
                CreatedOn = row.Field<DateTime>("CreatedOn"),
            }).ToList<object>();
            result.Data = arrBooking;
            result.Message = "";
            result.Status = "Success";
            return result;
        }

        public Result GetBookingById(Booking booking)
        {
            result = new Result();
            SqlCommand command = new SqlCommand(@"SELECT b.Id BookingId,c.FullName AS CustomerName,FORMAT(b.OrderDate,'dd-MM-yyy') OrderDate,b.TotalQuanity,(b.TotalDiscountAmount+b.GrandDiscountAmount) TotalDiscountAmount,b.GrandTotalAmount,b.CompanyCode,c.CreatedOn
            FROM tblBooking b 
            LEFT JOIN tblCustomer c ON b.CustomerId=c.Id
            WHERE b.Id=@BookingId AND b.CompanyCode=@CompanyCode AND c.CompanyCode=@CompanyCode
            ", sql.Connection);
            command.Parameters.AddWithValue("@BookingId", booking.BookingId);
            command.Parameters.AddWithValue("@CompanyCode", booking.CompanyCode);

            DataTable dt = sql.ExecuteSqlCommand<DataTable>(command);
            var arrBooking = dt.AsEnumerable().Select(row => new
            {
                BookingId = row.Field<int>("BookingId"),
                CustomerName = row.Field<string>("CustomerName"),
                OrderDate = row.Field<string>("OrderDate"),
                TotalQuanity = row.Field<int>("TotalQuanity"),
                TotalDiscountAmount = row.Field<decimal>("TotalDiscountAmount"),
                GrandTotalAmount = row.Field<decimal>("GrandTotalAmount"),
                CompanyCode = row.Field<string>("CompanyCode"),
                CreatedOn = row.Field<DateTime>("CreatedOn"),
            }).ToList<object>();
            result.Data = arrBooking;
            result.Message = "";
            result.Status = "Success";
            return result;
        }

        public Result AddBooking(Booking booking)
        {
            result = new Result();
            try
            {
                SqlCommand command = new SqlCommand(@"INSERT INTO tblBooking (CustomerId, OrderDate, Discription, TotalQuanity, TotalDiscountAmount, TotalTaxAmount, TotalAmount, GrandDiscountAmount, GrandTotalAmount, CompanyCode, CreatedBy, CreatedOn)
                VALUES(@CustomerId, @OrderDate, @Discription, @TotalQuanity, @TotalDiscountAmount, @TotalTaxAmount, @TotalAmount, @GrandDiscountAmount, @GrandTotalAmount, @CompanyCode, @CreatedBy, GetDate()); SELECT SCOPE_IDENTITY();", sql.Connection);
                command.Parameters.AddWithValue("@CustomerId", booking.CustomerId);
                command.Parameters.AddWithValue("@OrderDate",Convert.ToDateTime( booking.OrderDate));
                command.Parameters.AddWithValue("@Discription", booking.Discription);
                command.Parameters.AddWithValue("@TotalQuanity", booking.TotalQuanity);
                command.Parameters.AddWithValue("@TotalDiscountAmount", booking.TotalDiscountAmount);
                command.Parameters.AddWithValue("@TotalTaxAmount", booking.TotalTaxAmount);
                command.Parameters.AddWithValue("@TotalAmount", booking.TotalAmount);
                command.Parameters.AddWithValue("@GrandDiscountAmount", booking.GrandDiscountAmount);
                command.Parameters.AddWithValue("@GrandTotalAmount", booking.GrandTotalAmount);
                command.Parameters.AddWithValue("@CompanyCode", booking.CompanyCode);
                command.Parameters.AddWithValue("@CreatedBy", booking.CreatedBy);
                string newBookingId= sql.ExecuteScalar<string>(command);
                booking.BookingData.ForEach(obj => obj.BookingId = Convert.ToInt32(newBookingId));
                AddBookingDetail(booking.BookingData);
                result.Status = "Success";
                result.Message = "Data inserted successfully";
            }
            catch(Exception ex)
            {
                result.Status = "Error";
                result.Message =Convert.ToString(ex);
            }
            
            return result;
        }

        public string AddBookingDetail(List<BookingDetail> bookingDetailData)
        {
            result = new Result();
            try
            {
                bookingDetailData.ForEach(bookingDetail =>
                {
                    SqlCommand command = new SqlCommand(@"INSERT INTO tblBookingDetail (ItemId, BookingId, Quantity, UnitId, Price, DiscountPercentage, DiscountAmount, GrandDiscountAmount, TaxPercentage, TaxAmount, TotalAmount, CompanyCode, CreatedBy, CreatedOn)
	                VALUES (@ItemId,@BookingId, @Quantity, @UnitId, @Price, @DiscountPercentage, @DiscountAmount, @GrandDiscountAmount, @TaxPercentage, @TaxAmount, @TotalAmount, @CompanyCode, @CreatedBy,GetDate()); SELECT SCOPE_IDENTITY();", sql.Connection);
                    command.Parameters.AddWithValue("@ItemId", bookingDetail.ItemId);
                    command.Parameters.AddWithValue("@BookingId", bookingDetail.BookingId);
                    command.Parameters.AddWithValue("@Quantity", bookingDetail.Quantity);
                    command.Parameters.AddWithValue("@UnitId", bookingDetail.UnitId);
                    command.Parameters.AddWithValue("@Price", bookingDetail.Price);
                    command.Parameters.AddWithValue("@DiscountPercentage", bookingDetail.DiscountPercentage);
                    command.Parameters.AddWithValue("@DiscountAmount", bookingDetail.DiscountAmount);
                    command.Parameters.AddWithValue("@GrandDiscountAmount", bookingDetail.GrandDiscountAmount);
                    command.Parameters.AddWithValue("@TaxPercentage", bookingDetail.TaxPercentage);
                    command.Parameters.AddWithValue("@TaxAmount", bookingDetail.TaxAmount);
                    command.Parameters.AddWithValue("@TotalAmount", bookingDetail.TotalAmount);
                    command.Parameters.AddWithValue("@CompanyCode", bookingDetail.CompanyCode);
                    command.Parameters.AddWithValue("@CreatedBy", bookingDetail.CreatedBy);
                    string newBookingId = sql.ExecuteScalar<string>(command);
                });
            }
           catch(Exception ex)
            {
                return Convert.ToString(ex);
            }
            return "Data inserted successfully";
        }

        public Result UpdateBooking(Booking Booking)
        {
            SqlCommand command = new SqlCommand("UPDATE tblBooking SET FullName = @FullName, PhoneNo = @PhoneNo, Email = @Email, Address = @Address, UpdatedBy=@UpdatedBy, UpdatedOn=GetDate() WHERE Id = @Id and CompanyCode=@CompanyCode", sql.Connection);
            command.Parameters.AddWithValue("@Id", Booking.Id);
            //command.Parameters.AddWithValue("@FullName", Booking.FullName);
            //command.Parameters.AddWithValue("@PhoneNo", Booking.PhoneNo);
            //command.Parameters.AddWithValue("@Email", Booking.Email);
            //command.Parameters.AddWithValue("@Address", Booking.Address);
            command.Parameters.AddWithValue("@CompanyCode", Booking.CompanyCode);
            command.Parameters.AddWithValue("@UpdatedBy", Booking.UpdatedBy);
            sql.ExecuteScalar<string>(command);
            //using (SqlConnection connection = new SqlConnection(connectionString))
            //{
            //    connection.Open();

            //    using (SqlCommand command = new SqlCommand("UPDATE tblBooking SET FullName = @FullName, PhoneNo = @PhoneNo, Email = @Email, Address = @Address WHERE Id = @Id", connection))
            //    {
            //        command.Parameters.AddWithValue("@Id", Booking.Id);
            //        command.Parameters.AddWithValue("@FullName", Booking.FullName);
            //        command.Parameters.AddWithValue("@PhoneNo", Booking.PhoneNo);
            //        command.Parameters.AddWithValue("@Email", Booking.Email);
            //        command.Parameters.AddWithValue("@Address", Booking.Address);

            //        command.ExecuteNonQuery();
            //    }
            //}
            result.Data = GetBookingById(Booking);
            result.Status = "Success";
            result.Message = "Data upated successfully";
            return result;
        }

        public Result DeleteBooking(Booking Booking)
        {
            SqlCommand command = new SqlCommand("DELETE FROM tblBooking WHERE Id = @Id and CompanyCode=@CompanyCode", sql.Connection);
            command.Parameters.AddWithValue("@Id", Booking.Id);
            command.Parameters.AddWithValue("@CompanyCode", Booking.CompanyCode);
            int i = sql.ExecuteScalar<int>(command);
            //using (SqlConnection connection = new SqlConnection(connectionString))
            //{
            //    connection.Open();

            //    using (SqlCommand command = new SqlCommand("DELETE FROM tblBooking WHERE Id = @Id", connection))
            //    {
            //        command.Parameters.AddWithValue("@Id", id);

            //        command.ExecuteNonQuery();
            //    }
            //}
            result.Status = "Success";
            result.Message = "Data deleted successfully";
            return result;
        }

    }
}