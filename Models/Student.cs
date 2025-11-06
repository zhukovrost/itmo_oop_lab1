namespace Platform.Models
{
    public sealed class Student : Person
    {
        public string Major { get; }

        public Student(Guid id, string name, string surname, string major): base(id, name, surname)
        {
            Major = major ?? string.Empty;
        }
    }
}
