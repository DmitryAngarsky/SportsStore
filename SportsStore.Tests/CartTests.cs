using System.Linq;
using SportsStore.Models;
using Xunit;

namespace SportsStore.Tests
{
    public class CartTests
    {
        [Fact]
        public void Can_Add_New_Lines()
        {
            //Arrange
            Product p1 = new Product {ProductID = 1, Name = "P1"};
            Product p2 = new Product {ProductID = 3, Name = "P2"};
            
            Cart target = new Cart();
            
            //Act
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            CartLine[] result = target.Lines.ToArray();
            
            //Assert
            Assert.Equal(2, result.Length);
            Assert.Equal(p1, result[0].Product);
            Assert.Equal(p2, result[1].Product);
        }

        [Fact]
        public void Can_Add_Quantity_Lines()
        {
            //Arrange
            Product p1 = new Product {ProductID = 1, Name = "P1"};
            Product p2 = new Product {ProductID = 2, Name = "P2"};

            Cart target = new Cart();
            
            //Act
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p1, 10);
            CartLine[] result = target.Lines
                .OrderBy(x => x.Product.ProductID).ToArray();
            
            //Assert
            Assert.Equal(2, result.Length);
            Assert.Equal(11, result[0].Quantity);
            Assert.Equal(1, result[1].Quantity);
        }

        [Fact]
        public void Can_Remove_Line()
        {
            //Arrange
            Product p1 = new Product {ProductID = 1, Name = "P1"};
            Product p2 = new Product {ProductID = 2, Name = "P2"};
            Product p3 = new Product {ProductID = 3, Name = "P3"};
            
            Cart target = new Cart();
            target.AddItem(p1, 1);
            target.AddItem(p2, 2);
            target.AddItem(p3, 5);
            target.AddItem(p2, 1);
            
            //Act
            target.RemoveLine(p2);
            
            //Assert
            Assert.Empty(target.Lines.Where(p => p.Product == p2));
            Assert.Equal(2, target.Lines.Count);
        }

        [Fact]
        public void Calculate_Cart_Total()
        {
            //Arrange
            Product p1 = new Product {ProductID = 1, Name = "P1", Price = 100M};
            Product p2 = new Product {ProductID = 2, Name = "P2", Price = 50M};
            
            Cart target = new Cart();
            
            //Act
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p1, 3);
            
            //Assert
            Assert.Equal(450M, target.ComputeTotalValue());
        }

        [Fact]
        public void Can_Clear_Content()
        {
            //Arrange
            Product p1 = new Product {ProductID = 1, Name = "P1"};
            Product p2 = new Product {ProductID = 2, Name = "P2"};
            
            Cart target = new Cart();
            
            //Act
            target.AddItem(p1, 2);
            target.AddItem(p2, 3);
            target.Clear();
            
            //Assert
            Assert.Empty(target.Lines);
        }
    }
}