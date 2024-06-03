using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class PhoneBook
{
    private Dictionary<string, Contact> contacts = new Dictionary<string, Contact>();
    private const string FileName = "contacts.json";

    public PhoneBook()
    {
        LoadContacts();
    }

    public void AddContact(string name, string phoneNumber)
    {
        if (!contacts.ContainsKey(name))
        {
            contacts[name] = new Contact { Name = name, PhoneNumber = phoneNumber };
            SaveContacts();
        }
        else
        {
            Console.WriteLine("Contact already exists.");
        }
    }

    public void RemoveContact(string name)
    {
        if (contacts.ContainsKey(name))
        {
            contacts.Remove(name);
            SaveContacts();
        }
        else
        {
            Console.WriteLine("Contact not found.");
        }
    }

    public void ListContacts()
    {
        foreach (var contact in contacts.Values)
        {
            Console.WriteLine($"Name: {contact.Name}, Phone Number: {contact.PhoneNumber}");
        }
    }

    private void SaveContacts()
    {
        string json = JsonSerializer.Serialize(contacts);
        File.WriteAllText(FileName, json);
    }

    private void LoadContacts()
    {
        if (File.Exists(FileName))
        {
            string json = File.ReadAllText(FileName);
            contacts = JsonSerializer.Deserialize<Dictionary<string, Contact>>(json);
        }
    }
}
