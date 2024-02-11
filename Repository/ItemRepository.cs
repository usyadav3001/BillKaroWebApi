using StudioWebApi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace StudioWebApi.Repository
{
    public class ItemRepository : IItemRepository
    {
        private readonly string connectionString;
        Sql sql = new Sql();
        Result result = new Result();
        public ItemRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public Result GetAllItems(Item item)
        {
            List<Customer> arrC = new List<Customer>();
            result = new Result();
            string query = @"SELECT i.Id,i.ItemName,i.Price,i.UnitId,u.UnitName,i.CreatedOn FROM tblItem i
                    LEFT JOIN tblUnit u ON i.UnitId=U.Id WHERE i.CompanyCode="+item.CompanyCode;
            DataTable dt = sql.ExecuteQuery(query);
            var arrCustomer = dt.AsEnumerable().Select(row => new
            {
                Id = row.Field<int>("Id"),
                ItemName = row.Field<string>("ItemName"),
                Price = row.Field<int>("Price"),
                UnitId = row.Field<int>("UnitId"),
                UnitName = row.Field<string>("UnitName"),
                CreatedOn = row.Field<DateTime>("CreatedOn")
            }).ToList<object>();
            result.Data = arrCustomer;
            result.Message = "";
            result.Status = "Success";
            return result;
        }
        public Result GetItemById(Item item)
        {
            result = new Result();
            string query = sql.ConcatString(@"SELECT i.Id,i.ItemName,i.Price,i.CreatedOn,i.UnitId,u.UnitName FROM tblItem i
                    LEFT JOIN tblUnit u ON i.UnitId=U.Id WHERE i.Id =", item.Id.ToString(), " and i.CompanyCode="+item.CompanyCode);
            DataTable dt = sql.ExecuteQuery(query);
            var arrCustomer = dt.AsEnumerable().Select(row => new
            {
                Id = row.Field<int>("Id"),
                ItemName = row.Field<string>("ItemName"),
                Price = row.Field<int>("Price"),
                UnitId = row.Field<int>("UnitId"),
                UnitName = row.Field<string>("UnitName"),
                CreatedOn = row.Field<DateTime>("CreatedOn")
            }).ToList<object>();
            result.Data = arrCustomer;
            result.Message = "";
            result.Status = "Success";
            return result;
        }
        public string AddItem(Item item)
        {
            result = new Result();
            string query = sql.ConcatString("INSERT INTO tblItem (ItemName, Price, UnitId, CompanyCode, CreatedBy, CreatedOn) VALUES ",
                "('", item.ItemName, "','", item.Price, "','", item.UnitId.ToString(), "','", item.CompanyCode, "','", item.CreatedBy, "',", "GetDate()); SELECT SCOPE_IDENTITY();");
            string newCustomerId = sql.ExecuteScalar<string>(query);
            item.Id = Convert.ToInt32(newCustomerId);
            return newCustomerId;
        }
        public Result UpdateItem(Item item)
        {
            SqlCommand command = new SqlCommand("UPDATE tblItem SET ItemName = @ItemName, Price = @Price, UnitId = @UnitId, " +
                "CompanyCode = @CompanyCode, UpdatedBy = @UpdatedBy, UpdatedOn = GetDate() WHERE Id = @Id and CompanyCode=@CompanyCode", sql.Connection);
            command.Parameters.AddWithValue("@Id", item.Id);
            command.Parameters.AddWithValue("@ItemName", item.ItemName);
            command.Parameters.AddWithValue("@Price", item.Price);
            command.Parameters.AddWithValue("@UnitId", item.UnitId);
            command.Parameters.AddWithValue("@CompanyCode", item.CompanyCode);
            command.Parameters.AddWithValue("@UpdatedBy", item.UpdatedBy);
            sql.ExecuteScalar<string>(command);
            result.Data = GetItemById(item);
            result.Message = "Data upated successfully";
            return result;
        }
        public Result DeleteItem(Item item)
        {
            SqlCommand command = new SqlCommand("DELETE FROM tblItem WHERE Id = @Id and CompanyCode=@CompanyCode", sql.Connection);
            command.Parameters.AddWithValue("@Id", item.Id);
            command.Parameters.AddWithValue("@CompanyCode", item.CompanyCode);
            int i = sql.ExecuteScalar<int>(command);
            result.Message = "Data deleted successfully";
            return result;
        }

        public Result GetAllUnits(Unit unit)
        {
            List<Customer> arrC = new List<Customer>();
            result = new Result();
            string query = "SELECT * FROM tblUnit Where CompanyCode="+unit.CompanyCode;
            DataTable dt = sql.ExecuteQuery(query);
            var arrCustomer = dt.AsEnumerable().Select(row => new
            {
                Id = row.Field<int>("Id"),
                UnitName = row.Field<string>("UnitName"),
                CreatedOn = row.Field<DateTime>("CreatedOn")
            }).ToList();
            result.Data = arrCustomer;
            result.Message = "";
            result.Status = "Success";
            return result;
        }

        public Result GetUnitById(Unit unit)
        {
            result = new Result();
            string query = sql.ConcatString("SELECT * FROM tblUnit WHERE Id =", unit.Id.ToString()," and CompanyCode="+unit.CompanyCode);
            DataTable dt = sql.ExecuteQuery(query);
            var arrCustomer = dt.AsEnumerable().Select(row => new
            {
                Id = row.Field<int>("Id"),
                UnitName = row.Field<string>("UnitName"),
                CreatedOn = row.Field<DateTime>("CreatedOn")
            }).ToList();
            result.Data = arrCustomer;
            result.Message = "";
            result.Status = "Success";
            return result;
        }
        public string AddUnit(Unit unit)
        {
            result = new Result();
            string query = sql.ConcatString("INSERT INTO tblUnit (UnitName, CompanyCode, CreatedBy, CreatedOn) VALUES",
                "('", unit.UnitName, "','", unit.CompanyCode, "','", unit.CreatedBy, "',", "GetDate()); SELECT SCOPE_IDENTITY();");
            string newUnitId = sql.ExecuteScalar<string>(query);
            unit.Id = Convert.ToInt32(newUnitId);
            return newUnitId;
        }

        public Result UpdateUnit(Unit unit)
        {
            SqlCommand command = new SqlCommand("UPDATE tblUnit SET UnitName = @UnitName, UpdatedBy = @UpdatedBy, UpdatedOn = GetDate() WHERE Id = @Id and CompanyCode=@CompanyCode", sql.Connection);
            command.Parameters.AddWithValue("@Id", unit.Id);
            command.Parameters.AddWithValue("@UnitName", unit.UnitName);
            command.Parameters.AddWithValue("@UpdatedBy", unit.UpdatedBy);
            command.Parameters.AddWithValue("@CompanyCode", unit.CompanyCode);
            sql.ExecuteScalar<string>(command);
            result.Message = "Data upated successfully";
            return result;
        }
        public Result DeleteUnit(Unit unit)
        {
            SqlCommand command = new SqlCommand("DELETE FROM tblUnit WHERE Id = @Id and CompanyCode=@CompanyCode", sql.Connection);
            command.Parameters.AddWithValue("@Id", unit.Id);
            command.Parameters.AddWithValue("@CompanyCode", unit.CompanyCode);
            int i = sql.ExecuteScalar<int>(command);
            result.Message = "Data deleted successfully";
            return result;
        }


    }
}
