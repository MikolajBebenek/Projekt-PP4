using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using FishingWorldEShop.Controllers;
using FishingWorldEShop.Models;
using FishingWorldEShop.Models.Auth;
using FishingWorldEShop.Repositories;
using FishingWorldEShop.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FishingWorldEShop.Tests
{
    public class AuthControllerTests
    {
        private AuthController GetController(string dbName)
        {
            var options = new DbContextOptionsBuilder<FishingWorldEShopContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            var context = new FishingWorldEShopContext(options);
            var hasher = new PasswordHasher<Customer>();
            var jwt = new JwtService(GetConfiguration());

            return new AuthController(context, hasher, jwt);
        }

        private IConfiguration GetConfiguration()
        {
            return new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "Jwt:Key", "testowykluczyk16digitminimum123456789" },
                    { "Jwt:Issuer", "TestIssuer" },
                    { "Jwt:Audience", "TestAudience" },
                    { "Jwt:ExpiresInMinutes", "60" }
                })
                .Build();
        }

        private Customer CreateFakeCustomer(string email, string password, IPasswordHasher<Customer> hasher)
        {
            var customer = new Customer
            {
                Email = email,
                Name = "Test",
                Surname = "Testowy",
                Phone = "123456789",
                Address = "Testowa 1"
            };

            customer.PasswordHash = hasher.HashPassword(customer, password);
            return customer;
        }

        [Fact]
        public async Task Register_NewUser_ReturnsOk()
        {
            var controller = GetController("RegisterDb");

            var request = new RegisterRequest
            {
                Name = "Misiu",
                Surname = "Testowy",
                Email = "nowy@eshop.pl",
                Phone = "123456789",
                Address = "Rybna 1",
                Password = "tajnehaslo"
            };

            var result = await controller.Register(request);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Register_ExistingEmail_ReturnsBadRequest()
        {
            var dbName = "RegisterExistsDb";

            var options = new DbContextOptionsBuilder<FishingWorldEShopContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            var context = new FishingWorldEShopContext(options);
            var hasher = new PasswordHasher<Customer>();
            var jwt = new JwtService(GetConfiguration());
            var existingUser = CreateFakeCustomer("duplikat@eshop.pl", "haslo", hasher);

            context.Customers.Add(existingUser);
            context.SaveChanges();

            var controller = new AuthController(context, hasher, jwt);

            var request = new RegisterRequest
            {
                Name = "Duplikat",
                Surname = "Test",
                Email = "duplikat@eshop.pl",
                Phone = "000000000",
                Address = "gdzieś",
                Password = "haslo"
            };

            var result = await controller.Register(request);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsOk()
        {
            var dbName = "LoginSuccessDb";

            var options = new DbContextOptionsBuilder<FishingWorldEShopContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            var context = new FishingWorldEShopContext(options);
            var hasher = new PasswordHasher<Customer>();
            var jwt = new JwtService(GetConfiguration());
            var user = CreateFakeCustomer("log@eshop.pl", "haslo123", hasher);

            context.Customers.Add(user);
            context.SaveChanges();

            var controller = new AuthController(context, hasher, jwt);

            var request = new LoginRequest
            {
                Email = "log@eshop.pl",
                Password = "haslo123"
            };

            var result = await controller.Login(request);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Login_InvalidPassword_ReturnsUnauthorized()
        {
            var dbName = "LoginFailDb";

            var options = new DbContextOptionsBuilder<FishingWorldEShopContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            var context = new FishingWorldEShopContext(options);
            var hasher = new PasswordHasher<Customer>();
            var jwt = new JwtService(GetConfiguration());
            var user = CreateFakeCustomer("zle@eshop.pl", "poprawnehaslo", hasher);

            context.Customers.Add(user);
            context.SaveChanges();

            var controller = new AuthController(context, hasher, jwt);

            var request = new LoginRequest
            {
                Email = "zle@eshop.pl",
                Password = "zlehaslo"
            };

            var result = await controller.Login(request);
            Assert.IsType<UnauthorizedObjectResult>(result);
        }
        [Fact]
        public async Task ResetPassword_ChangesPassword()
        {
            var dbName = "ResetPasswordDb";

            var options = new DbContextOptionsBuilder<FishingWorldEShopContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            var context = new FishingWorldEShopContext(options);
            var hasher = new PasswordHasher<Customer>();
            var jwt = new JwtService(GetConfiguration());

            var user = new Customer
            {
                Email = "reset@eshop.pl",
                Name = "Resetek",
                Surname = "Testowy",
                Phone = "000111222",
                Address = "Resetowa 5"
            };
            user.PasswordHash = hasher.HashPassword(user, "starehaslo");

            context.Customers.Add(user);
            context.SaveChanges();

            var controller = new AuthController(context, hasher, jwt);

            var resetRequest = new ResetPasswordRequest
            {
                Email = "reset@eshop.pl",
                NewPassword = "nowehaslo"
            };

            var result = await controller.ResetPassword(resetRequest);
            Assert.IsType<OkObjectResult>(result);

            var loginRequest = new LoginRequest
            {
                Email = "reset@eshop.pl",
                Password = "nowehaslo"
            };

            var loginResult = await controller.Login(loginRequest);
            Assert.IsType<OkObjectResult>(loginResult);
        }

    }
}