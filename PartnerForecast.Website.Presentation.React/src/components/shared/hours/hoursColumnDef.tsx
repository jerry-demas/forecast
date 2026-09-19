"use client";

import { IHour } from "@/entities/interfaces/IHour";
import {
  PencilSquareIcon,
  TrashIcon,
  ListBulletIcon,
} from "@heroicons/react/16/solid";
import { ColumnDef } from "@tanstack/react-table";

const commonColumns: ColumnDef<IHour>[] = [
  {
    accessorKey: "employeeNameAssigned",
    header: "Employee Name",
  },
  {
    accessorKey: "month",
    header: "Month",
  },
  {
    accessorKey: "year",
    header: "Year",
  },
  {
    accessorKey: "hours",
    header: "#Hours",
  },
];

const actionsColumn = (
  onEdit: (hour: IHour) => void,
  onDelete: (hour: IHour) => void,
  onViewLog: (hour: IHour) => void,
): ColumnDef<IHour> => ({
  id: "actions",
  header: "Actions",
  cell: ({ row }) => (
    <div className="flex gap-2">
      <button title="Edit Hours" onClick={() => onEdit(row.original)}>
        <PencilSquareIcon className="h-5 w-5 text-blue-600" />
      </button>

      <button title="Delete Hours" onClick={() => onDelete(row.original)}>
        <TrashIcon className="h-5 w-5 text-red-600" />
      </button>
      <button title="View Log" onClick={() => onViewLog(row.original)}>
        <ListBulletIcon className="h-5 w-5 text-black-600" />
      </button>
    </div>
  ),
});

export const hoursBillableColumns = (
  onEdit: (hour: IHour) => void,
  onDelete: (hour: IHour) => void,
  onViewLog: (hour: IHour) => void,
): ColumnDef<IHour>[] => [
  {
    accessorKey: "customerName",
    header: "Client Name",
  },
  {
    accessorKey: "customerNumber",
    header: "Client Number",
  },
  ...commonColumns,
  actionsColumn(onEdit, onDelete, onViewLog),
];

export const hoursNonBillableColumns = (
  onEdit: (hour: IHour) => void,
  onDelete: (hour: IHour) => void,
  onViewLog: (hour: IHour) => void,
): ColumnDef<IHour>[] => [
  {
    accessorKey: "taskCode",
    header: "Task Code",
  },
  {
    accessorKey: "taskCodeDescription",
    header: "Task Code Description",
  },

  ...commonColumns,
  actionsColumn(onEdit, onDelete, onViewLog),
];
