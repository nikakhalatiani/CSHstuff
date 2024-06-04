using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace ContactBookApp
{
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
                contacts[name.ToLower()] = new Contact { Name = name, PhoneNumber = phoneNumber };
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

        public bool IsEmpty()
        {
            return contacts.Count == 0;
        }

        public void FindContact(string name) // better patter matching can be used here
        {
            if (contacts.ContainsKey(name))
            {
                Contact contact = contacts[name];
                Console.WriteLine($"Name: {contact.Name}, Phone Number: {contact.PhoneNumber}");
            }
            Dictionary<string, Contact> matchedContacts = MatchName(name, contacts);
            if (matchedContacts.Count > 0)
            {
                Console.WriteLine("Did you mean:");
                foreach (var contact in matchedContacts)
                {
                    Console.WriteLine($"Name: {contact.Value.Name}, Phone Number: {contact.Value.PhoneNumber}");
                }
            }
            else
            {
                Console.WriteLine("Contact not found.");
            }
        }

        public void UpdateContact(string name, string newPhoneNumber)
        {
            if (contacts.ContainsKey(name))
            {
                contacts[name].PhoneNumber = newPhoneNumber;
                SaveContacts();
            }
            else
            {
                Console.WriteLine("Contact not found.");
            }
        }

        private bool IsPhoneNumberValid(string phoneNumber)
        {
            return (phoneNumber.Length == 9 && int.TryParse(phoneNumber, out _)) || (phoneNumber.Length == 13 && phoneNumber.StartsWith("+995") && int.TryParse(phoneNumber.Substring(4), out _));
        }

        private Dictionary<string, Contact> MatchName(string name, Dictionary<string, Contact> contacts)
        {
            Dictionary<string, Contact> matchedContacts = new Dictionary<string, Contact>();
            // write line
            foreach (var contact in contacts)
            {
                if (contact.Key.Contains(name.ToLower()))
                {
                    matchedContacts.Add(contact.Key, contact.Value);
                }
            }
            Console.WriteLine(matchedContacts.Count);
            return matchedContacts;
        }

}

}