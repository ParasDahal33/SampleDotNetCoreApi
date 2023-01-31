namespace BusReservationSystemApi.Extension
{
    public static class StringExtension
    {
        public static string RemoveWhiteSpace(this string target)
        {
            return new string(target.Where(c => !Char.IsWhiteSpace(c)).ToArray());
        }

        public static bool BeAValidName(string name)
        {
            name = name.Replace(" ", "");
            name = name.Replace("-", "");
            return name.All(Char.IsLetter);
        }
    }
}
