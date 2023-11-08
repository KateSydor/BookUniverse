namespace BookUniverse.DAL.Entities
{
    public class GoogleDrive
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public long? Size { get; set; }

        public long? Version { get; set; }

        public DateTime? CreatedTime { get; set; }
    }
}
