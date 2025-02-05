using FribergsCarRental.Models;
using Microsoft.Identity.Client;

namespace FribergsCarRental.Helpers
{
    public class SessionHelper
    {
        private readonly IHttpContextAccessor httpContextAccesor;

        public SessionHelper(IHttpContextAccessor httpContextAccesor)
        {
            this.httpContextAccesor = httpContextAccesor;
        }

        public void SetUserSession(Role role, int id)
        {
            var session = httpContextAccesor.HttpContext.Session;
            session.SetInt32("Role", Convert.ToInt32(role));
            session.SetInt32("UserId", id);
        }

        public void SetCarSession(int id)
        {
            var session = httpContextAccesor.HttpContext.Session;
            session.SetInt32("CarId", id);
        }

        public (int? Role, int? Id) GetUserSession()
        {
            var session = httpContextAccesor.HttpContext.Session;
            var role = session.GetInt32("Role");
            var id = session.GetInt32("UserId");
            return (role, id);
        }

        public int? GetCarSession()
        {
            var session = httpContextAccesor.HttpContext.Session;
            var id = session.GetInt32("CarId");
            return id;
        }

        public void ClearSession()
        {
            httpContextAccesor.HttpContext.Session.Clear();
        }

        public void ClearCarSession()
        {
            httpContextAccesor.HttpContext.Session.Remove("CarId");
        }
    }
}
