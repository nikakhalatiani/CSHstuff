namespace ContactBookApp
{
    public class Contact
    {
        private string name = string.Empty;
        private string phoneNumber = string.Empty;
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
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Phone number cannot be empty.");
                }
                phoneNumber = value;
            }
        }
    }
}
