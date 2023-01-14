namespace UnitSelection.Entities;

public class Mobile
{
    public Mobile(string countryCallingCode, string mobileNumber)
    {
        CountryCallingCode = countryCallingCode;
        MobileNumber = mobileNumber;
    }

    public string CountryCallingCode { get; set; }
    public string MobileNumber { get; set; }
}