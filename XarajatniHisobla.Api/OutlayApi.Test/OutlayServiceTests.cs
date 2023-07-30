using Moq;
using OutlayApi.Models;
using OutlayApi.Repositories;
using OutlayApi.Services;
using System.Security.Claims;

namespace OutlayApi.Test;

public class OutlayServiceTests
{

    [Fact]
    public void ToEntityCreateOutlay_Returns_Correct_OutlayEntity()
    {
        // Arrange
        var createOutlay = new CreateOutlay
        {
            Name = "Outlay 1",
            Description = "Description 1",
            Cost = 10,
            CategoryId = 1
        };
        int userId = 1;

        // Act
        var result = OutlayService.ToEntityCreateOutlay(createOutlay, userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(createOutlay.Name, result.Name);
        Assert.Equal(createOutlay.Description, result.Description);
        Assert.Equal(createOutlay.Cost, result.Cost);
        Assert.Equal(userId, result.UserId);
        Assert.Equal(createOutlay.CategoryId, result.CategoryId);
        Assert.Equal(DateTime.Now.Date, result.CreateAt.Date);
    }


    [Fact]
    public void ToModelOutlay_Returns_Correct_Outlay()
    {
        // Arrange
        var outlayEntity = new Entities.Outlay
        {
            Id = 1,
            CategoryId = 2,
            Name = "Outlay 1",
            Description = "Description 1",
            CreateAt = DateTime.Now,
            LastUpdateAt = DateTime.Now,
            Cost = 10,
            User = new Entities.User { UserName = "Aziz" },
            Category = new Entities.Category
            {
                UserCategories = new List<Entities.UserCategory>
            {
                new Entities.UserCategory { UserId = 1, IsAdmin = true }
            }
            }
        };

        int userId = 1;
        string userName = "Aziz";

        // Act
        var result = OutlayService.ToModelOutlay(outlayEntity, userId, userName);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(outlayEntity.Id, result.Id);
        Assert.Equal(outlayEntity.CategoryId, result.CategoryId);
        Assert.Equal(outlayEntity.Name, result.Name);
        Assert.Equal(outlayEntity.Description, result.Description);
        Assert.Equal(outlayEntity.CreateAt, result.CreateAt);
        Assert.Equal(outlayEntity.LastUpdateAt, result.LastUpdateAt);
        Assert.Equal(outlayEntity.Cost ?? 0, result.Cost);
        Assert.Equal(outlayEntity.User.UserName, result.Username);
        Assert.True(result.IsAdmin);
        Assert.True(result.IsOwner);
    }


    [Fact]
    public void ToModelOutlays_Returns_Correct_OutlaysList()
    {
        // Arrange
        var outlayEntities = new List<Entities.Outlay>
    {
        new Entities.Outlay
        {
            Id = 1,
            Name = "Outlay 1",
            Description = "Description 1",
            Cost = 10,
            User = new Entities.User { UserName = "Aziz" }
        },
        new Entities.Outlay
        {
            Id = 2,
            Name = "Outlay 2",
            Description = "Description 2",
            Cost = 20,
            User = new Entities.User { UserName = "Niyatov" }
        }
    };

        // Act
        var result = OutlayService.ToModelOutlays(outlayEntities);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(outlayEntities.Count, result.Count);

        for (int i = 0; i < outlayEntities.Count; i++)
        {
            Assert.Equal(i + 1, result[i].Index);
            Assert.Equal(outlayEntities[i].Id, result[i].Id);
            Assert.Equal(outlayEntities[i].Name, result[i].Name);
            Assert.Equal(outlayEntities[i].Description, result[i].Description);
            Assert.Equal(outlayEntities[i].Cost ?? 0, result[i].Cost);
            Assert.Equal(outlayEntities[i].User.UserName, result[i].Username);
        }
    }


    [Fact]
    public async Task GetByIdAsync_Returns_Null_When_Entity_Not_Found()
    {
        // Arrange
        int id = 1;
        ClaimsPrincipal claims = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "1"),
            new Claim(ClaimTypes.Name, "Niyatov")
        }));
        var unitOfWorkMock = new Mock<IUnitOfWork>();

        unitOfWorkMock.Setup(u => u.Outlays.GetByIdAsync(id)).ReturnsAsync((Entities.Outlay)null);

        var service = new OutlayService(unitOfWorkMock.Object);

        // Act
        var result = await service.GetByIdAsync(id, claims);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByIdAsync_Returns_Correct_Outlay()
    {
        // Arrange
        int id = 1;
        ClaimsPrincipal claims = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "1"),
            new Claim(ClaimTypes.Name, "Niyatov")
        }));

        var outlayEntity = new Entities.Outlay
        {
            Id = id,
            CategoryId = 2,
            Name = "Outlay 1",
            Description = "Description 1",
            CreateAt = DateTime.Now,
            LastUpdateAt = DateTime.Now,
            Cost = 10,
            User = new Entities.User { UserName = "Niyatov" },
            Category = new Entities.Category
            {
                UserCategories = new List<Entities.UserCategory>
                {
                    new Entities.UserCategory { UserId = 1, IsAdmin = true }
                }
            }
        };

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(u => u.Outlays.GetByIdAsync(id)).ReturnsAsync(outlayEntity);

        var service = new OutlayService(unitOfWorkMock.Object);

        // Act
        var result = await service.GetByIdAsync(id, claims);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(outlayEntity.Id, result.Id);
        Assert.Equal(outlayEntity.CategoryId, result.CategoryId);
        Assert.Equal(outlayEntity.Name, result.Name);
        Assert.Equal(outlayEntity.Description, result.Description);
        Assert.Equal(outlayEntity.CreateAt, result.CreateAt);
        Assert.Equal(outlayEntity.LastUpdateAt, result.LastUpdateAt);
        Assert.Equal(outlayEntity.Cost ?? 0, result.Cost);
        Assert.Equal(outlayEntity.User.UserName, result.Username);
        Assert.True(result.IsAdmin);
        Assert.True(result.IsOwner);

    }


    [Fact]
    public async Task CreateAsync_Adds_Outlay_Entity()
    {
        // Arrange
        var createOutlay = new CreateOutlay
        {
            Name = "Outlay 1",
            Description = "Description 1",
            Cost = 10,
            CategoryId = 1
        };

        var userId = 1;
        var userName = "Aziz";

        var claims = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
        new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
        new Claim(ClaimTypes.Name, userName)
        }));

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var outlayRepositoryMock = new Mock<IOutlayRepository>();
        unitOfWorkMock.Setup(u => u.Outlays).Returns(outlayRepositoryMock.Object);

        var service = new OutlayService(unitOfWorkMock.Object);

        // Act
        await service.CreateAsync(createOutlay, claims);

        // Assert
        outlayRepositoryMock.Verify(r => r.AddAsync(It.Is<Entities.Outlay>(e =>
            e.Name == createOutlay.Name &&
            e.Description == createOutlay.Description &&
            e.Cost == createOutlay.Cost &&
            e.UserId == userId &&
            e.CategoryId == createOutlay.CategoryId &&
            e.CreateAt.Date == DateTime.Now.Date
        )), Times.Once);
    }


    [Fact]
    public async Task GetAllAsync_Returns_Correct_OutlaysList()
    {
        // Arrange
        int categoryId = 1;
        int userId = 1;

        var claims = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
        new Claim(ClaimTypes.NameIdentifier, userId.ToString())
    }));

        var outlayEntities = new List<Entities.Outlay>
    {
        new Entities.Outlay
        {
            Id = 1,
            Name = "Outlay 1",
            Description = "Description 1",
            Cost = 10,
            CategoryId = categoryId,
            User = new Entities.User { UserName = "Aziz" }
        },
        new Entities.Outlay
        {
            Id = 2,
            Name = "Outlay 2",
            Description = "Description 2",
            Cost = 20,
            CategoryId = categoryId,
            User = new Entities.User { UserName = "Niyatov" }
        }
    };

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var outlayRepositoryMock = new Mock<IOutlayRepository>();
        unitOfWorkMock.Setup(u => u.Outlays).Returns(outlayRepositoryMock.Object);
        outlayRepositoryMock.Setup(r => r.GetAll()).Returns(outlayEntities);

        var service = new OutlayService(unitOfWorkMock.Object);

        // Act
        var result = await service.GetAllAsync(categoryId, claims);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(outlayEntities.Count, result.Count);

        for (int i = 0; i < outlayEntities.Count; i++)
        {
            Assert.Equal(i + 1, result[i].Index);
            Assert.Equal(outlayEntities[i].Id, result[i].Id);
            Assert.Equal(outlayEntities[i].Name, result[i].Name);
            Assert.Equal(outlayEntities[i].Description, result[i].Description);
            Assert.Equal(outlayEntities[i].Cost ?? 0, result[i].Cost);
            Assert.Equal(outlayEntities[i].User.UserName, result[i].Username);
        }
    }


    [Fact]
    public async Task RemoveByIdAsync_Removes_Outlay_Entity()
    {
        // Arrange
        int id = 1;
        int userId = 1;
        string userName = "Aziz";

        var claims = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
        new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
        new Claim(ClaimTypes.Name, userName)
    }));

        var outlayEntity = new Entities.Outlay
        {
            Id = id,
            Name = "Outlay 1",
            Description = "Description 1",
            Cost = 10,
            UserId = userId,
            User = new Entities.User { UserName = userName }
        };

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var outlayRepositoryMock = new Mock<IOutlayRepository>();
        unitOfWorkMock.Setup(u => u.Outlays).Returns(outlayRepositoryMock.Object);
        outlayRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(outlayEntity);

        var service = new OutlayService(unitOfWorkMock.Object);

        // Act
        await service.RemoveByIdAsync(id, claims);

        // Assert
        outlayRepositoryMock.Verify(r => r.Remove(outlayEntity), Times.Once);
    }


    [Fact]
    public async Task UpdateAsync_Updates_Outlay_Entity()
    {
        // Arrange
        int id = 1;
        int userId = 1;
        string userName = "Aziz";

        var claims = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
        new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
        new Claim(ClaimTypes.Name, userName)
        }));

        var outlayEntity = new Entities.Outlay
        {
            Id = id,
            Name = "Outlay 1",
            Description = "Description 1",
            Cost = 10,
            UserId = userId,
            User = new Entities.User { UserName = userName }
        };

        var updateOutlay = new UpdateOutlay
        {
            Name = "Updated Outlay",
            Description = "Updated Description",
            Cost = 20
        };

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var outlayRepositoryMock = new Mock<IOutlayRepository>();
        unitOfWorkMock.Setup(u => u.Outlays).Returns(outlayRepositoryMock.Object);
        outlayRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(outlayEntity);

        var service = new OutlayService(unitOfWorkMock.Object);

        // Act
        await service.UpdateAsync(id, updateOutlay, claims);

        // Assert
        Assert.Equal(updateOutlay.Name, outlayEntity.Name);
        Assert.Equal(updateOutlay.Description, outlayEntity.Description);
        Assert.Equal(updateOutlay.Cost, outlayEntity.Cost);
        Assert.Equal(DateTime.Now.Date, outlayEntity.LastUpdateAt?.Date);

        outlayRepositoryMock.Verify(r => r.Update(outlayEntity), Times.Once);
    }
}