using Azure;
using Azure.Data.Tables;
using System;

namespace CityLibrary.Models
{
    public class Borrower : ITableEntity
    {
        // Borrower details
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int BooksBorrowed { get; set; }
        public double RatePerBook { get; set; }
        public double Fine { get; set; } = 100; // Default fine
        public double Total { get; private set; }

        // Azure Table Storage properties
        public string PartitionKey { get; set; } = "Borrowers";
        public string RowKey { get; set; } = Guid.NewGuid().ToString();
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        // Constructor to initialize borrower details
        public Borrower(string firstName, string lastName, int booksBorrowed, double ratePerBook)
        {
            FirstName = firstName;
            LastName = lastName;
            BooksBorrowed = booksBorrowed;
            RatePerBook = ratePerBook;
            UpdateTotal(); // Calculate the total when the object is created
        }

        // Parameterless constructor for Azure Table Storage
        public Borrower() { }

        // Method to calculate the total amount due
        private double CalculateTotal()
        {
            return (BooksBorrowed * RatePerBook) + Fine;
        }

        // Public method to update the total amount due
        public void UpdateTotal()
        {
            Total = CalculateTotal();
        }
    }
}
