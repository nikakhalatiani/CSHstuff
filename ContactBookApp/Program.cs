using System;

public class Program
{
    public static void Main(string[] args)
    {
        PhoneBook phoneBook = new PhoneBook();
        string command = string.Empty;

        Console.WriteLine("Welcome to the Phone Book Application!");

        while (command != "exit")
        {
            Console.WriteLine("\nEnter a command (add, remove, list, exit):");
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
                    Console.WriteLine("Enter name:");
                    string nameToRemove = Console.ReadLine();
                    phoneBook.RemoveContact(nameToRemove);
                    break;
                case "list":
                    phoneBook.ListContacts();
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
}
