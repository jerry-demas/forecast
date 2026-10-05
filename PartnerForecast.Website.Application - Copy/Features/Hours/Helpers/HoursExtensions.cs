using System;
using System.Collections.Generic;
using System.Text;

namespace PartnerForecast.Website.Application.Features.Hours.Helpers;

public static class HoursExtensions
{

    public static string BuildPredicateString(Dictionary<string, string> filters)
    {
        if (filters == null || filters.Count == 0)
        {
            return string.Empty;
        }
        var predicateBuilder = new StringBuilder();
        foreach (var filter in filters)
        {
            if (predicateBuilder.Length > 0)
            {
                predicateBuilder.Append(" AND ");
            }
            predicateBuilder.Append($"{filter.Key} = '{filter.Value}'");
        }
        return predicateBuilder.ToString();
    }






}
