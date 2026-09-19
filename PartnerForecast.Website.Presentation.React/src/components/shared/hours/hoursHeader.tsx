"use client";
import { ClockIcon } from "@heroicons/react/24/outline";

export default function HoursHeader({
  headerCaption,
}: {
  headerCaption: string;
}) {
  return (
    <div className="flex items-center justify-between">
      <div className="flex items-center justify-between">
        <div className="flex items-center gap-2">
          <ClockIcon className="h-6 w-6 text-gray-600" />
          <h1 className="text-2xl font-semibold text-gray-900">
            {headerCaption}
          </h1>
        </div>
      </div>
    </div>
  );
}
