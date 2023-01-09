using UnitSelection.Entities.Terms;

namespace UnitSelection.Entities.Classes
{
    public class Class
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public int TermId { get; set; }
        public Term Term { get; set; }
    }
}
