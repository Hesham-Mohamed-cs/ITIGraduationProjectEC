using GraduaitionProjectITI.Models;
using GraduaitionProjectITI.Models.Entity;
using GraduaitionProjectITI.Reprository.BrandRepository;
using GraduaitionProjectITI.Reprository.CategoryReprositry;
using GraduaitionProjectITI.Reprository.OrderReprository;
using GraduaitionProjectITI.Reprository.ProductReprository;
using GraduaitionProjectITI.Reprository.SubOrderReprository;
using GraduaitionProjectITI.ViewModel;
using GraduaitionProjectITI.ViewModel.Cart;
using GraduaitionProjectITI.ViewModel.ConfirmOrder;
using GraduaitionProjectITI.ViewModel.Products;
using GraduaitionProjectITI.ViewModel.profile;
using GraduaitionProjectITI.ViewModel.SubOrders;
using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using NuGet.Protocol.Core.Types;
using PayPal.Api;
using System.Diagnostics;
using System.Text.Json;
using static QuestPDF.Helpers.Colors;

namespace GraduaitionProjectITI.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductReprository reprositry;
        private readonly ICategoryReprositry reprositrycaty;
        private readonly IBrandRepository reprositryBrand;
        private readonly IOrderReprository reprositryOrder;
        private readonly ISubOrderReprository reprositrySubOrder;
        private static List<int> _productsSelected;
        private static List<ProductsWithCount> _productsWithCount;
        private readonly UserManager<ApplicationUser> userManger;
        private IHttpContextAccessor httpContextAccessor;
        IConfiguration _configuration;
        //string Total = "1.00" ;

        public CartController(IProductReprository reprositry, ICategoryReprositry reprositrycaty, IBrandRepository reprositryBrand,
            UserManager<ApplicationUser> userManager, IOrderReprository reprositryOrder,
            ISubOrderReprository reprositrySubOrder, IHttpContextAccessor context, IConfiguration iconfiguration)
        {
            this.reprositry = reprositry;
            this.reprositrycaty = reprositrycaty;
            this.reprositryBrand = reprositryBrand;
            this.userManger = userManager;
            this.reprositryOrder = reprositryOrder;
            this.reprositrySubOrder = reprositrySubOrder;
            httpContextAccessor = context;
            _configuration = iconfiguration;
        }

        public IActionResult Index()
        {
            List<int> prodsIds = _productsSelected;
            if (_productsSelected != null)
            {
                IEnumerable<Product> products = reprositry.GetProducts(prodsIds);
                return View(products);
            }
            return RedirectToAction("Index");
        }



        [HttpPost]
        public IActionResult GetIDs([FromBody] GetProducts model)
        {
            if (model.Ids is null)
            {
                _productsSelected = new List<int>();
                return RedirectToAction("Index");

            }
            else { 
                List<int>? productsSelected = JsonConvert.DeserializeObject<List<int>>(model.Ids.TrimEnd(','));
                _productsSelected = productsSelected;

                return RedirectToAction("Index");
            }
        }


        [HttpPost]

        public IActionResult Cart([FromBody] List<ProductData> products)
        {
            List<ProductsWithCount> productsWithCountsList = new List<ProductsWithCount>();
            foreach (var product in products)
            {
                Product product1 = reprositry.GetProduct(product.Id);
                int count = product.Count;
                ProductsWithCount productsWithCount = new ProductsWithCount(product1, count);
                productsWithCountsList.Add(productsWithCount);
            }
            _productsWithCount = productsWithCountsList;
            return RedirectToAction("Payment");
        }

        [HttpGet]
        public async Task<IActionResult> PaymentAsync()
        {
            var user = await userManger.FindByEmailAsync(User.Identity.Name);
            List<ProductsWithCount> productsWithCounts = _productsWithCount;
            if (_productsWithCount != null)
            {
                ViewData["UserLogged"] = user;
                //double price =0;
                //foreach (var p in productsWithCounts)
                //{
                //    price+= p.product.Price* p.Count;
                //}
                //Total = price.ToString();

                return View(productsWithCounts);
            }
            return BadRequest();                          
                

        }

        [HttpPost]
        public async Task<IActionResult> Payment(UserOrdersDataViewModel OrderData)
        {
            var order = new AddOrderViewModel();

            order.Cost = OrderData.TotalCost;
            order.ResevationDate= DateTime.Now;
            order.DeliveryDate = DateTime.Now.AddDays(7);
            order.UserId = OrderData.UserID;
            order.destination = OrderData.Destination;
            order.IsConfirmed = true;
            var myOrder = reprositryOrder.InsertNew(order);

            var subOrderList = new List<AddSubOrderViewModel>();

            foreach (var product in OrderData.ProductData) 
            {
                var subOrder = new AddSubOrderViewModel();
                subOrder.ProductId = product.Id;
                subOrder.OrderId = myOrder.Id;
                subOrder.Amount = product.Count;
                subOrderList.Add(subOrder);
                reprositrySubOrder.Insert(subOrder);
                var prod = reprositry.GetProduct(subOrder.ProductId);
                prod.Count -= subOrder.Amount;
                reprositry.Save();
                reprositrySubOrder.Save();
            }
            var user = await userManger.FindByEmailAsync(User.Identity.Name);
            if (myOrder.User.Id == user.Id)
            {

            return RedirectToAction("Receipt", new { orderId = myOrder.Id,userID= user.Id});
            }
            return BadRequest();
        }

        [Authorize]
        public async Task<IActionResult> ReceiptAsync(int orderId,string userId)
        {
            var user = await userManger.FindByEmailAsync(User.Identity.Name);
            var order = reprositryOrder.GetOrderWithSubOrders(orderId);
            if (order == null)
            {
                return BadRequest();
            }else if(userId == user.Id)
            {
                
                ViewData["SubOrders"] = order.subOrders;
                return View(order);

            }
            else
            {
                return BadRequest();

            }
        }

        public async Task<IActionResult> PaymentWithPayPal(string Cancel = null, string blogId = "", string PayerID = "", string guid = "",string totalPrice="1.00")
        {
            //getting the apiContext  
            var ClientID = _configuration.GetValue<string>("PayPal:Key");
            var ClientSecret = _configuration.GetValue<string>("PayPal:Secret");
            var mode = _configuration.GetValue<string>("PayPal:mode");
            APIContext apiContext = PaypalConfiguration.GetAPIContext(ClientID, ClientSecret, mode);
            // apiContext.AccessToken="Bearer access_token$production$j27yms5fthzx9vzm$c123e8e154c510d70ad20e396dd28287";
            try
            {
                //A resource representing a Payer that funds a payment Payment Method as paypal  
                //Payer Id will be returned when payment proceeds or click to pay  
                string payerId = PayerID;
                if (string.IsNullOrEmpty(payerId))
                {
                    //this section will be executed first because PayerID doesn't exist  
                    //it is returned by the create function call of the payment class  
                    // Creating a payment  
                    // baseURL is the url on which paypal sendsback the data.  
                    string baseURI = this.Request.Scheme + "://" + this.Request.Host + "/Cart/PaymentWithPayPal";
                    //here we are generating guid for storing the paymentID received in session  
                    //which will be used in the payment execution  
                    var guidd = Convert.ToString((new Random()).Next(100000));
                    guid = guidd;
                    //CreatePayment function gives us the payment approval url  
                    //on which payer is redirected for paypal account payment  
                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid, blogId, totalPrice);
                    //get links returned from paypal in response to Create function call  
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment  
                            paypalRedirectUrl = lnk.href;
                        }
                    }
                    // saving the paymentID in the key guid  
                    httpContextAccessor.HttpContext.Session.SetString("payment", createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This function exectues after receving all parameters for the payment  

                    var paymentId = httpContextAccessor.HttpContext.Session.GetString("payment");
                    var executedPayment = ExecutePayment(apiContext, payerId, paymentId as string);
                    //If executed payment failed then we will show payment failure message to user  
                    if (executedPayment.state.ToLower() != "approved")
                    {

                        return View("PaymentFailed");
                    }
                    var blogIds = executedPayment.transactions[0].item_list.items[0].sku;


                    return View("PaymentSuccess");
                }
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
            //on successful payment, show success page to user.  
            return View("SuccessView");
        }
        private PayPal.Api.Payment payment;
        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            this.payment = new Payment()
            {
                id = paymentId
            };
            return this.payment.Execute(apiContext, paymentExecution);
        }
        private Payment CreatePayment(APIContext apiContext, string redirectUrl, string blogId, string totalPrice)
        {
            //create itemlist and add item objects to it  

            var itemList = new ItemList()
            {
                items = new List<Item>()
            };
            //Adding Item Details like name, currency, price etc  
            itemList.items.Add(new Item()
            {
                name = "Item Detail",
                currency = "USD",
                price = (double.Parse(totalPrice)/40).ToString(),
                quantity = "1",
                sku = "asd"
            });
            var payer = new Payer()
            {
                payment_method = "paypal"
            };
            // Configure Redirect Urls here with RedirectUrls object  
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };
            // Adding Tax, shipping and Subtotal details  
            //var details = new Details()
            //{
            //    tax = "1",
            //    shipping = "1",
            //    subtotal = "1"
            //};
            //Final amount with details  
            var amount = new Amount()
            {
                currency = "USD",
                total = (double.Parse(totalPrice) / 40).ToString(), // Total must be equal to sum of tax, shipping and subtotal.  
                //details = details
            };
            var transactionList = new List<Transaction>();
            // Adding description about the transaction  
            transactionList.Add(new Transaction()
            {
                description = "Transaction description",
                invoice_number = Guid.NewGuid().ToString(), //Generate an Invoice No  
                amount = amount,
                item_list = itemList
            });
            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };
            // Create a payment using a APIContext  
            return this.payment.Create(apiContext);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}

