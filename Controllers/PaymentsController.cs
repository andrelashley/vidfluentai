using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using Stripe;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using VidFluentAI.Models;

namespace VidFluentAI.Controllers
{
    [Authorize]
    public class PaymentsController : Controller
    {
        private readonly IConfiguration _config;
        private readonly IStripeClient _client;
        private readonly UserManager<ApplicationUser> _userManager;
        public PaymentsController(IConfiguration config, UserManager<ApplicationUser> userManager)
        {
            _config = config;
            _client = new StripeClient(_config["StripeApiSecret"]);
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByNameAsync(User!.Identity!.Name);
            if (user.IsSubscriptionActive == true) return RedirectToAction(nameof(AlreadySubscribed));

            ViewBag.PriceId = _config["StripePriceId"];

            return View();
        }

        [HttpPost("create-checkout-session")]
        public async Task<IActionResult> CreateCheckoutSession()
        {
            var user = await _userManager.FindByNameAsync(User!.Identity!.Name);
            var options = new SessionCreateOptions
            {
                SuccessUrl = $"{_config["BaseUrl"]}/Payments/Success?session_id={{CHECKOUT_SESSION_ID}}",
                CancelUrl = $"{_config["BaseUrl"]}/Payments/Canceled",
                Mode = "subscription",
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        Price = Request.Form["priceId"],
                        Quantity = 1,
                    },
                },
                CustomerEmail = user.UserName
                // AutomaticTax = new SessionAutomaticTaxOptions { Enabled = true },
            };
            var service = new SessionService(_client);
            try
            {
                var session = await service.CreateAsync(options);
                Response.Headers.Add("Location", session.Url);
                return new StatusCodeResult(303);
            }
            catch (StripeException)
            {
                // Console.WriteLine(e.StripeError.Message);
                return BadRequest();
            }
        }

        public IActionResult Success([FromQuery] string sessionId)
        {
            return View();
        }

        public IActionResult Canceled()
        {
            return View();
        }

        public IActionResult AlreadySubscribed()
        {
            return View();
        }

        [HttpGet("checkout-session")]
        public async Task<IActionResult> CheckoutSession(string sessionId)
        {
            var service = new SessionService(_client);
            var session = await service.GetAsync(sessionId);
            return Ok(session);
        }
    }
}
