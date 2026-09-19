"use client";

import { AuditLog } from "@/entities/models/AuditLog";
import { Change } from "@/entities/models/Change";
import { ColumnDef } from "@tanstack/table-core";

export const auditLogColumns = (): ColumnDef<AuditLog>[] => [
  {
    accessorKey: "changedByEmployeeName",
    header: "Changed by Employee",
  },
  {
    accessorKey: "changeDescription",
    header: "Description",
  },

  {
    accessorKey: "changes",
    header: "Change",
    cell: ({ row }) => {
      const changes = row.original.changes as Change[];

      return (
        <div className="space-y-1">
          {changes.map((change, index) => (
            <div key={index}>
              <strong>{change.field}</strong>: {String(change.valueFrom)} →{" "}
              {String(change.valueTo)}
            </div>
          ))}
        </div>
      );
    },
  },
  {
    accessorKey: "createdDateTime",
    header: "Change Date",
  },
];
