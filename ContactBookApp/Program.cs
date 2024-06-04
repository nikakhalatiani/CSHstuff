namespace ContactBookApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            PhoneBook phoneBook = new PhoneBook();
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
                        Console.WriteLine("Enter phone number:");
                        string phoneNumber = Console.ReadLine();
                        phoneBook.AddContact(name, phoneNumber);
                        break;
                    case "remove":
                        if (phoneBook.IsEmpty())
                        {
                            Console.WriteLine("No contacts to remove.");
                            break;
                        }
                        Console.WriteLine("Enter name:");
                        string nameToRemove = Console.ReadLine();
                        phoneBook.RemoveContact(nameToRemove);
                        break;
                    case "list":
                        if (phoneBook.IsEmpty())
                        {
                            Console.WriteLine("No contacts to list.");
                            break;
                        }
                        phoneBook.ListContacts();
                        break;
                    case "exit":
                        Console.WriteLine("Exiting the application.");
                        break;
                    case "find":
                        if (phoneBook.IsEmpty())
                        {
                            Console.WriteLine("No contacts to find.");
                            break;
                        }
                        Console.WriteLine("Enter name:");
                        string nameToFind = Console.ReadLine();
                        phoneBook.FindContact(nameToFind);
                        break;
                    case "update":
                        if (phoneBook.IsEmpty())
                        {
                            Console.WriteLine("No contacts to update.");
                            break;
                        }
                        Console.WriteLine("Enter name:");
                        string nameToUpdate = Console.ReadLine();
                        Console.WriteLine("Enter new phone number:");
                        string newPhoneNumber = Console.ReadLine();
                        phoneBook.UpdateContact(nameToUpdate, newPhoneNumber);
                        break;
                    default:
                        Console.WriteLine("Invalid command.");
                        break;
                }
            }
        }
    }
}
