using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.DynamicProxy.Contributors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Localization.Internal;
using Moq;
using SportsStore.Components;
using SportsStore.Infrastructure;
using SportsStore.Models;
using SportsStore.Models.ViewModels;
using Xunit;

namespace SportsStore.Tests
{
    public class NavigationMenuViewComponentTests
    {
        [Fact]
        public void Can_Select_Categories()
        {
            //Arrange
            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product {ProductID = 1, Name = "P1", Category="Apples" },
                new Product {ProductID = 2, Name = "P2", Category="Apples" },
                new Product {ProductID = 3, Name = "P3", Category="Tangerines" },
                new Product {ProductID = 4, Name = "P4", Category="Plums" },
                new Product {ProductID = 5, Name = "P5", Category="Tangerines" }
            }).AsQueryable<Product>());

            NavigationMenuViewComponent target =
                new NavigationMenuViewComponent(mock.Object);

            //Act
            string[] results = ((IEnumerable<string>)(target.Invoke()
                as ViewViewComponentResult).ViewData.Model).ToArray();

            //Assert
            Assert.True(Enumerable.SequenceEqual(new string[] { "Apples", "Plums", "Tangerines" }, results));
        }


        [Fact]
        public void Indicates_Selected_Category()
        {
            //Arrange
            string categoryToSelect = "Apples";
            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product { ProductID = 1, Name = "P1", Category = "Apples" },
                new Product { ProductID = 4, Name = "P2", Category = "Tangerines" },
            }).AsQueryable<Product>());

            NavigationMenuViewComponent target = 
                new NavigationMenuViewComponent(mock.Object);
            target.ViewComponentContext = new ViewComponentContext
            {
                ViewContext = new ViewContext
                {
                    RouteData = new Microsoft.AspNetCore.Routing.RouteData()
                }
            };
            target.RouteData.Values["category"] = categoryToSelect;
            //Act

            string result = (string)(target.Invoke() as
                ViewViewComponentResult).ViewData["SelectedCategory"];

            //Assert
            Assert.Equal(categoryToSelect, result);
        }
    }
}
