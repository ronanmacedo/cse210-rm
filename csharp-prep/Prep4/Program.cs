List<int> listOfNumbers = [];

Console.WriteLine("Enter a list of numbers, type 0 when finished.");

int number = -1;
while (number != 0)
{
    Console.WriteLine("Enter number: ");
    bool numberInput = int.TryParse(Console.ReadLine(), out number);

    if (!numberInput)
    {
        Console.WriteLine("No number found.");
        number = 0;
    }

    if (number == 0)
    {
        break;
    }

    listOfNumbers.Add(number);
}

Console.WriteLine($"The sum is: {listOfNumbers.Sum()}");
Console.WriteLine($"The average is: {listOfNumbers.Average()}");
Console.WriteLine($"The largest number is: {listOfNumbers.Max()}");
Console.WriteLine($"The smallest positive number is: {listOfNumbers.Where(n => n > 0).Min()}");
Console.WriteLine("The sorted list is:");
listOfNumbers.Reverse();
listOfNumbers.ForEach(n => Console.WriteLine(n));