using System;
using System.Collections.Generic;
using System.Linq;

namespace AseanPhonebook
{
    public class Phonebook
    {
        private readonly List<Student> _students = new List<Student>();

        public void AddStudent(Student student)
        {
            _students.Add(student);
        }

        public Student? FindByStudentNumber(string studentNumber)
        {
            return _students.FirstOrDefault(s =>
                s.StudentNumber.Equals(studentNumber.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        public bool StudentNumberExists(string studentNumber)
        {
            return FindByStudentNumber(studentNumber) != null;
        }

        public List<Student> GetStudentsByCountryCodes(IEnumerable<int> countryCodes)
        {
            var codeSet = new HashSet<int>(countryCodes);

            return _students
                .Where(s => codeSet.Contains(s.CountryCode))
                .OrderBy(s => s.Surname, StringComparer.OrdinalIgnoreCase)
                .ThenBy(s => s.FirstName, StringComparer.OrdinalIgnoreCase)
                .ToList();
        }
    }
}