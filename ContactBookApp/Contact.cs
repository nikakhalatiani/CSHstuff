namespace ContactBookApp
{
    public class Contact
    {
        private string name;
        private string phoneNumber;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Name cannot be empty.");
                }
                name = value;
            }
        }
        public string PhoneNumber
        {
            get
            {
                return phoneNumber;
            }
            set
            {
                while ((value.Length != 9 || !int.TryParse(value, out _)) && (value.Length != 13 || !value.StartsWith("+995") || !int.TryParse(value.Substring(4), out _)))
                {
                    Console.WriteLine("Phone number is not valid.");
                    value = Console.ReadLine();
                }
                phoneNumber = value;
            }
        }
    }
}
