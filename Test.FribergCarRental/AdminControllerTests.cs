using FribergsCarRental.Controllers;
using FribergsCarRental.Data;
using FribergsCarRental.Helpers;
using FribergsCarRental.Models;
using FribergsCarRental.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Test.FribergCarRental
{
	public class AdminControllerTests
	{
		private readonly Mock<IAdminRepository> adminMockRepo;
		private readonly Mock<ISessionHelper> mockSessionHelper;
		private readonly AdminController adminController;

		public AdminControllerTests() 
		{
			adminMockRepo = new Mock<IAdminRepository>();
			mockSessionHelper = new Mock<ISessionHelper>();
			adminController = new AdminController(adminMockRepo.Object, mockSessionHelper.Object);
		}

		private LoginViewModel CreateLoginModel(string email, string password) => new LoginViewModel
		{
			Email = email,
			Password = password
		};

		private Admin CreateAdmin(string email, string password, Role role, int id) => new Admin
		{
			AdminId = id,
			Email = email,
			Password = password,
			UserRole = new UserRole { Role = role }
		};

		[Fact]
		public async Task Login_RedirectsToBookingIndex_WhenCredentialsAreValid()
		{
			//Arrange
			var mockRepo = new Mock<IAdminRepository>();

			var loginModel = new LoginViewModel
			{
				Email = "admin@example.com",
				Password = "password123"
			};

			var adminUser = new Admin
			{
				AdminId = 1,
				Email = "admin@example.com",
				Password = "password123",
				UserRole = new UserRole { Role = Role.Admin } // 0 = Admin
			};
			mockRepo.Setup(r => r.GetByEmailAsync(loginModel.Email)).ReturnsAsync(adminUser);

			var mockSessionHelper = new Mock<ISessionHelper>();

			var adminController = new AdminController(mockRepo.Object, mockSessionHelper.Object);

			//Act

			var result = await adminController.Login(loginModel);

			//Assert
			var redirectResult = Assert.IsType<RedirectToActionResult>(result);
			Assert.Equal("Index", redirectResult.ActionName);
			Assert.Equal("Booking", redirectResult.ControllerName);

			mockSessionHelper.Verify(s => s.SetUserSession(Role.Admin, 1), Times.Once());

		}

		[Fact]
		public async Task Login_RedirectsToBookingIndex_WhenCredentialsAreValidRefactored()
		{
			//Arrange

			var loginModel = CreateLoginModel("test@test.se", "Test123!");
			var adminUser = CreateAdmin("test@test.se", "Test123!", Role.Admin, 1);

			adminMockRepo.Setup(r => r.GetByEmailAsync(loginModel.Email)).ReturnsAsync(adminUser);

			//Act

			var result = await adminController.Login(loginModel);

			//Assert
			var redirectResult = Assert.IsType<RedirectToActionResult>(result);
			Assert.Equal("Index", redirectResult.ActionName);
			Assert.Equal("Booking", redirectResult.ControllerName);

			mockSessionHelper.Verify(s => s.SetUserSession(Role.Admin, 1), Times.Once());

		}

		[Theory]
		[InlineData("admin@car.se", "Admin123!", Role.Admin, 1)]
		[InlineData("customer@car.se", "User123!", Role.Customer, 2)]
		public async Task Login_SetsSessionAndRedirects_WhenCredentialsAreValid(string email, string password, Role expectedRole, int expectedId)
		{
			//Arrange
			var mockRepo = new Mock<IAdminRepository>();

			var loginVM = new LoginViewModel
			{
				Email = email,
				Password = password,
			};

			var admin = new Admin
			{
				AdminId = expectedId,
				Email = email,
				Password = password,
				UserRole = new UserRole { Role = expectedRole }
			};

			mockRepo.Setup(r => r.GetByEmailAsync(email)).ReturnsAsync(admin);

			var mockSessionHelper = new Mock<ISessionHelper>();

			var adminController = new AdminController(mockRepo.Object, mockSessionHelper.Object);

			//Act
			var result = await adminController.Login(loginVM);

			//Assert
			var redirectResult = Assert.IsType<RedirectToActionResult>(result);
			Assert.Equal("Index", redirectResult.ActionName);
			Assert.Equal("Booking", redirectResult.ControllerName);

			mockSessionHelper.Verify(s => s.SetUserSession(expectedRole, expectedId));
		}
	}
}