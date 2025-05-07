using System.Text;

Console.WriteLine("Please provide your grade score: ");

bool gradeInput = int.TryParse(Console.ReadLine(), out int score);

if (!gradeInput || score > 100 || score < 0)
{
    Console.WriteLine("Your grade score should be less than 100 and higher than 0, or an number.");
    return;
}

string grade = CalculateGrade(score);

Console.WriteLine($"Your grade is: {grade}");

if (score >= 70)
{
    Console.WriteLine("Congrats! You passed!");
}
else
{
    Console.WriteLine("Sorry, your grade was not enough to pass.");
}

static string CalculateGrade(int score)
{
    if (score < 60)
    {
        return "F";
    }

    var baseGrade = new StringBuilder();
    int scoreLastDigit = score % 10;

    if (score >= 90)
    {
        baseGrade = scoreLastDigit < 3 ? baseGrade.Append("A-") : baseGrade.Append('A');
    }
    else if (score >= 80)
    {
        baseGrade = baseGrade.Append('B');
    }
    else if (score >= 70)
    {
        baseGrade = baseGrade.Append('C');
    }
    else if (score >= 60)
    {
        baseGrade = baseGrade.Append('D');
    }

    if (score >= 90)
    {
        return baseGrade.ToString();
    }
    else if (scoreLastDigit >= 7)
    {
        return baseGrade.Append('+').ToString();
    }
    else
    {
        return scoreLastDigit < 3 ? baseGrade.Append('-').ToString() : baseGrade.ToString();
    }
}