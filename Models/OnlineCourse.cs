namespace Platform.Models
{
    public sealed class OnlineCourse : Course
    {
        public string Platform { get; }
        public string CourseUrl { get; }

        public OnlineCourse(Guid id, string title, string platform, string courseUrl)
            : base(id, title)
        {
            if (string.IsNullOrWhiteSpace(platform))
            {
                throw new ArgumentException("Platform must be provided", nameof(platform));
            }

            if (string.IsNullOrWhiteSpace(courseUrl))
            {
                throw new ArgumentException("Course URL must be provided", nameof(courseUrl));
            }

            Platform = platform.Trim();
            CourseUrl = courseUrl.Trim();
        }

        public override CourseDeliveryMode DeliveryMode => CourseDeliveryMode.Online;

        public override string ToString()
        {
            return base.ToString() + $" | Platform: {Platform} | Url: {CourseUrl}";
        }
    }
}


