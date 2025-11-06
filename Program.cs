using System;
using Platform.Models;
using Platform.Services;

var system = new CourseManagementSystem();
SeedDemo(system);

while (true)
{
    PrintMenu();
    Console.Write("Выберите пункт меню: ");
    var input = Console.ReadLine();
    Console.WriteLine();

    switch (input)
    {
        case "1":
            ListCourses(system);
            break;
        case "2":
            AddCourse(system);
            break;
        case "3":
            RemoveCourse(system);
            break;
        case "4":
            ListTeachers(system);
            break;
        case "5":
            AddTeacher(system);
            break;
        case "6":
            AssignTeacher(system);
            break;
        case "7":
            UnassignTeacher(system);
            break;
        case "8":
            ListStudents(system);
            break;
        case "9":
            AddStudent(system);
            break;
        case "10":
            EnrollStudent(system);
            break;
        case "11":
            ListCoursesByTeacher(system);
            break;
        case "12":
            ListStudentsByCourse(system);
            break;
        case "0":
            Console.WriteLine("Выход...");
            return;
        default:
            Console.WriteLine("Неизвестная команда. Введите номер пункта меню.");
            break;
    }

    Console.WriteLine();
}

static void PrintMenu()
{
    Console.WriteLine("==== Система управления курсами ====");
    Console.WriteLine("1. Показать все курсы");
    Console.WriteLine("2. Добавить курс");
    Console.WriteLine("3. Удалить курс");
    Console.WriteLine("4. Показать всех преподавателей");
    Console.WriteLine("5. Добавить преподавателя");
    Console.WriteLine("6. Назначить преподавателя на курс");
    Console.WriteLine("7. Снять преподавателя с курса");
    Console.WriteLine("8. Показать всех студентов");
    Console.WriteLine("9. Добавить студента");
    Console.WriteLine("10. Зачислить студента на курс");
    Console.WriteLine("11. Показать курсы преподавателя");
    Console.WriteLine("12. Показать студентов курса");
    Console.WriteLine("0. Выход");
}

static void ListCourses(CourseManagementSystem system)
{
    if (!system.Courses.Any())
    {
        Console.WriteLine("Курсов нет.");
        return;
    }

    Console.WriteLine("Курсы:");
    foreach (var c in system.Courses)
        Console.WriteLine($" - {c.Id} | {c}");
}

static void AddCourse(CourseManagementSystem system)
{
    Console.WriteLine("Тип курса: 1 - Online, 2 - Offline");
    Console.Write("Введите тип: ");
    var type = Console.ReadLine();

    Console.Write("Название: ");
    var title = ReadNonEmpty();

    if (type == "1")
    {
        Console.Write("Платформа: ");
        var platform = ReadNonEmpty();
        Console.Write("URL: ");
        var url = ReadNonEmpty();
        var course = new OnlineCourse(Guid.NewGuid(), title, platform, url);
        system.AddCourse(course);
        Console.WriteLine("Онлайн-курс добавлен: " + course.Id);
    }
    else if (type == "2")
    {
        Console.Write("Адрес кампуса: ");
        var address = ReadNonEmpty();
        Console.Write("Аудитория: ");
        var room = ReadNonEmpty();
        var course = new OfflineCourse(Guid.NewGuid(), title, address, room);
        system.AddCourse(course);
        Console.WriteLine("Оффлайн-курс добавлен: " + course.Id);
    }
    else
    {
        Console.WriteLine("Неверный тип курса.");
    }
}

static void RemoveCourse(CourseManagementSystem system)
{
    Console.Write("Введите ID курса: ");
    if (TryReadGuid(out var id))
        Console.WriteLine(system.RemoveCourse(id) ? "Курс удалён." : "Курс не найден.");
}

static void ListTeachers(CourseManagementSystem system)
{
    if (!system.Teachers.Any())
    {
        Console.WriteLine("Преподавателей нет.");
        return;
    }

    Console.WriteLine("Преподаватели:");
    foreach (var t in system.Teachers)
        Console.WriteLine($" - {t.Id} | {t.FullName}");
}

static void AddTeacher(CourseManagementSystem system)
{
    Console.Write("Имя преподавателя: ");
    var name = ReadNonEmpty();
    Console.Write("Фамилия преподавателя: ");
    var surname = ReadNonEmpty();
    var teacher = new Teacher(Guid.NewGuid(), name, surname);
    system.AddTeacher(teacher);
    Console.WriteLine("Добавлен преподаватель: " + teacher.Id);
}

static void AssignTeacher(CourseManagementSystem system)
{
    Console.Write("ID преподавателя: ");
    if (!TryReadGuid(out var teacherId)) return;
    Console.Write("ID курса: ");
    if (!TryReadGuid(out var courseId)) return;
    Console.WriteLine(system.AssignTeacherToCourse(teacherId, courseId) ? "Назначено." : "Преподаватель или курс не найдены.");
}

static void UnassignTeacher(CourseManagementSystem system)
{
    Console.Write("ID преподавателя: ");
    if (!TryReadGuid(out var teacherId)) return;
    Console.Write("ID курса: ");
    if (!TryReadGuid(out var courseId)) return;
    Console.WriteLine(system.UnassignTeacherFromCourse(teacherId, courseId) ? "Снят." : "Преподаватель или курс не найдены, либо не был назначен.");
}

static void ListStudents(CourseManagementSystem system)
{
    if (!system.Students.Any())
    {
        Console.WriteLine("Студентов нет.");
        return;
    }

    Console.WriteLine("Студенты:");
    foreach (var s in system.Students)
        Console.WriteLine($" - {s.Id} | {s.FullName}");
}

static void AddStudent(CourseManagementSystem system)
{
    Console.Write("Имя студента: ");
    var name = ReadNonEmpty();
    Console.Write("Фамилия студента: ");
    var surname = ReadNonEmpty();
    Console.Write("Специальность студента: ");
    var major = ReadNonEmpty();
    var student = new Student(Guid.NewGuid(), name, surname, major);
    system.AddStudent(student);
    Console.WriteLine("Добавлен студент: " + student.Id);
}

static void EnrollStudent(CourseManagementSystem system)
{
    Console.Write("ID студента: ");
    if (!TryReadGuid(out var studentId)) return;
    Console.Write("ID курса: ");
    if (!TryReadGuid(out var courseId)) return;
    Console.WriteLine(system.EnrollStudentToCourse(studentId, courseId) ? "Зачислен." : "Студент/курс не найдены или уже зачислен.");
}

static void ListCoursesByTeacher(CourseManagementSystem system)
{
    Console.Write("ID преподавателя: ");
    if (!TryReadGuid(out var teacherId)) return;
    var courses = system.GetCoursesByTeacher(teacherId);
    if (courses.Count == 0)
    {
        Console.WriteLine("Курсов не найдено.");
        return;
    }
    Console.WriteLine("Курсы преподавателя:");
    foreach (var c in courses)
        Console.WriteLine($" - {c.Id} | {c.Title} ({c.DeliveryMode})");
}

static void ListStudentsByCourse(CourseManagementSystem system)
{
    Console.Write("ID курса: ");
    if (!TryReadGuid(out var courseId)) return;
    var students = system.GetStudentsByCourse(courseId);
    if (students.Count == 0)
    {
        Console.WriteLine("Студентов нет.");
        return;
    }
    Console.WriteLine("Студенты курса:");
    foreach (var s in students)
        Console.WriteLine($" - {s.Id} | {s.FullName}");
}

static bool TryReadGuid(out Guid id)
{
    var raw = Console.ReadLine();
    if (Guid.TryParse(raw, out id)) return true;
    Console.WriteLine("Неверный формат GUID.");
    return false;
}

static string ReadNonEmpty()
{
    while (true)
    {
        var s = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(s)) return s.Trim();
        Console.Write("Поле не может быть пустым. Повторите ввод: ");
    }
}

static void SeedDemo(CourseManagementSystem system)
{
    var teacherAlice = new Teacher(Guid.NewGuid(), "Alice", "Johnson");
    var teacherBob = new Teacher(Guid.NewGuid(), "Bob", "Smith");
    system.AddTeacher(teacherAlice);
    system.AddTeacher(teacherBob);

    var studentIlya = new Student(Guid.NewGuid(), "Ilya", "Ivanov", "Computer Science");
    var studentAnna = new Student(Guid.NewGuid(), "Anna", "Petrova", "Mathematics");
    var studentSergey = new Student(Guid.NewGuid(), "Sergey", "Sidorov", "Physics");
    system.AddStudent(studentIlya);
    system.AddStudent(studentAnna);
    system.AddStudent(studentSergey);

    var onlineCsharp = new OnlineCourse(Guid.NewGuid(), "C# Advanced", "MS Teams", "https://teams.microsoft.com/l/course/csharp-advanced");
    var offlineMath = new OfflineCourse(Guid.NewGuid(), "Discrete Math", "Main Campus, Nevsky 1", "Auditorium 204");
    system.AddCourse(onlineCsharp);
    system.AddCourse(offlineMath);

    system.AssignTeacherToCourse(teacherAlice.Id, onlineCsharp.Id);
    system.AssignTeacherToCourse(teacherBob.Id, offlineMath.Id);

    system.EnrollStudentToCourse(studentIlya.Id, onlineCsharp.Id);
    system.EnrollStudentToCourse(studentAnna.Id, onlineCsharp.Id);
    system.EnrollStudentToCourse(studentSergey.Id, offlineMath.Id);
}