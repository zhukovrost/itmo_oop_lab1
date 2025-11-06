using System;
using System.Collections.Generic;
using System.Linq;

namespace Platform.Models
{
    public enum CourseDeliveryMode
    {
        Online = 1,
        Offline = 2
    }

    public abstract class Course
    {
        private readonly List<Student> _enrolledStudents = new();
        private readonly List<Teacher> _assignedTeachers = new();

        public Guid Id { get; }
        public string Title { get; }

        public abstract CourseDeliveryMode DeliveryMode { get; }

        public IReadOnlyCollection<Student> EnrolledStudents => _enrolledStudents.ToList().AsReadOnly();
        public IReadOnlyCollection<Teacher> AssignedTeachers => _assignedTeachers.ToList().AsReadOnly();

        protected Course(Guid id, string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Title must be provided", nameof(title));
            }

            Id = id == Guid.Empty ? Guid.NewGuid() : id;
            Title = title.Trim();
        }

        public void AssignTeacher(Teacher teacher)
        {
            if (teacher == null)
                throw new ArgumentNullException(nameof(teacher));

            if (_assignedTeachers.Any(t => t.Id == teacher.Id))
                throw new InvalidOperationException($"Teacher with ID {teacher.Id} is already assigned");

            _assignedTeachers.Add(teacher);
        }

        public bool UnassignTeacher(Guid teacherId)
        {
            if (teacherId == Guid.Empty)
                throw new ArgumentException("Teacher ID cannot be empty", nameof(teacherId));

            var teacher = _assignedTeachers.FirstOrDefault(t => t.Id == teacherId);
            return teacher != null && _assignedTeachers.Remove(teacher);
        }

        public bool EnrollStudent(Student student)
        {
            if (student == null)
            {
                throw new ArgumentNullException(nameof(student));
            }

            if (_enrolledStudents.Any(s => s.Id == student.Id))
            {
                return false;
            }

            _enrolledStudents.Add(student);
            return true;
        }

        public override string ToString()
        {
            var teacherLabel = !_assignedTeachers.Any()
                ? "[no teacher]"
                : string.Join(", ", _assignedTeachers.Select(t => t.FullName));
            return $"{Title} ({DeliveryMode}) - Teachers: {teacherLabel} - Students: {_enrolledStudents.Count}";
        }
    }
}