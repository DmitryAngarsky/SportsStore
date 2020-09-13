using System.Linq;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using SportsStore.Models.ViewModels;
using Xunit;

namespace SportsStore.Tests
{
    public class ProductControllerTests
    {
        [Fact]
        public void Can_Use_Repository()
        {
            //Arrange
            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
            mock.Setup(m => m.Products).Returns((new[]
            {
                new Product {ProductID = 1, Name = "P1"},
                new Product {ProductID = 2, Name = "P2"}
            }).AsQueryable());

            //Act
            HomeController controller = new HomeController(mock.Object);

            //Assert
            if (controller.Index(null).ViewData.Model is ProductListViewModel result)
            {
                Product[] prodArray = result.Products.ToArray();
                Assert.True(prodArray.Length == 2);
                Assert.Equal("P1", prodArray[0].Name);
                Assert.Equal("P2", prodArray[1].Name);
            }
        }

        [Fact]
        public void Can_Paginate()
        {
            //Arrange
            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
            mock.Setup(m => m.Products).Returns((new[]
            {
                new Product {ProductID = 1, Name = "P1"},
                new Product {ProductID = 2, Name = "P2"},
                new Product {ProductID = 3, Name = "P3"},
                new Product {ProductID = 4, Name = "P4"},
                new Product {ProductID = 5, Name = "P5"}
            }).AsQueryable());

            HomeController controller = new HomeController(mock.Object) {PageSize = 3};

            //Act
            ProductListViewModel result = controller.Index(null,2).ViewData.Model as ProductListViewModel;

            //Assert
            Product[] prodArray = result?.Products.ToArray();
            Assert.True(prodArray != null && prodArray.Length == 2);
            Assert.Equal("P4", prodArray[0].Name);
            Assert.Equal("P5", prodArray[1].Name);
        }

        [Fact]
        public void Can_Send_Pagination_View_Model()
        {
            //Arrange
            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
            mock.Setup(m => m.Products).Returns((new[]
            {
                new Product {ProductID = 1, Name = "P1"},
                new Product {ProductID = 2, Name = "P2"},
                new Product {ProductID = 3, Name = "P3"},
                new Product {ProductID = 4, Name = "P4"},
                new Product {ProductID = 5, Name = "P5"}
            }).AsQueryable());

            //Arrange
            HomeController controller = new HomeController(mock.Object) { PageSize = 3 };

            //Act
            ProductListViewModel result = controller.Index(null,2).ViewData.Model as ProductListViewModel;

            //Assert
            if (result != null)
            {
                PagingInfo pageInfo = result.PagingInfo;
                Assert.Equal(2, pageInfo.CurrentPage);
                Assert.Equal(3, pageInfo.ItemsPerPage);
                Assert.Equal(5, pageInfo.TotalItems);
                Assert.Equal(2, pageInfo.TotalPages);
            }
        }

        [Fact]
        public void Can_Filtering_Products()
        {
            //Arrange
            // - create the mock repository
            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
            mock.Setup(m => m.Products).Returns((new[]
            {
                new Product {ProductID = 1, Name = "P1", Category = "Cat1"},
                new Product {ProductID = 2, Name = "P2", Category = "Cat2"}, 
                new Product {ProductID = 3, Name = "P3", Category = "Cat1"}, 
                new Product {ProductID = 4, Name = "P4", Category = "Cat2"}, 
                new Product {ProductID = 5, Name = "P5", Category = "Cat3"}
            }).AsQueryable());
            
            //Arrange - create a controller and make the page size 3 items
            HomeController controller = new HomeController(mock.Object) {PageSize = 3};
            
            //Action
            Product[] result =
                (controller.Index("Cat2").ViewData.Model as ProductListViewModel)?
                .Products.ToArray();
            
            //Assert
            if (result == null) return;
            Assert.Equal(2, result.Length);
            Assert.True(result[0].Name == "P2" && result[0].Category == "Cat2");
            Assert.True(result[1].Name == "P4" && result[1].Category == "Cat2");
        }
    }
}
