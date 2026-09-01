using System;
using System.Collections.Generic;
using System.Text;

namespace PartnerForecast.Website.Application.Features.Hours.Models;

public record ClientHoursRequest(
        bool isEqr,
        bool isNonBillable,
        int? EmployeeNumberAssigned,
        string? CustomerNumber,
        int? Year,
        int? Month
);
