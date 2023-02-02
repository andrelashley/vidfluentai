using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using VidFluentAI.Models;

namespace VidFluentAI.Controllers.Api
{
    [Route("api/webhook")]
    [ApiController]
    public class WebhookController : ControllerBase
    {
        private string _endpointSecret = "";
        private readonly IConfiguration _config;
        private readonly UserManager<ApplicationUser> _userManager;

        public WebhookController(IConfiguration config, UserManager<ApplicationUser> userManager)
        {
            _config = config;
            _endpointSecret = _config["StripeWebhookSecret"];
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Index()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], _endpointSecret);

                if (stripeEvent.Type == Events.CheckoutSessionCompleted)
                {
                    var session = stripeEvent.Data.Object as Session;

                    var user = await _userManager.FindByNameAsync(session!.CustomerEmail);
                    user.CustomerId = session.CustomerId;
                    user.SubscriptionId = session.SubscriptionId;
                    user.IsSubscriptionActive = true;

                    await _userManager.UpdateAsync(user);
                }
                else
                {
                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                }

                return Ok();
            }
            catch (StripeException)
            {
                return BadRequest();
            }
        }
    }
}
