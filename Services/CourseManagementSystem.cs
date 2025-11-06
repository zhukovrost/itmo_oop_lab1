using Platform.Models;

namespace Platform.Services
{
    public sealed class CourseManagementSystem
    {
        private readonly Dictionary<Guid, Course> _coursesById = new();
        private readonly Dictionary<Guid, Teacher> _teachersById = new();
        private readonly Dictionary<Guid, Student> _studentsById = new();
        private readonly Dictionary<Guid, HashSet<Guid>> _coursesByTeacher = new();

        public IEnumerable<Course> Courses => _coursesById.Values;
        public IEnumerable<Teacher> Teachers => _teachersById.Values;
        public IEnumerable<Student> Students => _studentsById.Values;

        public void AddCourse(Course course)
        {
            if (_coursesById.ContainsKey(course.Id))
                throw new InvalidOperationException($"Course with Id {course.Id} already exists");
            _coursesById[course.Id] = course;
        }

        public bool RemoveCourse(Guid courseId)
        {
            if (!_coursesById.TryGetValue(courseId, out var course)) return false;

            foreach (var t in course.AssignedTeachers)
            {
                if (_coursesByTeacher.TryGetValue(t.Id, out var set))
                {
                    set.Remove(courseId);
                    if (set.Count == 0) _coursesByTeacher.Remove(t.Id);
                }
            }

            return _coursesById.Remove(courseId);
        }

        public void AddTeacher(Teacher teacher)
        {
            if (_teachersById.ContainsKey(teacher.Id))
                throw new InvalidOperationException($"Teacher with Id {teacher.Id} already exists");
            _teachersById[teacher.Id] = teacher;
        }

        public void AddStudent(Student student)
        {
            if (_studentsById.ContainsKey(student.Id))
                throw new InvalidOperationException($"Student with Id {student.Id} already exists");
            _studentsById[student.Id] = student;
        }

        public bool AssignTeacherToCourse(Guid teacherId, Guid courseId)
        {
            if (!_teachersById.TryGetValue(teacherId, out var teacher)) return false;
            if (!_coursesById.TryGetValue(courseId, out var course)) return false;
            course.AssignTeacher(teacher);
            if (!_coursesByTeacher.TryGetValue(teacherId, out var set))
            {
                set = new HashSet<Guid>();
                _coursesByTeacher[teacherId] = set;
            }
            set.Add(courseId);
            return true;
        }

        public bool UnassignTeacherFromCourse(Guid teacherId, Guid courseId)
        {
            if (!_coursesById.TryGetValue(courseId, out var course)) return false;
            var removed = course.UnassignTeacher(teacherId);
            if (removed && _coursesByTeacher.TryGetValue(teacherId, out var set))
            {
                set.Remove(courseId);
                if (set.Count == 0) _coursesByTeacher.Remove(teacherId);
            }
            return removed;
        }

        public bool EnrollStudentToCourse(Guid studentId, Guid courseId)
        {
            if (!_studentsById.TryGetValue(studentId, out var student)) return false;
            if (!_coursesById.TryGetValue(courseId, out var course)) return false;
            return course.EnrollStudent(student);
        }

        public IReadOnlyCollection<Course> GetCoursesByTeacher(Guid teacherId)
        {
            if (!_coursesByTeacher.TryGetValue(teacherId, out var courseSet) || courseSet.Count == 0)
                return Array.Empty<Course>();
            var list = new List<Course>(courseSet.Count);
            foreach (var id in courseSet)
                if (_coursesById.TryGetValue(id, out var c)) list.Add(c);
            return list;
        }

        public IReadOnlyCollection<Student> GetStudentsByCourse(Guid courseId)
        {
            return _coursesById.TryGetValue(courseId, out var course)
                ? course.EnrolledStudents.ToList()
                : Array.Empty<Student>();
        }
    }
}