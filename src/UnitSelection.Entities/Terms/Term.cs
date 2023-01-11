using UnitSelection.Entities.Classes;

namespace UnitSelection.Entities.Terms
{
    public class Term
    {
        public Term()
        {
            Class = new HashSet<Class>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public HashSet<Class>? Class { get; set; }
    }
}