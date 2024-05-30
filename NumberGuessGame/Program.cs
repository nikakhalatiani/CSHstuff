namespace NumberGuessGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Number Guess Game!");
            Console.WriteLine("Please tell me ranges where I should look for your number.");

            int min = 0, max = 0;
            bool validInput = false;

            while (!validInput)
            {
                Console.Write("Enter the minimum number: ");
                try
                {
                    min = int.Parse(Console.ReadLine());
                    validInput = true;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid input. Please enter a valid number.");
                }
            }

            validInput = false; // Reset for the next input

            while (!validInput)
            {
                Console.Write("Enter the maximum number: ");
                try
                {
                    max = int.Parse(Console.ReadLine());
                    validInput = true;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid input. Please enter a valid number.");
                }
            }

            int guess = MakeGuess(min, max);
            Console.WriteLine($"Your number is {guess}.");
        }

        static int MakeGuess(int min, int max)
        {
            if (min > max)
            {
                Console.WriteLine("Range is Invalid. Please enter a valid range.");
                return -1;
            }
            else
            {
                return min;
            }
        }
    }
}
