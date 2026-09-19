import { useState } from "react";

interface YearSelectProps {
  value?: number;
  onChange?: (year: number) => void;
}

export function YearSelect({ value = undefined, onChange }: YearSelectProps) {
  const minYear = 2024;
  const maxYear = new Date().getFullYear() + 1;

  const years = Array.from({ length: maxYear - minYear + 1 }, (_, i) =>
    (maxYear - i).toString(),
  );

  return (
    <select
      value={value === 0 ? "" : value}
      onChange={(e) => onChange?.(Number(e.target.value) || 0)}
      className="border rounded px-2 py-1"
    >
      <option value="">Year</option>

      {years.map((year) => (
        <option key={year} value={year}>
          {year}
        </option>
      ))}
    </select>
  );
}
