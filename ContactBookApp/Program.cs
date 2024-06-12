using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;


namespace ContactBookApp
{
    public class Program
    {
        public static IConfiguration Configuration { get; set; }

        public static async Task Main(string[] args)
        {
            // var builder = new ConfigurationBuilder().AddEnvironmentVariables();

            var services = new ServiceCollection();
            ConfigureServices(services);

            var serviceProvider = services.BuildServiceProvider();

            var logger = serviceProvider.GetService<ILogger<Program>>();
            logger.LogInformation("Starting application");

            // Configuration = builder.Build();

            // var connectionString = Configuration["ConnectionStrings:DefaultConnection"];

            var phoneBook = serviceProvider.GetRequiredService<PhoneBook>();

            await phoneBook.InitializeDatabase();

            // PhoneBook phoneBook = new PhoneBook(connectionString);

            // await phoneBook.InitializeDatabase();

            string command = string.Empty;

            Console.WriteLine("Welcome to the Phone Book Application!");

            while (command != "exit")
            {
                Console.WriteLine("\nEnter a command (add, remove, list, find, update, exit):");
                command = Console.ReadLine().ToLower();

                switch (command)
                {
                    case "add":
                        Console.WriteLine("Enter name:");
                        string name = Console.ReadLine();
                        if (await phoneBook.CheckIfPresent(name))
                        {
                            Console.WriteLine("Contact already exists.");
                            break;
                        }
                        Console.WriteLine("Enter phone number:\nFormat: 599123456 or +995599123456");
                        string phoneNumber = Console.ReadLine();
                        await phoneBook.AddContact(name, phoneNumber);
                        break;
                    case "remove":
                        if (await phoneBook.IsEmpty())
                        {
                            Console.WriteLine("No contacts to remove.");
                            break;
                        }
                        Console.WriteLine("Enter name:");
                        string nameToRemove = Console.ReadLine();
                        await phoneBook.RemoveContact(nameToRemove);
                        break;
                    case "list":
                        if (await phoneBook.IsEmpty())
                        {
                            Console.WriteLine("No contacts to list.");
                            break;
                        }
                        await phoneBook.ListContacts();
                        break;
                    case "find":
                        if (await phoneBook.IsEmpty())
                        {
                            Console.WriteLine("No contacts to find.");
                            break;
                        }
                        Console.WriteLine("Enter name:");
                        string nameToFind = Console.ReadLine();
                        await phoneBook.FindContact(nameToFind);
                        break;
                    case "update":
                        if (await phoneBook.IsEmpty())
                        {
                            Console.WriteLine("No contacts to update.");
                            break;
                        }
                        Console.WriteLine("Enter name:");
                        string nameToUpdate = Console.ReadLine();
                        if (!await phoneBook.CheckIfPresent(nameToUpdate))
                        {
                            Console.WriteLine("Contact not found.");
                            break;
                        }
                        Console.WriteLine("Enter new phone number:\nFormat: 599123456 or +995599123456");
                        string newPhoneNumber = Console.ReadLine();
                        await phoneBook.UpdateContact(nameToUpdate, newPhoneNumber);
                        break;
                    case "exit":
                        Console.WriteLine("Exiting the application.");
                        break;
                    default:
                        Console.WriteLine("Invalid command.");
                        break;
                }
            }
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            var builder = new ConfigurationBuilder().AddEnvironmentVariables();
            Configuration = builder.Build();
            services.AddSingleton(Configuration);

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.SetMinimumLevel(LogLevel.Trace);
                loggingBuilder.AddNLog("nlog.config");
            });

            var connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception("Connection string is not set.");
            }
            services.AddSingleton(new PhoneBook(connectionString, services.BuildServiceProvider().GetRequiredService<ILogger<PhoneBook>>()));
        }
    }
}
