using System.Linq;
using Moq;
using SportsStore.Models;
using SportsStore.Pages;
using Xunit;

namespace SportsStore.Tests
{
    public class CartPageTests
    {
        [Fact]
        public void Can_Load_Cart()
        {
            //Arrange
            Product p1 = new Product { ProductID = 1, Name = "p1"};
            Product p2 = new Product { ProductID = 2, Name = "P2"};
            
            Mock<IStoreRepository> mockRepo = new Mock<IStoreRepository>();
            mockRepo.Setup(m => m.Products).Returns((new[]
            {
                p1, p1
            }).AsQueryable());
            
            Cart testCart = new Cart();
            testCart.AddItem(p1, 2);
            testCart.AddItem(p2, 1);

            //Action
            CartModel cartModel = new CartModel(mockRepo.Object, testCart);
            cartModel.OnGet("myUrl");
            
            //Assert
            Assert.Equal(2, cartModel.Cart.Lines.Count());
            Assert.Equal("myUrl", cartModel.ReturnUrl);
        }

        [Fact]
        public void Can_Update_Cart()
        {
            //Arrange
            Mock<IStoreRepository> mockRepo = new Mock<IStoreRepository>();
            mockRepo.Setup(m => m.Products).Returns((new[]
            {
                new Product { ProductID = 1, Name = "P1"}
            }).AsQueryable());
            
            Cart testCart = new Cart();

            //Action
            CartModel cartModel = new CartModel(mockRepo.Object, testCart);
            cartModel.OnPost(1, "myUrl");
            
            //Assert
            Assert.Single(testCart.Lines);
            Assert.Equal("P1", testCart.Lines.First().Product.Name);
            Assert.Equal(1, testCart.Lines.First().Quantity);
        }
    }
}