using System;
using System.Collections.Generic;
using System.Text;

namespace PartnerForecast.Website.Application.Features.Hours.Models;

public record ClientHoursRequest(
        bool isEqr,
        bool isNonBillable,        
        string? SearchText,
        int? Year,
        int? Month,
        bool forNewHours,
        string? ClientNumber,    
        int? EmployeeNumber ,
        string? TaskCode
);
