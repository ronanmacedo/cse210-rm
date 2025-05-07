int magicNumber = Random.Shared.Next(1, 100);
bool playGuessGame = true;

while (playGuessGame)
{
    PlayRound(magicNumber);

    Console.WriteLine("Do you want to keep playing?");
    string answer = Console.ReadLine();

    if (string.Equals(answer, "yes", StringComparison.OrdinalIgnoreCase))
    {
        magicNumber = Random.Shared.Next(1, 100);
    }
    else
    {
        playGuessGame = false;
    }
}

static void PlayRound(int magicNumber)
{
    Console.WriteLine("What is your guess?");
    bool guessNumberInput = int.TryParse(Console.ReadLine(), out int guessNumber);

    if (!guessNumberInput)
    {
        Console.WriteLine("No guess number found.");
        return;
    }

    int numberOfAttempts = 1;

    while (magicNumber != guessNumber)
    {
        if (guessNumber < magicNumber)
        {
            Console.WriteLine("Higher");
        }
        else
        {
            Console.WriteLine("Lower");
        }

        numberOfAttempts++;

        Console.WriteLine("What is your guess?");
        guessNumberInput = int.TryParse(Console.ReadLine(), out guessNumber);
        if (!guessNumberInput)
        {
            Console.WriteLine("No guess number found.");
            return;
        }
    }

    Console.WriteLine("You guessed it!");
    Console.WriteLine($"Number of {(numberOfAttempts == 1 ? "attempt" : "attempts")}: {numberOfAttempts}");
}