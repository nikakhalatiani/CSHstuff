# Contact Book Application

## Description

The Contact Book Application is a simple console-based phone book application that allows users to add, remove, list, find, and update contacts. The application stores contact information in a file to maintain persistence across sessions using JSON serialization.

## Features

- **Add Contact:** Add a new contact with a name and phone number.
- **Remove Contact:** Remove an existing contact by name.
- **List Contacts:** Display all contacts.
- **Find Contact:** Search for a contact by name.
- **Update Contact:** Update a contact's phone number.
- **Save Contacts:** Save contacts to a file in JSON format.
- **Load Contacts:** Load contacts from a file at startup.

## Installation

1. Clone the repository or download the source code.
2. Open a terminal and navigate to the project directory.

```sh
cd path/to/ContactBookApp
```

3. Run the project.

```sh
dotnet run
```

## Usage

When you run the application, you will be greeted with a welcome message and prompted to enter a command. The available commands are:

- `add`: Add a new contact.
- `remove`: Remove an existing contact.
- `list`: List all contacts.
- `find`: Find a contact by name.
- `update`: Update a contact's phone number.
- `exit`: Exit the application.

### Example Execution

1. **Add a Contact**

```sh
Enter a command (add, remove, list, find, update, exit):
add
Enter name:
John Doe
Enter phone number:
Format: 599123456 or +995599123456
592654321
```

2. **Remove a Contact**

```sh
Enter a command (add, remove, list, find, update, exit):
remove
Enter name:
John Doe
```

3. **List All Contacts**

```sh
Enter a command (add, remove, list, find, update, exit):
list
Name: John Doe, Phone Number: 599123456
```

4. **Find a Contact**

```sh
Enter a command (add, remove, list, find, update, exit):
find
Enter name:
John Doe
Name: John Doe, Phone Number: 599123456
```

5. **Update a Contact**

```sh
Enter a command (add, remove, list, find, update, exit):
update
Enter name:
John Doe
Enter new phone number:
Format: 599123456 or +995599123456
+995599123456
```

6. **Exit the Application**

```sh
Enter a command (add, remove, list, find, update, exit):
exit
Exiting the application.
```
