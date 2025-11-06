namespace Platform.Models
{
    public sealed class Teacher : Person
    {
        public string Bio { get; }

        public Teacher(Guid id, string name, string surname, string bio = ""): base(id, name, surname)
        {
            Bio = bio ?? string.Empty;
        }
    }
}


