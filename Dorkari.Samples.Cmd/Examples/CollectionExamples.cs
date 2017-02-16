using Dorkari.Helpers.Core.Linq;
using Dorkari.Samples.Cmd.Models;
using System.Collections.Generic;
using System.Linq;

namespace Dorkari.Samples.Cmd.Examples
{
    public class CollectionExamples
    {
        static IEnumerable<SoldierDTO> GetSoldiers()
        {
            var soldiers = new List<SoldierDTO>();
            soldiers.Add(new SoldierDTO { Id = 1, Category = "Army", Name = "Micky", YearsFought = new List<int> { 1955, 1988 } });
            soldiers.Add(new SoldierDTO { Id = 2, Category = "Army", Name = "Goofy", YearsFought = new List<int> { 1955, 1988 } });
            soldiers.Add(new SoldierDTO { Id = 3, Category = "Army", Name = "Donald", YearsFought = new List<int> { 1955, 1988 } });
            soldiers.Add(new SoldierDTO { Id = 4, Category = "Navy", Name = "Guddu", YearsFought = new List<int> { 2014, 2015 } });
            soldiers.Add(new SoldierDTO { Id = 5, Category = "Navy", Name = "Chachaji", YearsFought = new List<int> { 1982, 1997 } });
            soldiers.Add(new SoldierDTO { Id = 6, Category = "Navy", Name = "Batul", YearsFought = new List<int> { 1979, 2011 } });
            soldiers.Add(new SoldierDTO { Id = 7, Category = "AirForce", Name = "IronMan", YearsFought = new List<int> { 1998, 2006 } });
            soldiers.Add(new SoldierDTO { Id = 8, Category = "AirForce", Name = "SuperMan", YearsFought = new List<int> { 1931, 1965, 1978, 1999, 2012 } });
            soldiers.Add(new SoldierDTO { Id = 9, Category = "AirForce", Name = "SkatiMan", YearsFought = null });
            soldiers.Add(new SoldierDTO { Id = 10, Category = "AirForce", Name = "HanuMan", YearsFought = new List<int> { 1, 5 } });
            return soldiers;
        }

        public static void Show()
        {
            var soldiers = GetSoldiers();

            var minById = soldiers.MinBy(s => s.Id);
            var maxById = soldiers.MaxBy(s => s.Id);
            var airSoldierNames = soldiers.SelectWhere(s => s.Name, s => s.Category == "AirForce");
            var firstSoldierName = soldiers.SelectFirstOrDefault(s => s.Name);
            var firstSoldierNameOverId5 = soldiers.SelectFirstOrDefault(s => s.Name, s => s.Id > 5);
            var firstSoldierNameOverId10 = soldiers.SelectFirstOrDefault(s => s.Name, s => s.Id > 10);
            var soldiersFromEachCategory = soldiers.DistinctBy(s => s.Category);

            //randomize soldiers
            var soldiers2 = soldiers.ToList();
            soldiers2.Shuffle();
        }
    }
}
