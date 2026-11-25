namespace AseanPhonebook
{
    public class Student
    {
        public string StudentNumber { get; set; }
        public string Surname { get; set; }
        public string FirstName { get; set; }
        public string Occupation { get; set; }
        public char Gender { get; set; }
        public int CountryCode { get; set; }
        public int AreaCode { get; set; }
        public string PhoneNumber { get; set; }

        public Student(
            string studentNumber,
            string surname,
            string firstName,
            string occupation,
            char gender,
            int countryCode,
            int areaCode,
            string phoneNumber)
        {
            StudentNumber = studentNumber;
            Surname = surname;
            FirstName = firstName;
            Occupation = occupation;
            Gender = char.ToUpper(gender);
            CountryCode = countryCode;
            AreaCode = areaCode;
            PhoneNumber = phoneNumber;
        }

        public string PossessivePronoun => Gender == 'F' ? "Her" : "His";

        public string FullPhoneNumber => $"{CountryCode}-{AreaCode}-{PhoneNumber}";

        // Format used in the Edit menu
        public string ToEditSummary()
        {
            return $"{FirstName} {Surname} is a {Occupation}. {PossessivePronoun} number is {FullPhoneNumber}";
        }

        // Format used in the Search results
        public string ToSearchSummary()
        {
            return $"{Surname}, {FirstName}, with student number {StudentNumber}, is a {Occupation.ToLower()}. {PossessivePronoun} phone number is {FullPhoneNumber}";
        }
    }
}