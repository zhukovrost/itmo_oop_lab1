namespace Platform.Models
{
    public abstract class Person
    {
        public Guid Id { get; }
        public string Name { get; }
        public string Surname { get; }
        public string FullName => $"{Name} {Surname}";

        protected Person(Guid id, string name, string surname)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name must be provided", nameof(name));
            
            if (string.IsNullOrWhiteSpace(surname))
                throw new ArgumentException("Surname must be provided", nameof(surname));

            Id = id == Guid.Empty ? Guid.NewGuid() : id;
            Name = name.Trim();
            Surname = surname.Trim();
        }
    }
}