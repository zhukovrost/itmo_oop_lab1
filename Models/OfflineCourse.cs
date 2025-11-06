namespace Platform.Models
{
    public sealed class OfflineCourse : Course
    {
        public string CampusAddress { get; }
        public string Room { get; }

        public OfflineCourse(Guid id, string title, string campus, string room)
            : base(id, title)
        {
            if (string.IsNullOrWhiteSpace(campus))
            {
                throw new ArgumentException("Campus address/name must be provided", nameof(campus));
            }

            if (string.IsNullOrWhiteSpace(room))
            {
                throw new ArgumentException("Room must be provided", nameof(room));
            }

            CampusAddress = campus.Trim();
            Room = room.Trim();
        }

        public override CourseDeliveryMode DeliveryMode => CourseDeliveryMode.Offline;

        public override string ToString()
        {
            return base.ToString() + $" | Address: {CampusAddress} | Room: {Room}";
        }
    }
}


