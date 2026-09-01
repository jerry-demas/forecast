"use client";

import { naoUser } from "@/entities/naoUser";
import { 
    PencilSquareIcon, 
    TrashIcon
} from "@heroicons/react/16/solid";
import { ColumnDef } from '@tanstack/react-table';


export const naoUsersColumns = (
    onEdit: (user: naoUser) => void,
    onDelete: (user: naoUser) => void
): ColumnDef<naoUser>[] => [
    {
        accessorKey: 'employeeNumber',
        header: 'Employee #',
    },
    {
        accessorKey: 'employeeName',
        header: 'Name',
    },
    {
        accessorKey: 'employeeDomain',
        header: 'Domain',
    },
    {
        accessorKey: 'title',
        header: 'Title',
    },
    {
        accessorKey: 'emailAddress',
        header: 'Email',
    },
    {
        accessorKey: 'isAdmin',
        header: 'Admin',
        cell: ({ row }) => (row.original.isAdmin ? 'Yes' : 'No'),
    },
    {
        accessorKey: 'isInactive',
        header: 'Inactive',
        cell: ({ row }) => (row.original.isInactive ? 'Yes' : 'No'),
    },
    {
        id: 'actions',
        header: 'Actions',
        cell: ({ row }) => (
            <div className="flex gap-2">
                <button title="Edit User" onClick={() => onEdit(row.original)}>
                    <PencilSquareIcon className="h-5 w-5 text-blue-600"  />
                </button>

                <button title="Delete User" onClick={() => onDelete(row.original)}>
                    <TrashIcon className="h-5 w-5 text-red-600" />
                </button>
                
            </div>
        ),
    },
];
