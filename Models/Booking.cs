using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudioWebApi.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Discription { get; set; }
        public int TotalQuanity { get; set; }
        public Decimal TotalDiscountAmount { get; set; }
        public Decimal TotalTaxAmount { get; set; }
        public Decimal TotalAmount { get; set; }
        public Decimal GrandDiscountAmount { get; set; }
        public Decimal GrandTotalAmount { get; set; }
        public string CompanyCode { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public List<BookingDetail> BookingData { get; set; }

    }

    public class BookingDetail
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public int UnitId { get; set; }
        public Decimal Price { get; set; }
        public Decimal DiscountPercentage { get; set; }
        public Decimal DiscountAmount { get; set; }
        public Decimal GrandDiscountAmount { get; set; }
        public Decimal TaxPercentage { get; set; }
        public Decimal TaxAmount { get; set; }
        public Decimal TotalAmount { get; set; }
        public string CompanyCode { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }

    }
}
