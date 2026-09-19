"use client";

import { ScrollArea } from "@/components/ui/scroll-area";
import { AuditLog } from "@/entities/models/AuditLog";
import { auditService } from "@/services/auditService";
import {
  flexRender,
  getCoreRowModel,
  getSortedRowModel,
  SortingState,
  useReactTable,
} from "@tanstack/react-table";
import { useEffect, useState } from "react";
import { auditLogColumns } from "./auditLogColumnDef";

import { ChevronUpIcon, ChevronDownIcon } from "@heroicons/react/24/solid";

interface AuditLogFormProps {
  id: number;
  category: string;
  onClose: () => void;
}

export default function AuditLogForm({
  id,
  category,
  onClose,
}: AuditLogFormProps) {
  const [auditLogs, setAuditLogs] = useState<AuditLog[]>([]);
  const [sorting, setSorting] = useState<SortingState>([]);

  function loadAuditLog(id: number, category: string): void {
    auditService.getAuditLogs(id, category).then((data) => {
      setAuditLogs(data);
    });
  }

  useEffect(() => {
    loadAuditLog(id, category);
  }, [id, category]);

  const columns = auditLogColumns();

  const table = useReactTable({
    data: auditLogs,
    columns: columns,
    state: {
      sorting,
    },
    onSortingChange: setSorting,
    getCoreRowModel: getCoreRowModel(),
    getSortedRowModel: getSortedRowModel(),
  });

  return (
    <div className="w-full">
      <form className="space-y-2">
        <div className="flex flex-col gap-4">
          <div className="flex items-center justify-between">Log Details</div>
        </div>

        <div className="flex flex-col border rounded-lg ">
          <ScrollArea className="h-[700px] w-full">
            {auditLogs.length === 0 && (
              <div className="p-4 text-center text-gray-500">
                No audit logs available.
              </div>
            )}
            {auditLogs.length > 0 && (
              <table className="min-w-full border">
                <thead className="sticky top-0 z-10 bg-white">
                  {table.getHeaderGroups().map((headerGroup) => (
                    <tr key={headerGroup.id}>
                      {headerGroup.headers.map((header) => (
                        <th
                          key={header.id}
                          className="border p-2 cursor-pointer select-none text-left"
                          onClick={header.column.getToggleSortingHandler()}
                        >
                          <div className="flex items-center gap-1">
                            {flexRender(
                              header.column.columnDef.header,
                              header.getContext(),
                            )}
                            {header.column.getIsSorted() === "asc" && (
                              <ChevronUpIcon className="h-4 w-4" />
                            )}

                            {header.column.getIsSorted() === "desc" && (
                              <ChevronDownIcon className="h-4 w-4" />
                            )}
                          </div>
                        </th>
                      ))}
                    </tr>
                  ))}
                </thead>
                <tbody>
                  {table.getRowModel().rows.map((row) => (
                    <tr key={row.id}>
                      {row.getVisibleCells().map((cell) => (
                        <td key={cell.id} className="border p-2">
                          {flexRender(
                            cell.column.columnDef.cell,
                            cell.getContext(),
                          )}
                        </td>
                      ))}
                    </tr>
                  ))}
                </tbody>
              </table>
            )}
          </ScrollArea>
        </div>

        <div className="flex justify-end gap-2">
          <button
            type="button"
            onClick={onClose}
            className="border rounded px-3 py-2 hover:bg-gray-100"
          >
            Close
          </button>
        </div>
      </form>
    </div>
  );
}
