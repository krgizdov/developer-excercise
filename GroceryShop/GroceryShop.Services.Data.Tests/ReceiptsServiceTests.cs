namespace GroceryShop.Services.Data.Tests
{
    using GroceryShop.Services.Mapping;
    using GroceryShop.Data.Common.Repositories;
    using GroceryShop.Data.Models;
    using GroceryShop.Web.ViewModels.Receipts;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;
    using System.Reflection;
    using GroceryShop.Common;
    using MockQueryable.Moq;
    using GroceryShop.Web.Infrastructure.Exceptions;

    public class ReceiptsServiceTests
    {
        private readonly Mock<IDeletableEntityRepository<Receipt>> repositoryMock;
        private readonly Mock<IProductsService> productsServiceMock;
        private readonly Mock<IDealsService> dealsServiceMock;

        public ReceiptsServiceTests()
        {
            repositoryMock = new Mock<IDeletableEntityRepository<Receipt>>();
            productsServiceMock = new Mock<IProductsService>();
            dealsServiceMock = new Mock<IDealsService>();
        }

        [Fact]
        public async Task GetAllReceiptsAsync_PassCorrectCount_ExpectedCorrectNumber()
        {
            //Arrange
            var receipts = new List<Receipt>
            {
                new Receipt
                {
                    Id = 1,
                    TotalPrice = 50,
                    Discount = 0,
                    TotalPriceWithDiscount = 50,
                    Products = new List<ProductReceipt>
                    {
                        new ProductReceipt
                        {
                            Product = new Product
                            {
                                Id = 1,
                                Name = "apple",
                                Price = 50,

                            }
                        }
                    }
                }, new Receipt(), new Receipt(), new Receipt(), new Receipt(), new Receipt()
            }.AsQueryable();

            var receiptMock = receipts.BuildMock();

            this.repositoryMock
                .Setup(s => s.AllAsNoTracking())
                .Returns(receiptMock.Object);

            AutoMapperConfig.RegisterMappings(Assembly.Load(GlobalConstants.AutoMapperAssemblyName));

            var service = new ReceiptsService(this.repositoryMock.Object, null, null);

            //Act
            var response = await service.GetAllAsync<ReceiptViewModel>(5);

            var model = response.FirstOrDefault();

            //Assert
            Assert.True(response.Count() == 5);
            Assert.IsType<ReceiptViewModel>(model);
            Assert.True(model.Products.Any());
            Assert.True(model.Id == 1);
            Assert.True(model.TotalPrice == "50 clouds");
            Assert.True(model.Discount == "0 clouds");
            Assert.True(model.TotalPriceWithDiscount == "50 clouds");
            Assert.True(model.Products.FirstOrDefault().Name == "apple");
            Assert.True(model.Products.FirstOrDefault().Price == "50 clouds");
        }

        [Fact]
        public async Task GetAllReceiptsAsync_PassWrongCount_ExpectedInvaldParameter()
        {
            //Arrange
            var service = new ReceiptsService(null, null, null);

            //Act
            Func<Task> task = async () => await service.GetAllAsync<ReceiptViewModel>(-5);

            //Assert
            await Assert.ThrowsAsync<InvalidParameterException>(task);
        }

        [Fact]
        public async Task GetReceiptByIdAsync_PassRightId_ExpectedValidResult()
        {
            //Arrange
            var receipts = new List<Receipt>
            {
                new Receipt
                {
                    Id = 1,
                    TotalPrice = 50,
                    Discount = 0,
                    TotalPriceWithDiscount = 50,
                    Products = new List<ProductReceipt>
                    {
                        new ProductReceipt
                        {
                            Product = new Product
                            {
                                Id = 1,
                                Name = "apple",
                                Price = 50,

                            }
                        }
                    }
                }
            }.AsQueryable();

            var receiptMock = receipts.BuildMock();

            this.repositoryMock
                .Setup(s => s.AllAsNoTracking())
                .Returns(receiptMock.Object);

            AutoMapperConfig.RegisterMappings(Assembly.Load(GlobalConstants.AutoMapperAssemblyName));

            var service = new ReceiptsService(this.repositoryMock.Object, null, null);
            //Act
            var response = await service.GetByIdAsync<ReceiptViewModel>(1);

            //Assert
            Assert.IsType<ReceiptViewModel>(response);
            Assert.True(response.Products.Any());
            Assert.True(response.Id == 1);
            Assert.True(response.TotalPrice == "50 clouds");
            Assert.True(response.Discount == "0 clouds");
            Assert.True(response.TotalPriceWithDiscount == "50 clouds");
            Assert.True(response.Products.FirstOrDefault().Name == "apple");
            Assert.True(response.Products.FirstOrDefault().Price == "50 clouds");
        }

        [Fact]
        public async Task GetReceiptByIdAsync_PassWrongId_ExpectedObjectNotFound()
        {
            //Arrange
            var receipts = new List<Receipt>().AsQueryable();

            var receiptMock = receipts.BuildMock();

            repositoryMock
                .Setup(s => s.AllAsNoTracking())
                .Returns(receiptMock.Object);

            AutoMapperConfig.RegisterMappings(Assembly.Load(GlobalConstants.AutoMapperAssemblyName));

            var service = new ReceiptsService(this.repositoryMock.Object, null, null);

            //Act
            Func<Task> task = async () => await service.GetByIdAsync<ReceiptViewModel>(1);

            //Assert
            await Assert.ThrowsAsync<ObjectNotFoundException>(task);
        }

        [Fact]
        public async Task CreateReceiptAsync_PassEmptyArray_ExpectedInvaldParameter()
        {
            //Arrange
            var service = new ReceiptsService(null, null, null);

            //Act
            Func<Task> task = async () => await service.CreateReceiptAsync(Array.Empty<string>());

            //Assert
            await Assert.ThrowsAsync<InvalidParameterException>(task);
        }

        [Fact]
        public async Task CreateReceiptAsync_PassEmptyProdcutName_ExpectedInvaldParameter()
        {
            //Arrange
            var service = new ReceiptsService(null, null, null);

            //Act
            Func<Task> task = async () => await service.CreateReceiptAsync(new string[] { "" });

            //Assert
            await Assert.ThrowsAsync<InvalidParameterException>(task);
        }

        [Fact]
        public async Task CreateReceiptAsync_PassWrongProdcutName_ExpectedInvaldParameter()
        {
            //Arrange
            var service = new ReceiptsService(null, null, null);

            //Act
            Func<Task> task = async () => await service.CreateReceiptAsync(new string[] { new string('*', 51) });

            //Assert
            await Assert.ThrowsAsync<InvalidParameterException>(task);
        }

        [Fact]
        public async Task CreateReceiptAsync_PassNonExistentProductName_ExpectedObjectNotFound()
        {
            //Arrange
            Product product = null;

            this.productsServiceMock
                .Setup(s => s.GetByNameAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(product));

            var service = new ReceiptsService(null, this.productsServiceMock.Object, null);

            //Act
            Func<Task> task = async () => await service.CreateReceiptAsync(new string[] { "apple" });

            //Assert
            await Assert.ThrowsAsync<ObjectNotFoundException>(task);
        }

        [Fact]
        public async Task CreateReceiptAsync_PassCorrectValues_ExpectedCorrectModel()
        {
            //Arrange
            Product product = new Product
            {
                Id = 1,
                Name = "apple",
                Price = 50,
            };

            IEnumerable<Deal> deals = new List<Deal>
                {
                    new Deal
                    {
                        Name = "2 for 3",
                    },
                    new Deal
                    {
                        Name = "buy 1 get 1 half price",
                    },
                };

            this.productsServiceMock
                .Setup(s => s.GetByNameAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(product));

            this.dealsServiceMock
                .Setup(s => s.GetAllAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(deals));

            AutoMapperConfig.RegisterMappings(Assembly.Load(GlobalConstants.AutoMapperAssemblyName));

            var service = new ReceiptsService(this.repositoryMock.Object, this.productsServiceMock.Object, this.dealsServiceMock.Object);

            //Act
            var response = await service.CreateReceiptAsync(new string[] { "apple" });

            //Assert
            Assert.IsType<ReceiptViewModel>(response);
            Assert.True(response.Products.Any());
            Assert.True(response.TotalPrice == "50 clouds");
            Assert.True(response.Discount == "0 clouds");
            Assert.True(response.TotalPriceWithDiscount == "50 clouds");
            Assert.True(response.Products.FirstOrDefault().Name == "apple");
            Assert.True(response.Products.FirstOrDefault().Price == "50 clouds");
        }
    }
}

