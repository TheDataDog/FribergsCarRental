using FribergsCarRental.Data;
using FribergsCarRental.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FribergsCarRental.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerRepository customerRepository;
        private readonly SessionHelper sessionHelper;

        public CustomerController(ICustomerRepository customerRepository, SessionHelper sessionHelper)
        {
            this.customerRepository = customerRepository;
            this.sessionHelper = sessionHelper;
        }
        // GET: CustomerController
        [HttpGet]
        public ActionResult Index()
        {
            return View(customerRepository.GetAll());
        }

        // GET: CustomerController/Details/5
        [HttpGet]
        public ActionResult Details(int id)
        {
            return View(customerRepository.GetById(id));
        }

        // GET: CustomerController/Create
        [HttpGet]
        public ActionResult Create()
        {
            var customer = new Customer
            {
                UserRole = new UserRole { Role = (Role.Customer) }
            };

            ViewBag.User = "Null";
            var user = sessionHelper.GetUserSession();

            if (user.Role == 0)
            {
                ViewBag.User = "Admin";
            }
            if (sessionHelper.GetCarSession() == null)
            {
                return View(customer);
            }
            else
            {
                return PartialView(customer);
            }
        }

        // POST: CustomerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Customer customer)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    var addedCustomer = customerRepository.Add(customer); //returnera en customer här för att få customerId?
                    SetUserSession(addedCustomer.UserRole.Role, addedCustomer.CustomerId);
                }
                if (customer.UserRole.Role == Role.Admin)
                {
                    return RedirectToAction(nameof(Index));
                }
                else //(customer.UserRole.Role == Role.Customer)
                {
                    return RedirectToAction("Create", "Booking");
                }
                //else
                //{
                //    return View("Error"); //Vad vill jag ha här???
                //}
            }
            catch
            {
                return View();
            }
        }

        // GET: CustomerController/Edit/5
        [HttpGet]
        public ActionResult Edit(int id)
        {
            return View(customerRepository.GetById(id));
        }

        // POST: CustomerController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Customer customer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    customerRepository.Update(customer);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CustomerController/Delete/5
        [HttpGet]
        public ActionResult Delete(int id)
        {
            return View(customerRepository.GetById(id));
        }

        // POST: CustomerController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Customer customer)
        {
            try
            {
                customerRepository.Delete(customer);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult Login()
        {
            if(sessionHelper.GetCarSession() == null)
            {
                return View();
            }
            else
            {
                return PartialView();
            }
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            var customer = customerRepository.GetByEmail(email);
            if (customer == null || customer.Password != password)
            {
                ModelState.AddModelError("", "Ogiltig email eller lösenord");
                return View();
            }
            SetUserSession(customer.UserRole.Role, customer.CustomerId);

            if (sessionHelper.GetCarSession() == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Create", "Booking");
            }

        }

        [HttpGet]
        public ActionResult LoginOrRegister()
        {
            return View();
        }

        public void SetUserSession(Role role, int id)
        {
            sessionHelper.SetUserSession(role, id);
        }
        [HttpGet]
        public ActionResult Logout()
        {
            sessionHelper.ClearUserSession();
            return RedirectToAction("Index", "Home");
        }
    }
}
