var teamleads = CSVReader.Read<Teamlead>("./CSHARP_2024_NSU/Teamleads20.csv");
var juniors = CSVReader.Read<Junior>("./CSHARP_2024_NSU/Juniors20.csv");


var meanHarmonicsSum = 0.0;
for (int i = 0; i < 1000; i++) {
    var junLists = new Dictionary<Junior, List<Teamlead>>();
    foreach (var junior in juniors) {
        junLists.Add(junior, junior.createList(teamleads));
    }
    var teamleadLists = new Dictionary<Teamlead, List<Junior>>();
    foreach (var teamlead in teamleads) {
        teamleadLists.Add(teamlead, teamlead.createList(juniors));
    }
    var result_list = HR.assignEmployees(teamleads, juniors, teamleadLists, junLists);
    var result_dict = HR.calcSatisfactionIndex(teamleadLists, junLists, result_list);
    meanHarmonicsSum += HR.getHarmonicMean(result_dict);
}

Console.WriteLine(meanHarmonicsSum / 1000);
