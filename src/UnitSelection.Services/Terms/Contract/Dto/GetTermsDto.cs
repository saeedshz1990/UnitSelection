﻿namespace UnitSelection.Services.Terms.Contract.Dto;

public class GetTermsDto
{
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}