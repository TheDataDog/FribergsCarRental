using FribergsCarRental.Models;

namespace FribergsCarRental.Helpers
{
	public interface ISessionHelper
	{
		void SetUserSession(Role role, int id);
		void SetCarSession(int id);
		(int? Role, int? Id) GetUserSession();
		int? GetCarSession();
		void ClearSession();
		void ClearCarSession();

	}
}
