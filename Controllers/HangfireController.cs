using System;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace background_processing.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class HangfireController : ControllerBase
    {
        // POST
        [HttpPost]
        public IActionResult Welcome()
        {
            var jobId = BackgroundJob.Enqueue(
                () => SendWelcomeEmail("Welcome to our app.")
                );

            return Ok($" Job Id : {jobId} - Welcome email sent to the user.");
        }

        // POST
        [HttpPost]
        public IActionResult Discount()
        {
            int timeInSeconds = 30;
            var jobId = BackgroundJob.Schedule(
                () => DiscountEmail(""),
                TimeSpan.FromSeconds(timeInSeconds)
                );

            return Ok($" Job Id : {jobId} - Discount email will be sent in {timeInSeconds} seconds to the user.");
        }
        
        // POST
        [HttpPost]
        public IActionResult DatabaseUpdate()
        {
            RecurringJob.AddOrUpdate(() => Console.WriteLine("Database updated.") , Cron.Minutely);
            return Ok("Database check job initiated.");
            
        }
        
        // POST
        [HttpPost]
        public IActionResult Confirm()
        {
            int timeInSeconds = 30;
            var parentJobId = BackgroundJob.Schedule(
                () => Console.WriteLine("You asked to be unsubscribed."),
                TimeSpan.FromSeconds(timeInSeconds)
            );
            BackgroundJob.ContinueJobWith(parentJobId, () => Console.WriteLine("You were unsubscribe."));
            
            return Ok("Confirmation job created.");
        }
        


        // ↓ Jobs
        public void SendWelcomeEmail(string text)
        {
            Console.WriteLine(text);
        }

        public void DiscountEmail(string text)
        {
            Console.WriteLine(text);
        }

        // ↑ Jobs
    }
}