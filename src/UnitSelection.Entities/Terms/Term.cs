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

        public HashSet<Class>? Class { get; set; }
    }
}