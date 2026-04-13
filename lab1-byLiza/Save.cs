using System;

namespace GameSimulation
{
    public class Save
    {
        public DateTime SavedAt { get; }

        public string Description { get; }

        public Save(string description = "Auto save")
        {
            SavedAt = DateTime.Now;
            Description = description;
        }
    }
}