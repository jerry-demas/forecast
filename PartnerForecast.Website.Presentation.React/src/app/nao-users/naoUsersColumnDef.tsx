"use client";

import { INaoUser } from "@/entities/interfaces/INaoUser";
import {
  ListBulletIcon,
  PencilSquareIcon,
  TrashIcon,
} from "@heroicons/react/16/solid";
import { ColumnDef } from "@tanstack/react-table";

export const naoUsersColumns = (
  onEdit: (user: INaoUser) => void,
  onDelete: (user: INaoUser) => void,
  onViewLog: (user: INaoUser) => void,
): ColumnDef<INaoUser>[] => [
  {
    accessorKey: "employeeNumber",
    header: "Employee #",
  },
  {
    accessorKey: "employeeName",
    header: "Name",
  },
  {
    accessorKey: "employeeDomain",
    header: "Domain",
  },
  {
    accessorKey: "title",
    header: "Title",
  },
  {
    accessorKey: "emailAddress",
    header: "Email",
  },
  {
    accessorKey: "isAdmin",
    header: "Admin",
    cell: ({ row }) => (row.original.isAdmin ? "Yes" : "No"),
  },
  {
    accessorKey: "isInactive",
    header: "Inactive",
    cell: ({ row }) => (row.original.isInactive ? "Yes" : "No"),
  },
  {
    id: "actions",
    header: "Actions",
    cell: ({ row }) => (
      <div className="flex gap-2">
        <button title="Edit User" onClick={() => onEdit(row.original)}>
          <PencilSquareIcon className="h-5 w-5 text-blue-600" />
        </button>
        <button title="Delete User" onClick={() => onDelete(row.original)}>
          <TrashIcon className="h-5 w-5 text-red-600" />
        </button>
        <button title="View Log" onClick={() => onViewLog(row.original)}>
          <ListBulletIcon className="h-5 w-5 text-black-600" />
        </button>
      </div>
    ),
  },
];
