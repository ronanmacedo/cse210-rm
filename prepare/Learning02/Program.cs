using Learning02;

var job1 = new Job
{
    JobTitle = "Software Engineer",
    Company = "Microsoft",
    StartYear = 2015,
    EndYear = 2017
};

var job2 = new Job
{
    JobTitle = "Team Lead",
    Company = "Microsoft",
    StartYear = 2017,
    EndYear = 2019
};

var resume = new Resume
{
    Name = "John Doe",
    Jobs = [job1, job2]
};

resume.Display();