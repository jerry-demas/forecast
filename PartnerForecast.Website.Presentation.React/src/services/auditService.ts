"use client";

import { AuditLog } from "@/entities/models/AuditLog";
import { ADDAUDITLOG, GETAUDITLOGS } from "@/lib/apiEndPointBaseConstants";

const apiBaseUrl = process.env.NEXT_PUBLIC_API_URL ?? "";

class AuditService {
  getAuditLogs = async (
    id?: number,
    category?: string,
  ): Promise<AuditLog[]> => {
    //id is the id of the associated record. CLient Hours Id for example.
    //This will be used to filter the audit logs for a specific record.
    const auditLogsUrl = `${apiBaseUrl}${GETAUDITLOGS}`;

    try {
      const queryParams = new URLSearchParams();

      if (id) {
        queryParams.append("id", id.toString());
      }
      if (category) {
        queryParams.append("category", category);
      }

      const url = queryParams.toString()
        ? `${auditLogsUrl}?${queryParams.toString()}`
        : auditLogsUrl;

      const response = await fetch(url, {
        credentials: "include",
      });

      const auditLogs: AuditLog[] = await response.json();

      return auditLogs;
    } catch (error) {
      console.error("Error loading audit logs", {
        url: auditLogsUrl,
        error,
      });

      throw error;
    }
  };

  addAuditLog = async (auditLog: AuditLog): Promise<void> => {
    const auditLogsUrl = `${apiBaseUrl}${ADDAUDITLOG}`;
    try {
      const response = await fetch(auditLogsUrl, {
        method: "POST",
        credentials: "include",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(auditLog),
      });

      if (!response.ok) {
        const errorMessage = await response.text();
        throw new Error(errorMessage);
      }

      return await response.json();
    } catch (error) {
      console.error("Error adding audit log", {
        url: auditLogsUrl,
        message: error instanceof Error ? error.message : error,
      });

      throw error;
    }
  };
}

export const auditService = new AuditService();
