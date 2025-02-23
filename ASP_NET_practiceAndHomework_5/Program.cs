using ASP_NET_practiceAndHomework_5;
using ASP_NET_practiceAndHomework_5.Middleware;

var builder = WebApplication.CreateBuilder(args);
var configurationManager = builder.Configuration;
builder.Configuration.AddJsonFile("Student.json");
var app = builder.Build();
var configuration = app.Configuration;
builder.Configuration.AddEnvironmentVariables();

//builder.Services.Configure<FontColor>(configuration.GetSection("FontColor"));

// Let's get all data from our `Student.json`
IConfigurationSection student = configuration.GetSection("Student");
IConfigurationSection sName = student.GetSection("Name");
IConfigurationSection sSurname = student.GetSection("Surname");
IConfigurationSection sAge = student.GetSection("Age");
IConfigurationSection sSubjects = student.GetSection("Subjects");
// Let's write received data to strings
string name = sName.Value ??
    throw new InvalidOperationException("Error during getting student's name");
string surname = sSurname.Value ??
    throw new InvalidOperationException("Error during getting student's surname");
string age = sAge.Value ??
    throw new InvalidOperationException("Error during getting student's age");
string subjects = sSubjects.Value ??
    throw new InvalidOperationException("Error during getting student's subjects list");

// Let's create list with data to pass to our middleware
List<string> studentsData = new List<string>
{
    name, surname, age, subjects
};

// Let's use our middleware with data about Student
app.UseMiddleware<RoutesMiddleware>(studentsData);

app.MapGet("/", () => $"Student info: \r\n" +
$"Name: {name} \r\n" +
$"Surname: {surname} \r\n" +
$"Age: {age} \r\n" +
$"Well-known subjects: {subjects}");


app.Run();
