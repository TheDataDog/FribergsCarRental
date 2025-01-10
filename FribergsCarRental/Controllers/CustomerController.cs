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
            if (sessionHelper.GetCarSession() == null)
            {
                var model = new Customer
                {
                    UserRole = new UserRole { Role = (Role.Customer) }
                };
                return View(model);
            }
            else
            {
                return PartialView();
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
                    customerRepository.Add(customer);
                }
                return RedirectToAction(nameof(Index));
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
            var customer = customerRepository.GetById(id);
            return View(customer);
            //return View(customerRepository.GetById(id));
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
                ModelState.AddModelError("", "Invalid email or password"); //funkar ej?
                return View("Error");
            }
            SetUserSession(customer.UserRole.Role, customer.CustomerId); //Ha kvar som egen metod?

            if (sessionHelper.GetCarSession() == null)
            {
                return RedirectToAction("Index", "Home"); //eller till sidan ne var på innan Login? 
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
