"use client";

import { ITaskCode } from "@/entities/interfaces/ITaskCode";
import {
  ListBulletIcon,
  PencilSquareIcon,
  TrashIcon,
} from "@heroicons/react/16/solid";
import { ColumnDef } from "@tanstack/react-table";

export const taskCodeColumns = (
  onEdit: (code: ITaskCode) => void,
  onDelete: (code: ITaskCode) => void,
  onViewLog: (code: ITaskCode) => void,
): ColumnDef<ITaskCode>[] => [
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
        <button title="View Log" onClick={() => onViewLog(row.original)}>
          <ListBulletIcon className="h-5 w-5 text-black-600" />
        </button>
      </div>
    ),
  },
];
