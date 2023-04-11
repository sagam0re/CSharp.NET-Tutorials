using System.Text.Json.Serialization;

namespace EFCoreRelationships
{
    public class Skill
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Damage { get; set; }
        [JsonIgnore]
        public List<Character> Characters { get; set; }
     

    }
}
