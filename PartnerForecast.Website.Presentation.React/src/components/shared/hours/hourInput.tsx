"use client";

import { IHour } from "@/entities/interfaces/IHour";
import { getMonthViewValue } from "@/lib/partnerForecastConstants";

interface HoursInputProps {
  value: IHour | null;
  onChange: (value: IHour | null) => void;
}
export function HoursInput({ value, onChange }: HoursInputProps) {
  function isExistingHour(hour: IHour | null): boolean {
    if (!hour) return false;
    return hour.id > 0;
  }
  return (
    <div className="flex flex-col items-center gap-1 p-1 border border-gray-100 rounded-lg text-center">
      <span className="block text-l font-semibold text-gray-500">
        {getMonthViewValue(value?.month || 0)}
      </span>
      <span className="block text-l text-gray-400">{value?.year}</span>
      <input
        type="text"
        value={value ? value.hours.toString() : "0"}
        onChange={(e) =>
          onChange({ ...value, hours: parseFloat(e.target.value) } as IHour)
        }
        className={
          isExistingHour(value)
            ? "w-16 border border-gray-300 rounded-lg px-2 py-1 text-center mt-1 bg-gray-100"
            : "w-16 border border-red-300 rounded-lg px-2 py-1 text-center mt-1 bg-red-100"
        }
      />
    </div>
  );
}
