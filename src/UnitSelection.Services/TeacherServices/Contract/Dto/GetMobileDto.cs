namespace UnitSelection.Services.TeacherServices.Contract.Dto;

public class GetMobileDto
{
    public GetMobileDto(string countryCallingCode, string mobileNumber)
    {
        CountryCallingCode = countryCallingCode;
        MobileNumber = mobileNumber;
    }

    public string CountryCallingCode { get; set; } 
    public string MobileNumber { get; set; }
}