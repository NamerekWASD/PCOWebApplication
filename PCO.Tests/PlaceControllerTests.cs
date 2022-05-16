using Microsoft.AspNetCore.Mvc;
using Moq;
using PCO.Controllers;
using PCO.Models.Interfaces;
using PCO.Models.PlaceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PCO.Tests
{
    public class PlaceControllerTests
    {
        [Fact]
        public void GetListPlaces()
        {
            var mock = new Mock<IRepository<Place>>();
            mock.Setup(repo => repo.GetAll()).Returns(GetTestPlaces());
            var controller = new PlaceController(mock.Object);

            // Act
            var result = controller.List("", "");

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<PlaceCategoryViewModel>(viewResult.Model);
            Assert.Equal(GetTestPlaces().Count, model.Places.Count());
        }
        [Fact]
        public void GetPlaceReturnsBadRequestResultWhenIdIsNull()
        {
            // Arrange
            var mock = new Mock<IRepository<Place>>();
            var controller = new PlaceController(mock.Object);

            // Act
            var result = controller.GetPlace(null);

            // Arrange
            Assert.IsType<BadRequestResult>(result);
        }
        [Fact]
        public void AddPlaceReturnsViewResultWithUserModel()
        {
            // Arrange
            var mock = new Mock<IRepository<Place>>();
            var controller = new PlaceController(mock.Object);
            controller.ModelState.AddModelError("Name", "Required");
            Place newPlace = new ();

            // Act
            var result = controller.Create(newPlace);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(newPlace, viewResult?.Model);
        }
        [Fact]
        public void AddPlaceReturnsARedirectAndAddsUser()
        {
            // Arrange
            var mock = new Mock<IRepository<Place>>();
            var controller = new PlaceController(mock.Object);
            var newPlace = new Place()
            {
                Name = "Wall Street",
                Category = "Street",
                Country = "USA",
                City = "New-York"
            };

            // Act
            var result = controller.Create(newPlace);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            mock.Verify(r => r.Create(newPlace));
        }
        [Fact]
        public void GetPlaceReturnsNotFoundResultWhenUserNotFound()
        {
            // Arrange
            int testPlaceId = 1;
            var mock = new Mock<IRepository<Place>>();
            mock.Setup(repo => repo.Get(testPlaceId))
                .Returns(null as Place);
            var controller = new PlaceController(mock.Object);

            // Act
            var result = controller.GetPlace(testPlaceId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void GetPlaceReturnsViewResultWithUser()
        {
            // Arrange
            int TestPlaceId = 1;
            var mock = new Mock<IRepository<Place>>();
            mock.Setup(repo => repo.Get(TestPlaceId))
                .Returns(GetTestPlaces().FirstOrDefault(p => p.Id == TestPlaceId));
            var controller = new PlaceController(mock.Object);

            // Act
            var result = controller.GetPlace(TestPlaceId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Place>(viewResult.ViewData.Model);
            Assert.Equal("Maidan Nezalezhnosti", model.Name);
            Assert.Equal("Square", model.Category);
            Assert.Equal("Ukraine", model.Country);
            Assert.Equal("Kiev", model.City);
            Assert.Equal(TestPlaceId, model.Id);
        }
        public List<Place> GetTestPlaces()
        {
            string Square = "Square";
            string museum = "Museum";
            List<Place> places = new()
            {
                new Place
                {
                    Id = 1,
                    Name = "Maidan Nezalezhnosti",
                    Category = Square,
                    Country = "Ukraine",
                    City = "Kiev",
                },

                new Place
                {
                    Id = 2,
                    Name = "St. Sophia's Cathedral",
                    Category = museum,
                    Country = "Ukraine",
                    City = "Kiev",
                },

                new Place
                {
                    Id = 3,
                    Name = "Classic Remise Dusseldorf",
                    Category = museum,
                    Country = "Germany",
                    City = "Dusseldorf",
                },

                new Place
                {
                    Id = 4,
                    Name = "Tower of Pisa",
                    Category = Square,
                    Country = "Italy",
                    City = "Pisa",
                }
            };
            return places;
        }
    }
}
