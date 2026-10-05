"use client";

import { monthsToShow } from "@/lib/partnerForecastConstants";
import { Button } from "../ui/button";
import { ArrowLeftIcon, ArrowRightIcon } from "@heroicons/react/24/solid";

interface UserPagedControlProps {
  currentPage: number;
  totalPages: number;
  totalRecordsPerPage: number;
  recordCount: number;
  previousPage: number;
  onChangePage: (page: number) => void;
  onChangeRecordsPerPage?: (records: number) => void;
}

export function UserPagedControl({
  currentPage,
  totalPages,
  totalRecordsPerPage,
  recordCount,
  previousPage,
  onChangePage,
  onChangeRecordsPerPage,
}: UserPagedControlProps) {
  const recordsPerPage = [monthsToShow.toString(), "25", "50", "100"];
  const minPage = 1;
  const maxPage = Math.max(totalPages, 1);

  function setCurrentPage(selectedPage: number): number {
    if (selectedPage >= minPage && selectedPage <= maxPage) {
      return selectedPage;
    }
    return previousPage;
  }

  return (
    <div className="flex items-center gap-4 flex-wrap">
      <span>
        Page {currentPage} of {totalPages} [{recordCount} total records]
      </span>

      <Button
        className="flex w-10 h-10  items-center gap-  bg-blue-600 text-white px-3 py-2 rounded-lg hover:bg-blue-700"
        onClick={() => {
          onChangePage(setCurrentPage(currentPage - 1));
        }}
      >
        <ArrowLeftIcon className="h-4 w-4" />
      </Button>
      <Button
        className="flex w-10 h-10  items-center gap-2  bg-blue-600 text-white px-3 py-2 rounded-lg hover:bg-blue-700"
        onClick={() => {
          onChangePage(setCurrentPage(currentPage + 1));
        }}
      >
        <ArrowRightIcon className="h-4 w-4" />
      </Button>
      <label className="flex items-center gap-2">
        Page
        <input
          title="Go to specific page number"
          type="number"
          min="1"
          max={maxPage}
          value={currentPage}
          onChange={(e) => {
            const page = e.target.value ? Number(e.target.value) : 1;
            onChangePage(setCurrentPage(page));
          }}
          className="border rounded px-2 py-1"
        />
      </label>
      <label className="flex items-center gap-2">
        Records per page:
        <select
          value={totalRecordsPerPage === 0 ? "" : totalRecordsPerPage}
          onChange={(e) =>
            onChangeRecordsPerPage?.(Number(e.target.value) || 0)
          }
          className="border rounded px-2 py-1"
        >
          <option value="">Records per page</option>
          {recordsPerPage.map((records) => (
            <option key={records} value={records}>
              {records}
            </option>
          ))}
        </select>
      </label>
    </div>
  );
}
