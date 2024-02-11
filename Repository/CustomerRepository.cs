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
    public class CustomerRepository : ICustomerRepository
    {
        private readonly string connectionString;
        Sql sql=new Sql();
        Result result = new Result();
        public CustomerRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public Result GetAllCustomers(Customer customer)
        {
            List<Customer> arrC = new List<Customer>();
            result = new Result();
            string query = "SELECT * FROM tblCustomer  WHERE CompanyCode=" + customer.CompanyCode;
            DataTable dt = sql.ExecuteQuery(query);
            var arrCustomer = dt.AsEnumerable().Select(row => new
            {
                Id = row.Field<int>("Id"),
                FullName = row.Field<string>("FullName"),
                PhoneNo = row.Field<string>("PhoneNo"),
                Email = row.Field<string>("Email"),
                Address = row.Field<string>("Address"),
                CompanyCode = row.Field<string>("CompanyCode"),
                CreatedOn = row.Field<DateTime>("CreatedOn")
            }).ToList<object>();
            result.Data = arrCustomer;
            result.Message = "";
            result.Status = "Success";
            return result;
        }

        public Result GetCustomerById(Customer customer)
        {
            result = new Result();
            string query=sql.ConcatString("SELECT * FROM tblCustomer WHERE Id =",customer.Id.ToString());
            DataTable dt = sql.ExecuteQuery(query);
            var arrCustomer = dt.AsEnumerable().Select(row => new
            {
                Id = row.Field<int>("Id"),
                FullName = row.Field<string>("FullName"),
                PhoneNo = row.Field<string>("PhoneNo"),
                Email = row.Field<string>("Email"),
                Address = row.Field<string>("Address"),
                CompanyCode = row.Field<string>("CompanyCode"),
                CreatedOn = row.Field<DateTime>("CreatedOn")
            }).ToList<object>();
            result.Data = arrCustomer;
            result.Message = "";
            result.Status = "Success";
            return result;
        }

        public string AddCustomer(Customer customer)
        {
            result = new Result();
            string query = sql.ConcatString("INSERT INTO tblCustomer (FullName, PhoneNo, Email, Address,CompanyCode,CreatedBy,CreatedOn) VALUES ",
                "('",customer.FullName, "','", customer.PhoneNo, "','", customer.Email, "','", customer.Address, "','", customer.CompanyCode, "','", customer.CreatedBy,
                "',",  "GetDate()); SELECT SCOPE_IDENTITY();");
            string newCustomerId= sql.ExecuteScalar<string>(query);
            customer.Id = Convert.ToInt32(newCustomerId);
            return newCustomerId;
        }

        public Result UpdateCustomer(Customer customer)
        {
            SqlCommand command = new SqlCommand("UPDATE tblCustomer SET FullName = @FullName, PhoneNo = @PhoneNo, Email = @Email, Address = @Address, UpdatedBy=@UpdatedBy, UpdatedOn=GetDate() WHERE Id = @Id and CompanyCode=@CompanyCode", sql.Connection);
            command.Parameters.AddWithValue("@Id", customer.Id);
            command.Parameters.AddWithValue("@FullName", customer.FullName);
            command.Parameters.AddWithValue("@PhoneNo", customer.PhoneNo);
            command.Parameters.AddWithValue("@Email", customer.Email);
            command.Parameters.AddWithValue("@Address", customer.Address);
            command.Parameters.AddWithValue("@CompanyCode", customer.CompanyCode);
            command.Parameters.AddWithValue("@UpdatedBy", customer.UpdatedBy);
            sql.ExecuteScalar<string>(command);
            //using (SqlConnection connection = new SqlConnection(connectionString))
            //{
            //    connection.Open();

            //    using (SqlCommand command = new SqlCommand("UPDATE tblCustomer SET FullName = @FullName, PhoneNo = @PhoneNo, Email = @Email, Address = @Address WHERE Id = @Id", connection))
            //    {
            //        command.Parameters.AddWithValue("@Id", customer.Id);
            //        command.Parameters.AddWithValue("@FullName", customer.FullName);
            //        command.Parameters.AddWithValue("@PhoneNo", customer.PhoneNo);
            //        command.Parameters.AddWithValue("@Email", customer.Email);
            //        command.Parameters.AddWithValue("@Address", customer.Address);

            //        command.ExecuteNonQuery();
            //    }
            //}
            result.Data = GetCustomerById(customer);
            result.Status = "Success";
            result.Message = "Data upated successfully";
            return result;
        }

        public Result DeleteCustomer(Customer customer)
        {
            SqlCommand command = new SqlCommand("DELETE FROM tblCustomer WHERE Id = @Id and CompanyCode=@CompanyCode", sql.Connection);
            command.Parameters.AddWithValue("@Id", customer.Id);
            command.Parameters.AddWithValue("@CompanyCode", customer.CompanyCode);
            int i = sql.ExecuteScalar<int>(command);
            //using (SqlConnection connection = new SqlConnection(connectionString))
            //{
            //    connection.Open();

            //    using (SqlCommand command = new SqlCommand("DELETE FROM tblCustomer WHERE Id = @Id", connection))
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