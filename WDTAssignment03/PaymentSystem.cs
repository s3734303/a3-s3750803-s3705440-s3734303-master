using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Admin.Data;
using Admin.Models;
using System.Collections.Generic;
using WDTAssignment03.Utilities;

namespace WDTAssignment03
{
    public class PaymentService
    {
        private NWBAContext context;
        private IServiceProvider _serviceProvider;
        private Timer _timer;
        public PaymentService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            var t = Task.Run(() => Execute());
        }

        public async Task Execute()
        {
            context = new NWBAContext(_serviceProvider.GetRequiredService<DbContextOptions<NWBAContext>>());
            while (true)
            {
                PaymentAsync(context);
                await Task.Delay(TimeSpan.FromSeconds(1));
            }



        }
        public async Task PaymentAsync(NWBAContext context)
        {
            IEnumerable<BillPay> billPays = context.BillPay;
            foreach (BillPay billPay in billPays)
            {
                if (DateTime.UtcNow.Minute == billPay.ScheduleDate.Minute && billPay.Active)
                {
                    var account = context.Account.Find(billPay.Account);
                    if (MiscellaneousExtensionUtilities.BalanceCheck(account, billPay.Amount, TransactionType.BillPay))

                    new Transaction
                    {
                        TransactionType = TransactionType.BillPay,
                        Account = billPay.Account,
                        Amount = billPay.Amount,
                        Comment = billPay.Payee.PayeeName,
                        ModifyDate = DateTime.UtcNow
                    };

                    account.Balance -= billPay.Amount;
                }
                switch (billPay.Period)
                {
                    case (PeriodType.M):
                        billPay.ScheduleDate.AddMonths(1);
                        break;
                    case (PeriodType.Q):
                        billPay.ScheduleDate.AddMonths(3);
                        break;
                    case (PeriodType.Y):
                        billPay.ScheduleDate.AddYears(1);
                        break;
                }
                context.Update(billPay);
                await context.SaveChangesAsync();

            }
        }


    }
}