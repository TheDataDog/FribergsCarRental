using FribergsCarRental.Data;
using FribergsCarRental.Models;
using FribergsCarRental.ViewModels;
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

            ViewBag.User = "Null"; //behöver denna deklareras här???
            var user = GetUserSession();

            if (user.Role == 0)
            {
                ViewBag.User = "Admin";
            }
            //if (sessionHelper.GetCarSession() == null)
            //{
            //    return View(customer);
            //}
            //else
            //{
            //    return PartialView(customer);
            //}
            return View(customer);
        }

        // POST: CustomerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Customer customer)
        {
            var user = GetUserSession();

            try
            {
                if (ModelState.IsValid)
                {
                    var addedCustomer = customerRepository.Add(customer);
                    if (user.Role == null)
                    {
                        SetUserSession(addedCustomer.UserRole.Role, addedCustomer.CustomerId);
                        return RedirectToAction("Create", "Booking");
                    }

                    return RedirectToAction(nameof(Index));

                }
                else
                {
                    return View();
                }
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
            //if(sessionHelper.GetCarSession() == null)
            //{
            //    return View();
            //}
            //else
            //{
            //    return PartialView();
            //}
            return View();

        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var customer = customerRepository.GetByEmail(model.Email);
            if (customer == null || customer.Password != model.Password)
            {
                ModelState.AddModelError("", "Fel email eller lösenord");
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

        [HttpGet]
        public ActionResult Logout()
        {
            sessionHelper.ClearUserSession();
            return RedirectToAction("Index", "Home");
        }
        public void SetUserSession(Role role, int id)
        {
            sessionHelper.SetUserSession(role, id);
        }

        public (int? Role, int? Id) GetUserSession()
        {
            return sessionHelper.GetUserSession();
        }
    }
}
