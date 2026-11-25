using System;
using System.Collections.Generic;
using System.Linq;

namespace AseanPhonebook
{
    class Program
    {
        private static readonly Phonebook _phonebook = new Phonebook();

        static void Main(string[] args)
        {
            bool running = true;

            while (running)
            {
                Console.WriteLine("\nMAIN MENU");
                Console.WriteLine("[1] Store to ASEAN phonebook");
                Console.WriteLine("[2] Edit entry in ASEAN phonebook");
                Console.WriteLine("[3] Search ASEAN phonebook by country");
                Console.WriteLine("[4] Exit");
                Console.Write("Enter choice: ");

                string? choice = Console.ReadLine()?.Trim();

                switch (choice)
                {
                    case "1":
                        HandleStoreMenu();
                        break;
                    case "2":
                        HandleEditMenu();
                        break;
                    case "3":
                        HandleSearchMenu();
                        break;
                    case "4":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please select 1-4.");
                        break;
                }
            }
        }

        static void HandleStoreMenu()
        {
            bool addMore = true;

            while (addMore)
            {
                Console.WriteLine();
                string studentNumber = PromptNonEmptyString("Enter student number: ");
                if (_phonebook.StudentNumberExists(studentNumber))
                {
                    Console.WriteLine("A student with that student number already exists.");
                    continue;
                }

                string surname = PromptNonEmptyString("Enter surname: ");
                string firstName = PromptNonEmptyString("Enter first name: ");
                string occupation = PromptNonEmptyString("Enter occupation: ");
                char gender = PromptGender("Enter gender (M for male, F for female): ");
                int countryCode = PromptValidCountryCode("Enter country code: ");
                int areaCode = PromptPositiveInt("Enter area code: ");
                string phoneNumber = PromptNonEmptyString("Enter number: ");

                Student newStudent = new Student(
                    studentNumber,
                    surname,
                    firstName,
                    occupation,
                    gender,
                    countryCode,
                    areaCode,
                    phoneNumber
                );

                _phonebook.AddStudent(newStudent);

                while (true)
                {
                    Console.Write("Do you want to enter another entry [Y/N]? ");
                    string? again = Console.ReadLine()?.Trim().ToUpper();
                    if (again == "Y")
                    {
                        addMore = true;
                        break;
                    }
                    if (again == "N")
                    {
                        addMore = false;
                        break;
                    }
                    Console.WriteLine("Please enter Y or N.");
                }
            }
        }

        static void HandleEditMenu()
        {
            Console.Write("\nEnter student number: ");
            string? studentNumber = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(studentNumber))
            {
                Console.WriteLine("Error: Student number cannot be empty.");
                return;
            }

            Student? student = _phonebook.FindByStudentNumber(studentNumber);
            if (student == null)
            {
                Console.WriteLine($"Error: Student number '{studentNumber}' does not exist.");
                return;
            }

            bool editing = true;
            while (editing)
            {
                Console.WriteLine($"\nHere is the existing information about {student.StudentNumber}:");
                Console.WriteLine(student.ToEditSummary());
                Console.WriteLine("Which of the following information do you wish to change?");
                Console.WriteLine("[1] Student number [2] Surname [3] Gender [4] Occupation");
                Console.WriteLine("[5] Country code [6] Area code [7] Phone number");
                Console.WriteLine("[8] None - Go back to main menu");
                Console.Write("Enter choice: ");

                string? choice = Console.ReadLine()?.Trim();

                switch (choice)
                {
                    case "1":
                        string newNumber = PromptNonEmptyString("Enter new student number: ");
                        if (!newNumber.Equals(student.StudentNumber, StringComparison.OrdinalIgnoreCase)
                            && _phonebook.StudentNumberExists(newNumber))
                        {
                            Console.WriteLine("Error: That student number is already assigned to another entry.");
                        }
                        else
                        {
                            student.StudentNumber = newNumber;
                        }
                        break;
                    case "2":
                        student.Surname = PromptNonEmptyString("Enter new surname: ");
                        break;
                    case "3":
                        student.Gender = PromptGender("Enter new gender (M for male, F for female): ");
                        break;
                    case "4":
                        student.Occupation = PromptNonEmptyString("Enter new occupation: ");
                        break;
                    case "5":
                        student.CountryCode = PromptValidCountryCode("Enter new country code: ");
                        break;
                    case "6":
                        student.AreaCode = PromptPositiveInt("Enter new area code: ");
                        break;
                    case "7":
                        student.PhoneNumber = PromptNonEmptyString("Enter new phone number: ");
                        break;
                    case "8":
                        editing = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please select 1-8.");
                        break;
                }
            }
        }

        static void HandleSearchMenu()
        {
            List<int> selectedCodes = new List<int>();
            int choiceCount = 1;

            Console.WriteLine("\nFrom which country:");
            Console.WriteLine("[1] Philippines [2] Thailand [3] Singapore [4] Indonesia");
            Console.WriteLine("[5] Malaysia [6] ALL [0] No More");

            while (true)
            {
                Console.WriteLine($"Enter choice {choiceCount}:");
                string? input = Console.ReadLine()?.Trim();

                if (!int.TryParse(input, out int choice))
                {
                    Console.WriteLine("Invalid input. Enter a number from 0 to 6.");
                    continue;
                }

                if (choice == 0)
                {
                    break;
                }
                else if (choice == 6)
                {
                    selectedCodes = CountryHelper.CodeToName.Keys.ToList();
                    break;
                }
                else if (CountryHelper.MenuChoiceToCode.TryGetValue(choice, out int code))
                {
                    if (!selectedCodes.Contains(code))
                    {
                        selectedCodes.Add(code);
                    }
                    choiceCount++;
                }
                else
                {
                    Console.WriteLine("Invalid choice. Enter 0-6.");
                }
            }

            if (selectedCodes.Count == 0)
            {
                Console.WriteLine("No countries selected. Returning to main menu.");
                return;
            }

            List<string> countryNames = selectedCodes
                .Select(c => CountryHelper.GetCountryName(c))
                .ToList();

            string headerCountries = countryNames.Count == 5
                ? "all ASEAN countries"
                : string.Join(" and ", countryNames);

            Console.WriteLine($"\nHere are the students from {headerCountries}:");

            List<Student> results = _phonebook.GetStudentsByCountryCodes(selectedCodes);

            if (results.Count == 0)
            {
                Console.WriteLine("No student entries found.");
            }
            else
            {
                foreach (Student student in results)
                {
                    Console.WriteLine(student.ToSearchSummary());
                }
            }
        }

        // --- Input Validation Helpers ---

        static string PromptNonEmptyString(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine()?.Trim();
                if (!string.IsNullOrEmpty(input))
                {
                    return input;
                }
                Console.WriteLine("Input cannot be empty.");
            }
        }

        static char PromptGender(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine()?.Trim().ToUpper();
                if (input == "M" || input == "F")
                {
                    return input[0];
                }
                Console.WriteLine("Invalid gender. Enter M or F.");
            }
        }

        static int PromptValidCountryCode(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine()?.Trim(), out int code)
                    && CountryHelper.IsValidCountryCode(code))
                {
                    return code;
                }
                Console.WriteLine("Invalid ASEAN country code. Valid codes: 60 (Malaysia), 62 (Indonesia), 63 (Philippines), 65 (Singapore), 66 (Thailand).");
            }
        }

        static int PromptPositiveInt(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine()?.Trim(), out int value) && value > 0)
                {
                    return value;
                }
                Console.WriteLine("Please enter a valid positive number.");
            }
        }
    }
}