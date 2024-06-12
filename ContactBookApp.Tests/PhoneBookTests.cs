using System.Threading.Tasks;
using Xunit;
using ContactBookApp;
using Microsoft.Extensions.Configuration;
using Dapper;
using MySqlConnector;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace ContactBookApp.Tests
{
    public class PhoneBookTests : IDisposable
    {
        private readonly IConfiguration _configuration;
        private readonly PhoneBook _phoneBook;

        public PhoneBookTests()
        {
            var builder = new ConfigurationBuilder().AddEnvironmentVariables();
            _configuration = builder.Build();
            var connectionString = _configuration["ConnectionStrings:DefaultConnection"];
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception("Connection string is not set.");
            }
            var logger = NullLogger<PhoneBook>.Instance;

            _phoneBook = new PhoneBook(connectionString, logger);
            _phoneBook.InitializeDatabase().Wait();
            _phoneBook.CleanDatabase().Wait();
        }

        public async void Dispose()
        {
            await _phoneBook.CleanDatabase();
        }

        [Fact]
        public async Task AddContact_ShouldAddContact()
        {
            // Arrange
            await _phoneBook.CleanDatabase();
            var name = "John Doe";
            var phoneNumber = "599123456";

            // Act
            await _phoneBook.AddContact(name, phoneNumber);
            var isPresent = await _phoneBook.CheckIfPresent(name);

            // Assert
            Assert.True(isPresent);
        }

        [Fact]
        public async Task RemoveContact_ShouldRemoveContact()
        {
            // Arrange
            await _phoneBook.CleanDatabase();
            var name = "Jane Doe";
            var phoneNumber = "599123456";
            await _phoneBook.AddContact(name, phoneNumber);

            // Act
            await _phoneBook.RemoveContact(name);
            var isPresent = await _phoneBook.CheckIfPresent(name);

            // Assert
            Assert.False(isPresent);
        }

        [Fact]
        public async Task UpdateContact_ShouldUpdatePhoneNumber()
        {
            // Arrange
            await _phoneBook.CleanDatabase();
            var name = "Bob Smith";
            var phoneNumber = "599123456";
            var newPhoneNumber = "599654321";
            await _phoneBook.AddContact(name, phoneNumber);

            // Act
            await _phoneBook.UpdateContact(name, newPhoneNumber);
            var contact = await _phoneBook.FindContact(name);

            // Assert
            Assert.Equal(newPhoneNumber, contact.PhoneNumber);
        }

        [Fact]
        public async Task AddDuplicateContact_ShouldNotAddContact()
        {
            // Arrange
            await _phoneBook.CleanDatabase();
            var name = "John Doe";
            var phoneNumber = "599123456";
            await _phoneBook.AddContact(name, phoneNumber);

            // Act
            await _phoneBook.AddContact(name, phoneNumber);
            var count = await _phoneBook.GetContactCount();

            // Assert
            Assert.Equal(1, count);
        }
    }
}
