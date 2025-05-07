DisplayWelcome();
string userName = PromptUserName();
int userNumber = PromptUserNumber();
int squareNumber = SquareNumber(userNumber);
DisplayResult(userName, squareNumber);

static void DisplayWelcome()
{
    Console.WriteLine("Welcome to the program!");
}

static string PromptUserName()
{
    Console.WriteLine("Please enter your name: ");
    return Console.ReadLine();
}

static int PromptUserNumber()
{
    Console.WriteLine("Please enter your favorite number: ");
    bool userNumberInput = int.TryParse(Console.ReadLine(), out int number);

    if (!userNumberInput)
    {
        Console.WriteLine("No number found.");
        return 0;
    }

    return number;
}

static int SquareNumber(int userNumber)
{
    return userNumber * userNumber;
}

static void DisplayResult(string userName, int squareNumber)
{
    Console.WriteLine($"{userName}, the square of your number is {squareNumber}");
}