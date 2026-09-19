import { months } from "@/lib/partnerForecastConstants";
import { useState } from "react";

interface MonthSelectProps {
  value?: number;
  onChange?: (month: number) => void;
}

export function MonthSelect({ value = undefined, onChange }: MonthSelectProps) {
  /*
  const months: { value: number; viewValue: string }[] = [
    { value: 1, viewValue: "January" },
    { value: 2, viewValue: "February" },
    { value: 3, viewValue: "March" },
    { value: 4, viewValue: "April" },
    { value: 5, viewValue: "May" },
    { value: 6, viewValue: "June" },
    { value: 7, viewValue: "July" },
    { value: 8, viewValue: "August" },
    { value: 9, viewValue: "September" },
    { value: 10, viewValue: "October" },
    { value: 11, viewValue: "November" },
    { value: 12, viewValue: "December" },
  ];
  */
  return (
    <select
      value={value === 0 ? "" : value}
      onChange={(e) => onChange?.(Number(e.target.value) || 0)}
      className="border rounded px-2 py-1"
    >
      <option value="">Month</option>
      {months.map((month) => (
        <option key={month.value} value={month.value}>
          {month.viewValue}
        </option>
      ))}
    </select>
  );
}
