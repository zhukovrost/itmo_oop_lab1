using System;
using System.Linq;
using Platform.Models;
using Platform.Services;
using Xunit;

namespace itmo_oop_lab1.Tests;

public class CourseManagementSystemTests
{
    [Fact]
    public void AddCourse_ShouldAddCourse()
    {
        var system = new CourseManagementSystem();
        var course = new OnlineCourse(Guid.NewGuid(), "C# Advanced", "MS Teams", "https://example.com");

        system.AddCourse(course);

        Assert.Contains(system.Courses, c => c.Id == course.Id);
    }

    [Fact]
    public void RemoveCourse_ShouldReturnTrue_AndRemoveExistingCourse()
    {
        var system = new CourseManagementSystem();
        var course = new OfflineCourse(Guid.NewGuid(), "Math", "Main Campus", "101");
        system.AddCourse(course);

        var removed = system.RemoveCourse(course.Id);

        Assert.True(removed);
        Assert.DoesNotContain(system.Courses, c => c.Id == course.Id);
    }

    [Fact]
    public void RemoveCourse_ShouldReturnFalse_WhenCourseNotFound()
    {
        var system = new CourseManagementSystem();
        var removed = system.RemoveCourse(Guid.NewGuid());
        Assert.False(removed);
    }

    [Fact]
    public void AssignTeacherToCourse_ShouldReturnTrue_WhenBothExist()
    {
        var system = new CourseManagementSystem();
        var teacher = new Teacher(Guid.NewGuid(), "Alice", "Johnson");
        var course = new OnlineCourse(Guid.NewGuid(), "C# Advanced", "MS Teams", "https://example.com");
        system.AddTeacher(teacher);
        system.AddCourse(course);

        var ok = system.AssignTeacherToCourse(teacher.Id, course.Id);

        Assert.True(ok);
        Assert.Contains(system.GetCoursesByTeacher(teacher.Id), c => c.Id == course.Id);
    }

    [Fact]
    public void AssignTeacherToCourse_ShouldReturnFalse_WhenTeacherOrCourseMissing()
    {
        var system = new CourseManagementSystem();
        var teacher = new Teacher(Guid.NewGuid(), "Bob", "Smith");
        var course = new OfflineCourse(Guid.NewGuid(), "Discrete Math", "Campus", "204");
        system.AddTeacher(teacher);

        var ok1 = system.AssignTeacherToCourse(teacher.Id, course.Id);        // course не добавлен
        var ok2 = system.AssignTeacherToCourse(Guid.NewGuid(), course.Id);    // teacher отсутствует
        var ok3 = system.AssignTeacherToCourse(teacher.Id, Guid.NewGuid());   // course отсутствует

        Assert.False(ok1);
        Assert.False(ok2);
        Assert.False(ok3);
    }

    [Fact]
    public void UnassignTeacherFromCourse_ShouldReturnTrue_WhenPreviouslyAssigned()
    {
        var system = new CourseManagementSystem();
        var teacher = new Teacher(Guid.NewGuid(), "Alice", "Johnson");
        var course = new OnlineCourse(Guid.NewGuid(), "C# Advanced", "MS Teams", "https://example.com");
        system.AddTeacher(teacher);
        system.AddCourse(course);
        Assert.True(system.AssignTeacherToCourse(teacher.Id, course.Id));

        var ok = system.UnassignTeacherFromCourse(teacher.Id, course.Id);

        Assert.True(ok);
        Assert.DoesNotContain(system.GetCoursesByTeacher(teacher.Id), c => c.Id == course.Id);
    }

    [Fact]
    public void UnassignTeacherFromCourse_ShouldReturnFalse_WhenNotAssignedOrMissing()
    {
        var system = new CourseManagementSystem();
        var teacher = new Teacher(Guid.NewGuid(), "Bob", "Smith");
        var course = new OfflineCourse(Guid.NewGuid(), "Discrete Math", "Campus", "204");
        system.AddTeacher(teacher);
        system.AddCourse(course);

        var ok1 = system.UnassignTeacherFromCourse(teacher.Id, course.Id);
        var ok2 = system.UnassignTeacherFromCourse(Guid.NewGuid(), course.Id);
        var ok3 = system.UnassignTeacherFromCourse(teacher.Id, Guid.NewGuid());

        Assert.False(ok1);
        Assert.False(ok2);
        Assert.False(ok3);
    }

    [Fact]
    public void EnrollStudentToCourse_ShouldReturnTrue_AndEnrollStudent()
    {
        var system = new CourseManagementSystem();
        var student = new Student(Guid.NewGuid(), "Ilya", "Ivanov", "CS");
        var course = new OnlineCourse(Guid.NewGuid(), "C# Advanced", "MS Teams", "https://example.com");
        system.AddStudent(student);
        system.AddCourse(course);

        var ok = system.EnrollStudentToCourse(student.Id, course.Id);

        Assert.True(ok);
        Assert.Contains(system.GetStudentsByCourse(course.Id), s => s.Id == student.Id);
    }

    [Fact]
    public void EnrollStudentToCourse_ShouldReturnFalse_WhenDuplicateOrMissing()
    {
        var system = new CourseManagementSystem();
        var student = new Student(Guid.NewGuid(), "Anna", "Petrova", "Math");
        var course = new OfflineCourse(Guid.NewGuid(), "Discrete Math", "Campus", "204");
        system.AddStudent(student);
        system.AddCourse(course);

        Assert.True(system.EnrollStudentToCourse(student.Id, course.Id));
        var duplicate = system.EnrollStudentToCourse(student.Id, course.Id);
        var missingStudent = system.EnrollStudentToCourse(Guid.NewGuid(), course.Id);
        var missingCourse = system.EnrollStudentToCourse(student.Id, Guid.NewGuid());

        Assert.False(duplicate);
        Assert.False(missingStudent);
        Assert.False(missingCourse);
        Assert.Equal(1, system.GetStudentsByCourse(course.Id).Count(s => s.Id == student.Id));
    }

    [Fact]
    public void GetCoursesByTeacher_ShouldReturnOnlyAssignedCourses()
    {
        var system = new CourseManagementSystem();
        var teacher = new Teacher(Guid.NewGuid(), "Alice", "Johnson");
        var c1 = new OnlineCourse(Guid.NewGuid(), "C# Advanced", "MS Teams", "https://example.com/1");
        var c2 = new OfflineCourse(Guid.NewGuid(), "Discrete Math", "Campus", "204");
        var c3 = new OnlineCourse(Guid.NewGuid(), "Algorithms", "MS Teams", "https://example.com/2");
        system.AddTeacher(teacher);
        system.AddCourse(c1);
        system.AddCourse(c2);
        system.AddCourse(c3);

        Assert.True(system.AssignTeacherToCourse(teacher.Id, c1.Id));
        Assert.True(system.AssignTeacherToCourse(teacher.Id, c3.Id));

        var courses = system.GetCoursesByTeacher(teacher.Id).Select(c => c.Id).ToHashSet();

        Assert.Contains(c1.Id, courses);
        Assert.Contains(c3.Id, courses);
        Assert.DoesNotContain(c2.Id, courses);
    }

    [Fact]
    public void GetStudentsByCourse_ShouldReturnOnlyEnrolledStudents()
    {
        var system = new CourseManagementSystem();
        var s1 = new Student(Guid.NewGuid(), "Ilya", "Ivanov", "CS");
        var s2 = new Student(Guid.NewGuid(), "Anna", "Petrova", "Math");
        var s3 = new Student(Guid.NewGuid(), "Sergey", "Sidorov", "Physics");
        var course = new OnlineCourse(Guid.NewGuid(), "C# Advanced", "MS Teams", "https://example.com");
        system.AddStudent(s1);
        system.AddStudent(s2);
        system.AddStudent(s3);
        system.AddCourse(course);

        Assert.True(system.EnrollStudentToCourse(s1.Id, course.Id));
        Assert.True(system.EnrollStudentToCourse(s2.Id, course.Id));

        var students = system.GetStudentsByCourse(course.Id).Select(s => s.Id).ToHashSet();

        Assert.Contains(s1.Id, students);
        Assert.Contains(s2.Id, students);
        Assert.DoesNotContain(s3.Id, students);
    }
}