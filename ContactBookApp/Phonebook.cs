// using System.Text.Json;
using Dapper;
using MySqlConnector;
using System.Threading.Tasks;


namespace ContactBookApp
{
    public class PhoneBook
    {
        private string _connectionString;

        public PhoneBook(string connectionString)
        {
            _connectionString = connectionString;
        }


        public async Task InitializeDatabase()
        {

            // var databaseConnectionString = _connectionString + "Database=;";
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = "CREATE DATABASE IF NOT EXISTS ContactBookDB";
                await connection.ExecuteAsync(sql);
                _connectionString = _connectionString + "Database=ContactBookDB;";
            }
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Create the Contacts table if it doesn't exist
                var createTableSql = @"CREATE TABLE IF NOT EXISTS Contacts (
                                        Name VARCHAR(255) PRIMARY KEY,
                                        PhoneNumber VARCHAR(13) NOT NULL
                                    )";
                await connection.ExecuteAsync(createTableSql);
            }
        }


        public async Task AddContact(string name, string phoneNumber)
        {
            if (!IsPhoneNumberValid(phoneNumber))
            {
                Console.WriteLine("Invalid phone number format.");
                return;
            }

            if (await CheckIfPresent(name))
            {
                Console.WriteLine("Contact already exists.");
                return;
            }

            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = "INSERT INTO Contacts (Name, PhoneNumber) VALUES (@Name, @PhoneNumber)";
                var result = await connection.ExecuteAsync(sql, new { Name = name, PhoneNumber = phoneNumber });

                if (result > 0)
                {
                    Console.WriteLine("Contact added successfully.");
                }
                else
                {
                    Console.WriteLine("Failed to add contact.");
                }
            }
        }

        public async Task RemoveContact(string name)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = "DELETE FROM Contacts WHERE Name = @Name";
                var result = await connection.ExecuteAsync(sql, new { Name = name });

                if (result > 0)
                {
                    Console.WriteLine("Contact removed successfully.");
                }
                else
                {
                    Console.WriteLine("Contact not found.");
                }
            }
        }

        public async Task ListContacts()
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = "SELECT Name, PhoneNumber FROM Contacts";
                var contacts = await connection.QueryAsync<Contact>(sql);

                foreach (var contact in contacts)
                {
                    Console.WriteLine($"Name: {contact.Name}, Phone Number: {contact.PhoneNumber}");
                }
            }
        }

        public async Task<Contact> FindContact(string name)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = "SELECT Name, PhoneNumber FROM Contacts WHERE Name = @Name";
                var contact = await connection.QuerySingleOrDefaultAsync<Contact>(sql, new { Name = name });

                if (contact != null)
                {
                    Console.WriteLine($"Name: {contact.Name}, Phone Number: {contact.PhoneNumber}");
                    return contact;
                }
                else
                {
                    var matchedContacts = MatchName(name);
                    if (matchedContacts.Count > 0)
                    {
                        Console.WriteLine("We couldn't find the exact contact. Maybe you searched for one of these:");
                        foreach (var matchedContact in matchedContacts)
                        {
                            Console.WriteLine($"Name: {matchedContact.Name}, Phone Number: {matchedContact.PhoneNumber}");
                        }
                        return null;
                    }
                    else
                    {
                        Console.WriteLine("Contact not found.");
                        return null;
                    }
                }
            }
        }

        public async Task UpdateContact(string name, string newPhoneNumber)
        {

            if (!IsPhoneNumberValid(newPhoneNumber))
            {
                Console.WriteLine("Invalid phone number format.");
                return;
            }

            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = "UPDATE Contacts SET PhoneNumber = @PhoneNumber WHERE Name = @Name";
                var result = await connection.ExecuteAsync(sql, new { Name = name, PhoneNumber = newPhoneNumber });

                if (result > 0)
                {
                    Console.WriteLine("Contact updated successfully.");
                }
                else
                {
                    Console.WriteLine("Contact not found.");
                }
            }
        }

        private bool IsPhoneNumberValid(string phoneNumber)
        {
            return (phoneNumber.Length == 9 && int.TryParse(phoneNumber, out _)) ||
                   (phoneNumber.Length == 13 && phoneNumber.StartsWith("+995") && int.TryParse(phoneNumber.Substring(4), out _));
        }

        private List<Contact> MatchName(string name)
        {
            var matchedContacts = new List<Contact>();

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var sql = "SELECT Name, PhoneNumber FROM Contacts WHERE Name LIKE @Name";
                var contacts = connection.Query<Contact>(sql, new { Name = "%" + name + "%" });

                matchedContacts.AddRange(contacts);
            }

            return matchedContacts;
        }

        public async Task<bool> IsEmpty()
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = "SELECT COUNT(*) FROM Contacts";
                var count = await connection.ExecuteScalarAsync<int>(sql);
                return count == 0;
            }
        }

        public async Task<bool> CheckIfPresent(string name)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = "SELECT Name FROM Contacts WHERE Name = @Name";
                var contact = await connection.QuerySingleOrDefaultAsync<Contact>(sql, new { Name = name });

                return contact != null;
            }
        }

        public async Task CleanDatabase()
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = "DELETE FROM Contacts";
                await connection.ExecuteAsync(sql);
            }
        }

        public async Task<int> GetContactCount()
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = "SELECT COUNT(*) FROM Contacts";
                return await connection.ExecuteScalarAsync<int>(sql);
            }
        }
    }
}