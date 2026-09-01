"use client";

import { taskCode } from "@/entities/taskCode";
import { PencilSquareIcon, TrashIcon } from "@heroicons/react/16/solid";
import { ColumnDef } from "@tanstack/react-table";

export const taskCodeColumns = (
  onEdit: (code: taskCode) => void,
  onDelete: (code: taskCode) => void,
): ColumnDef<taskCode>[] => [
  {
    accessorKey: "code",
    header: "Task Code",
  },
  {
    accessorKey: "codeDescription",
    header: "Task Code Description",
  },
  {
    accessorKey: "isActive",
    header: "Active",
    cell: ({ row }) => (row.original.isActive ? "Yes" : "No"),
  },
  {
    accessorKey: "sort",
    header: "Sort",
  },
  {
    id: "actions",
    header: "Actions",
    cell: ({ row }) => (
      <div className="flex gap-2">
        <button title="Edit Task" onClick={() => onEdit(row.original)}>
          <PencilSquareIcon className="h-5 w-5 text-blue-600" />
        </button>

        <button title="Delete Task" onClick={() => onDelete(row.original)}>
          <TrashIcon className="h-5 w-5 text-red-600" />
        </button>
      </div>
    ),
  },
];
