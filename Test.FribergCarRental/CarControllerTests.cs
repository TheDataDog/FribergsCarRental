using FribergsCarRental.Controllers;
using FribergsCarRental.Data;
using FribergsCarRental.Helpers;
using FribergsCarRental.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.FribergCarRental
{
	public class CarControllerTests
	{
		[Fact]
		public async Task Index_ReturnsIndexAdminView_WhenUserIsAdmin()
		{
			//Arrange
			var mockRepo = new Mock<ICarRepository>();
			mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Car>());

			var mockSessionHelper = new Mock<ISessionHelper>();
			mockSessionHelper.Setup(s => s.GetUserSession()).Returns((0, 1));

			var carController = new CarController(mockRepo.Object, mockSessionHelper.Object);

			//Act
			var result = await carController.Index(null);

			//Assert
			var viewResult = Assert.IsType<ViewResult>(result);
			Assert.Equal("IndexAdmin", viewResult.ViewName);

		}
	}
}
