﻿using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;

namespace BusinessLogic.Interfaces
{
    public class EmailService
    {

        private readonly PeriodicTimer _periodicTimer = new PeriodicTimer(TimeSpan.FromDays(1));
        private readonly IServiceScopeFactory scopeFactory;
        private readonly IConfiguration config;

        private readonly DateTime sendingTime = DateTime.Now.Date.AddHours(21).AddMinutes(42);

        public CancellationToken token { get; set; }

        public EmailService(IServiceScopeFactory scopeFactory, IConfiguration config)
        {

            CancellationTokenSource source = new CancellationTokenSource();
            token = source.Token;
            this.scopeFactory = scopeFactory;
            this.config = config;
            startEmailService(token);
        }

        private async Task startEmailService(CancellationToken token)
        {
            var time = DateTime.Now;
            var compare = time.CompareTo(sendingTime);


            if (compare >= 0)
            {
                
                while (await _periodicTimer.WaitForNextTickAsync(token) && !token.IsCancellationRequested)
                {
                    await sendEmails();
                }

            }
            else
            {
                var helperTimer = new PeriodicTimer(sendingTime - time);
                if (await helperTimer.WaitForNextTickAsync(token))
                {
                    await sendEmails();
                    while (await _periodicTimer.WaitForNextTickAsync(token) && !token.IsCancellationRequested)
                    {
                        await sendEmails();
                    }

                }
            }
        }

        private async Task sendEmails()
        {

            var ourEmail = "parking@gmail.com";

            var password = "mustUsePassword";

            var client = new SmtpClient(config.GetSection("SMTPConnection").GetValue<string>("host"), config.GetSection("SMTPConnection").GetValue<int>("port"))
            {
                UseDefaultCredentials = true
            };
            var msg = "You have a reservation tomorrow";



            var sender = new MailAddress(ourEmail);

            using (var scope = scopeFactory.CreateScope())
            {
                var parkingContext = scope.ServiceProvider.GetRequiredService<ParkingContext>();

                var reservationsQuerry = from reservation in parkingContext.Reservations
                                         where reservation.Beginning.Date.CompareTo(DateTime.Now.Date.AddDays(1)) == 0
                                         select reservation;

                var reservationsList = await reservationsQuerry.Include("User").ToListAsync();

                foreach (var reservation in reservationsList)
                {

                    var mm = new MailMessage(sender, new MailAddress(reservation.User.Email));
                    mm.Subject = "Reservation";
                    mm.Body = msg;
                    client.SendAsync(mm, mm);
                    
                }
            }
        }
    }
}
