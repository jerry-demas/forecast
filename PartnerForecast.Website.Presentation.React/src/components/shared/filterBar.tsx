"use client";

import { Button } from "@/components/ui/button";
import { FunnelIcon, ArrowPathIcon } from "@heroicons/react/24/outline";
import { YearSelect } from "./yearSelect";
import { MonthSelect } from "./monthSelect";

interface UserFilterBarProps {
  searchText: string;
  searchTextPlaceholder?: string;

  adminOnly?: boolean;
  activeOnly?: boolean;

  onSearchChange: (value: string) => void;
  onMonthChange?: (value: number) => void;
  onYearChange?: (year: number) => void;
  onAdminChange?: (value: boolean) => void;
  onActiveChange?: (value: boolean) => void;
  showMonthSelect?: boolean;
  showYearSelect?: boolean;
  showAdminFilter?: boolean;
  showActiveFilter?: boolean;
  onFilter: () => void;
  onClear: () => void;
  monthValue?: number;
  yearValue?: number;
}

export function UserFilterBar({
  searchText,
  searchTextPlaceholder,
  adminOnly,
  activeOnly,
  onSearchChange,
  onMonthChange,
  onYearChange,
  onAdminChange,
  onActiveChange,
  onFilter,
  onClear,
  showMonthSelect,
  showYearSelect,
  showAdminFilter,
  showActiveFilter,
  monthValue,
  yearValue,
}: UserFilterBarProps) {
  return (
    <div className="flex items-center gap-4 flex-wrap">
      <input
        type="text"
        placeholder={searchTextPlaceholder ?? "Search"}
        value={searchText}
        onChange={(e) => onSearchChange(e.target.value)}
        className="w-64 border border-gray-300 rounded-lg px-3 py-2"
      />
      {showMonthSelect && (
        <MonthSelect onChange={onMonthChange} value={monthValue} />
      )}
      {showYearSelect && (
        <YearSelect onChange={onYearChange} value={yearValue} />
      )}
      {showAdminFilter && (
        <label className="flex items-center gap-2">
          <input
            type="checkbox"
            checked={adminOnly}
            onChange={(e) => onAdminChange?.(e.target.checked)}
          />
          <span>Admin Only</span>
        </label>
      )}

      {showActiveFilter && (
        <label className="flex items-center gap-2">
          <input
            type="checkbox"
            checked={activeOnly}
            onChange={(e) => onActiveChange?.(e.target.checked)}
          />
          <span>Active Only</span>
        </label>
      )}

      <Button
        onClick={onFilter}
        className="flex w-50 h-10 mr-3 items-center gap-2  bg-blue-600 text-white px-3 py-2 rounded-lg hover:bg-blue-700"
      >
        <FunnelIcon className="h-4 w-4" />
        Filter
      </Button>

      <Button
        onClick={onClear}
        variant="outline"
        className="flex w-50 h-10 mr-3 items-center gap-2  bg-blue-600 text-white px-3 py-2 rounded-lg hover:bg-blue-700"
      >
        <ArrowPathIcon className="h-4 w-4" />
        Clear
      </Button>
    </div>
  );
}
