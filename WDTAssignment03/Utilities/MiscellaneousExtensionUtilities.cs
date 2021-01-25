using System;
using System.Linq;
using Admin.Models;

namespace WDTAssignment03.Utilities
{
    public static class MiscellaneousExtensionUtilities
    {
        public static bool HasMoreThanNDecimalPlaces(this decimal value, int n) => decimal.Round(value, n) != value;
        public static bool HasMoreThanTwoDecimalPlaces(this decimal value) => value.HasMoreThanNDecimalPlaces(2);
        public static bool BalanceCheck(Account account, decimal amount, TransactionType transactionType)
        {
            decimal fee = 0;
            switch (transactionType)
            {
                case (TransactionType.Withdraw):
                    fee = Convert.ToDecimal(0.1);
                    break;
                case (TransactionType.Transfer):
                    fee = Convert.ToDecimal(0.2);
                    break;
                case (TransactionType.BillPay):
                    fee = 0;
                    break;
                default:
                    return false;
            }
            var counter = account.Transactions.Count(n => n.TransactionType == TransactionType.Transfer)
                + account.Transactions.Count(n => n.TransactionType == TransactionType.Withdraw);
            if (amount <= 0)
                return false;
            if ((account.AccountType == AccountType.C) && ((account.Balance - amount - fee < 200) || (account.Balance - amount < 200 && counter < 4)))
                return false;
            if ((amount + fee >= account.Balance) || (amount >= account.Balance && counter < 4))
                return false;
            return true;
        }

    }
}
