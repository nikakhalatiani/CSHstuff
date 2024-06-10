// using System.Text.Json;
using Dapper;
using MySqlConnector;
using System.Threading.Tasks;


namespace ContactBookApp
{
    public class PhoneBook
    {
        private readonly string _connectionString;

        public PhoneBook(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task AddContact(string name, string phoneNumber)
        {
            if (!IsPhoneNumberValid(phoneNumber))
            {
                Console.WriteLine("Invalid phone number format.");
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

        public async Task FindContact(string name)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = "SELECT Name, PhoneNumber FROM Contacts WHERE Name = @Name";
                var contact = await connection.QuerySingleOrDefaultAsync<Contact>(sql, new { Name = name });

                if (contact != null)
                {
                    Console.WriteLine($"Name: {contact.Name}, Phone Number: {contact.PhoneNumber}");
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
                    }
                    else
                    {
                        Console.WriteLine("Contact not found.");
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

        // public async Task InitializeDatabase()
        // {
        //     using (var connection = new MySqlConnection(_connectionString))
        //     {
        //         await connection.OpenAsync();
        //         var sql = @"CREATE TABLE IF NOT EXISTS Contacts (
        //                         Id INT AUTO_INCREMENT PRIMARY KEY,
        //                         Name VARCHAR(255) NOT NULL,
        //                         PhoneNumber VARCHAR(13) NOT NULL
        //                     )";
        //         await connection.ExecuteAsync(sql);
        //     }
        // }

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
    }
}