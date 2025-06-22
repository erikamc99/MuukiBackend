public static class Constants
{
    public static readonly Dictionary<string, List<string>> AllowedBreedsBySpecies = new()
    {
        { "Gallo", new List<string> { "Común", "Pita Pinta Asturiana" } },
        { "Gallina", new List<string> { "Común", "Pita Pinta Asturiana" } },
        { "Pollito", new List<string> { "Común", "Pita Pinta Asturiana" } }
    };

    public static readonly List<string> AllowedSpecies = AllowedBreedsBySpecies.Keys.ToList();

    public static readonly List<string> AllowedSpaceTypes = new()
    {
        "Granja",
        "Corral",
        "Prao"
    };
}