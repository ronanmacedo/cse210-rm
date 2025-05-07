namespace Learning02;

public class Job
{
    public string Company {  get; set; }
    public string JobTitle { get; set; }
    public int StartYear { get; set; }
    public int EndYear { get; set; }

    public void Display()
    {
        Console.WriteLine($"{JobTitle} ({Company}) {StartYear}-{EndYear}");
    }
}
