namespace NumberGuessGame
{
    public class Program
    {
        public static void Main(string[] _)
        {
            Console.WriteLine("Welcome to Number Guess Game!");
            Console.WriteLine("Please tell me the exclusive ranges where I should look for your number.");
            int min = AskNumber("Enter the minimum number: ");
            int max = int.MinValue;
            while (min >= max)
            {
                max = AskNumber("Enter the maximum number: ");
                if (min >= max)
                {
                    Console.WriteLine("Maximum number should be greater than the minimum number.");
                }
            }

            MakeGuess(min, max); // Using Binary Search here
            Console.WriteLine("Thank you for playing the game!");
        }

        private static void MakeGuess(int min, int max)
        {
            int guess;
            bool guessed = false;

            while (!guessed)
            {
                if (min > max)
                {
                    Console.WriteLine("You are cheating! You already told me that my guess is wrong.");
                    return;
                }

                guess = min + (max - min) / 2;

                Console.WriteLine($"Is your number {guess}? (yes/no)");
                string response = GetResponse("Enter your response: ");

                if (response == "yes")
                {
                    Console.WriteLine("I guessed your number!");
                    guessed = true;
                }
                else if (response == "no")
                {
                    if (min == max)
                    {
                        Console.WriteLine("You are cheating! You already told me that my guess is wrong.");
                        return;
                    }
                    int answer = AskEstimate("Is your number higher or lower than my guess? (1/2)\n1. Higher\n2. Lower\nEnter your choice: ");
                    if (answer == 1)
                    {
                        min = guess + 1;
                    }
                    else if (answer == 2)
                    {
                        max = guess - 1;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a valid choice.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter 'yes' or 'no'.");
                }
            }
        }

        private static int AskNumber(string message)
        {
            int number = 0;
            bool validInput = false;

            while (!validInput)
            {
                Console.Write(message);
                try
                {
                    number = int.Parse(Console.ReadLine());
                    validInput = true;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid input. Please enter a valid number.");
                }
            }

            return number;
        }

        private static string GetResponse(string message)
        {
            string response = "";
            bool validInput = false;

            while (!validInput)
            {
                Console.Write(message);
                response = Console.ReadLine().ToLower();
                if (response == "yes" || response == "no")
                {
                    validInput = true;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter 'yes' or 'no'.");
                }
            }

            return response;
        }

        private static int AskEstimate(string message)
        {
            int estimate = 0;
            bool validInput = false;

            while (!validInput)
            {
                Console.Write(message);
                try
                {
                    estimate = int.Parse(Console.ReadLine());
                    if (estimate == 1 || estimate == 2)
                    {
                        validInput = true;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a valid choice.");
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid input. Please enter a valid number.");
                }
            }

            return estimate;
        }
    }
}
