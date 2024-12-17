
using CsProj;
using Nsu.HackathonProblem.Contracts;

var strategy = new SuperStrategy();

List<Employee> juniors = [
    new Employee(1, "123"),
    new Employee(1, "Юдин Адам"),
    new Employee(2, "Яшина Яна"),
    new Employee(3, "Никитина Вероника"),
    new Employee(4, "Рябинин Александр"),
    new Employee(5, "Ильин Тимофей"),
    new Employee(6, "Кулагина Виктория"),
    new Employee(7, "Лапшина Варвара"),
    new Employee(8, "Лопатина Камилла"),
    new Employee(9, "Кузьмина Елизавета"),
    new Employee(10, "Галкина Есения"),
    new Employee(11, "Панина Таисия"),
    new Employee(12, "Семина Арина"),
    new Employee(13, "Бабушкин Иван"),
    new Employee(14, "Кузьмин Глеб"),
    new Employee(15, "Добрынин Степан"),
    new Employee(16, "Фомин Никита"),
    new Employee(17, "Маркина Кристина"),
    new Employee(18, "Зеленина Кира"),
    new Employee(19, "Капустина Елизавета"),
    new Employee(20, "Костин Александр"),
];

List<Employee> teamLeads = [
    new Employee(1, "Филиппова Ульяна"),
    new Employee(2, "Николаев Григорий"),
    new Employee(3, "Андреева Вероника"),
    new Employee(4, "Коротков Михаил"),
    new Employee(5, "Кузнецов Александр"),
    new Employee(6, "Максимов Иван"),
    new Employee(7, "Павлова Мария"),
    new Employee(8, "Артемов Матвей"),
    new Employee(9, "Денисов Дмитрий"),
    new Employee(10, "Астафьев Андрей"),
    new Employee(11, "Демидов Дмитрий"),
    new Employee(12, "Климов Михаил"),
    new Employee(13, "Терехов Демид"),
    new Employee(14, "Полякова Мария"),
    new Employee(15, "Волков Мирослав"),
    new Employee(16, "Волкова Ольга"),
    new Employee(17, "Киреева Виктория"),
    new Employee(18, "Волков Артём"),
    new Employee(19, "Панов Максим"),
    new Employee(20, "Комаров Макар"),
];

var teamLeadsWishlists = teamLeads.Select(x => new Wishlist(x.Id, juniors.GetShuffled().Select(y => y.Id).ToArray())).ToArray();
var teamLeadsWishlistsIdMap = teamLeadsWishlists.Select(x => x.EmployeeId).ToArray();
var juniorWishlists = juniors.Select(x => new Wishlist(x.Id, teamLeads.GetShuffled().Select(y => y.Id).ToArray())).ToArray();
var juniorWishlistsIdMap = juniorWishlists.Select(x => x.EmployeeId).ToArray();

var teams = strategy.BuildTeams(teamLeads, juniors, teamLeadsWishlists, juniorWishlists);

var resultDict = new List<int>();
foreach (var (teamlead, junior) in teams)
{
    var juniorIndexInJuniorWishLists = Array.IndexOf(juniorWishlistsIdMap, junior.Id);
    var juniorWishList = juniorWishlists[juniorIndexInJuniorWishLists].DesiredEmployees;
    var teamLeadIndex = Array.IndexOf(juniorWishList, teamlead.Id);
    var teamLeadScore = juniorWishList.Count() - teamLeadIndex;
    var teamLeadIndexInTeamLeadsWishlists = Array.IndexOf(teamLeadsWishlistsIdMap, teamlead.Id);
    var teamLeadWishList = teamLeadsWishlists[teamLeadIndexInTeamLeadsWishlists].DesiredEmployees;
    var juniorIndex = Array.IndexOf(teamLeadWishList, junior.Id);
    var juniorScore = teamLeadWishList.Count() - juniorIndex;

    resultDict.Add(teamLeadScore + juniorScore);
}
Console.WriteLine(resultDict.GetHarmonicMean());
