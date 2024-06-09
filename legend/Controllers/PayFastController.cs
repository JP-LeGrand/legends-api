namespace legend.Controllers
{
    using legend.Entities.Enums;
    using legend.PayFast;
    using legend.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;

    [ApiController]
    [Route("[controller]")]
    public class PayFastController : Controller
    {
        #region Fields

        private readonly PayFastSettings payFastSettings;
        private readonly ILogger logger;
        private readonly IOrderService _orderService;

        #endregion Fields

        #region Constructor

        public PayFastController(
            IOptions<PayFastSettings> payFastSettings,
            ILogger<PayFastController> logger,
            IOrderService orderService)
        {
            this.payFastSettings = payFastSettings.Value;
            this.logger = logger;
            this._orderService = orderService;
        }

        #endregion Constructor

        [HttpPost]
        public async Task<ActionResult> Notify()
        {
            var formData = await Request.ReadFormAsync();

            var payFastNotifyViewModel = new PayFastNotify();

            // Populate PayFastNotify object with form data
            payFastNotifyViewModel.FromFormCollection(formData);
            payFastNotifyViewModel.SetPassPhrase(this.payFastSettings.PassPhrase);

            var calculatedSignature = payFastNotifyViewModel.GetCalculatedSignature();

            var isValid = payFastNotifyViewModel.signature == calculatedSignature;

            this.logger.LogInformation($"Signature Validation Result: {isValid}");

            // The PayFast Validator is still under developement
            // Its not recommended to rely on this for production use cases
            var payfastValidator = new PayFastValidator(this.payFastSettings, payFastNotifyViewModel, this.HttpContext.Connection.RemoteIpAddress);

            var merchantIdValidationResult = payfastValidator.ValidateMerchantId();

            this.logger.LogInformation($"Merchant Id Validation Result: {merchantIdValidationResult}");

            var ipAddressValidationResult = await payfastValidator.ValidateSourceIp();

            this.logger.LogInformation($"Ip Address Validation Result: {ipAddressValidationResult}");

            // Currently seems that the data validation only works for success
            if (payFastNotifyViewModel.payment_status == PayFastStatics.CompletePaymentConfirmation)
            {
                var dataValidationResult = await payfastValidator.ValidateData();

                this.logger.LogInformation($"Data Validation Result: {dataValidationResult}");

                Guid.TryParse(payFastNotifyViewModel.item_name, out Guid orderId);

                await _orderService.UpdateOrderStatusAsync(orderId, OrderStatus.Processed);

                this.logger.LogInformation($"Update Order {orderId} to Status: {OrderStatus.Processed}");
            }

            if (payFastNotifyViewModel.payment_status == PayFastStatics.CancelledPaymentConfirmation)
            {
                this.logger.LogInformation($"Subscription was cancelled");
            }

            return Ok();
        }
    }
}

