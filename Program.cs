var teamleads = CSVReader.Read<Teamlead>("./CSHARP_2024_NSU/Teamleads20.csv");
var juniors = CSVReader.Read<Junior>("./CSHARP_2024_NSU/Juniors20.csv");

Console.WriteLine(teamleads);
Console.WriteLine(juniors);
