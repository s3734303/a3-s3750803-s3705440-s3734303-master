using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Admin.Models;
using Admin.Data;

namespace Admin.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new NWBAContext(serviceProvider.GetRequiredService<DbContextOptions<NWBAContext>>());
            const string format = "dd/MM/yyyy hh:mm:ss tt";
            // Look for customers.
            if (context.Customer.Any())
                return; // DB has already been seeded.

            context.Customer.AddRange(
                new Customer
                {
                    CustomerID = 2100,
                    CustomerName = "Matthew Bolger",
                    CustAddress = "123 Fake Street",
                    CustCity = "Melbourne",
                    PostCode = "3000",
                    PhoneNum = "61423213242"
                },
                new Customer
                {
                    CustomerID = 2200,
                    CustomerName = "Rodney Cocker",
                    CustAddress = "456 Real Road",
                    CustCity = "Melbourne",
                    PostCode = "3005",
                    PhoneNum = "61423213242"
                },
                new Customer
                {
                    CustomerID = 2300,
                    CustomerName = "Shekhar Kalra",
                    PhoneNum = "61423213242"
                });

            context.Login.AddRange(
                new Login
                {
                    UserID = "12345678",
                    CustomerID = 2100,
                    Password = "YBNbEL4Lk8yMEWxiKkGBeoILHTU7WZ9n8jJSy8TNx0DAzNEFVsIVNRktiQV+I8d2",
                    ModifyDate = DateTime.ParseExact("19/12/2019 08:00:00 PM", format, null),
                    Flag = 0,
                    LoginStatus = false
                },
                new Login
                {
                    UserID = "38074569",
                    CustomerID = 2200,
                    Password = "EehwB3qMkWImf/fQPlhcka6pBMZBLlPWyiDW6NLkAh4ZFu2KNDQKONxElNsg7V04",
                    ModifyDate = DateTime.ParseExact("19/12/2019 08:00:00 PM", format, null),
                    Flag = 0,
                    LoginStatus = false
                },
                new Login
                {
                    UserID = "17963428",
                    CustomerID = 2300,
                    Password = "LuiVJWbY4A3y1SilhMU5P00K54cGEvClx5Y+xWHq7VpyIUe5fe7m+WeI0iwid7GE",
                    ModifyDate = DateTime.ParseExact("19/12/2019 08:00:00 PM", format, null),
                    Flag = 0,
                    LoginStatus = false
                });

            context.Account.AddRange(
                new Account
                {
                    AccountNumber = 4100,
                    AccountType = AccountType.S,
                    CustomerID = 2100,
                    Balance = 100,
                    ModifyDate = DateTime.ParseExact("19/12/2019 08:00:00 PM", format, null)
                },
                new Account
                {
                    AccountNumber = 4101,
                    AccountType = AccountType.C,
                    CustomerID = 2100,
                    Balance = 500,
                    ModifyDate = DateTime.ParseExact("19/12/2019 08:00:00 PM", format, null)
                },
                new Account
                {
                    AccountNumber = 4200,
                    AccountType = AccountType.S,
                    CustomerID = 2200,
                    Balance = 500.95m,
                    ModifyDate = DateTime.ParseExact("19/12/2019 08:00:00 PM", format, null)
                },
                new Account
                {
                    AccountNumber = 4300,
                    AccountType = AccountType.C,
                    CustomerID = 2300,
                    Balance = 1250.50m,
                    ModifyDate = DateTime.ParseExact("19/12/2019 08:00:00 PM", format, null)
                });

            const string openingBalance = "Opening balance";
            
            context.Transaction.AddRange(
                new Transaction
                {
                    TransactionType = TransactionType.Deposit,
                    AccountNumber = 4100,
                    Amount = 100,
                    Comment = openingBalance,
                    ModifyDate = DateTime.ParseExact("19/12/2019 08:00:00 PM", format, null)
                },
                new Transaction
                {
                    TransactionType = TransactionType.Deposit,
                    AccountNumber = 4101,
                    Amount = 500,
                    Comment = openingBalance,
                    ModifyDate = DateTime.ParseExact("19/12/2019 08:30:00 PM", format, null)
                },
                new Transaction
                {
                    TransactionType = TransactionType.Deposit,
                    AccountNumber = 4200,
                    Amount = 500.95m,
                    Comment = openingBalance,
                    ModifyDate = DateTime.ParseExact("19/12/2019 09:00:00 PM", format, null)
                },
                new Transaction
                {
                    TransactionType = TransactionType.Deposit,
                    AccountNumber = 4300,
                    Amount = 1250.50m,
                    Comment = "Opening balance",
                    ModifyDate = DateTime.ParseExact("19/12/2019 10:00:00 PM", format, null)
                });

            context.Payee.AddRange(
                new Payee
                {
                    PayeeId = 1000,
                    PayeeName = "Optus",
                    PhoneNum = "123456789"
                },
                new Payee
                {
                    PayeeId = 1001,
                    PayeeName = "Telstra",
                    PhoneNum = "987654321"
                });

            context.SaveChanges();
        }
    }
}
