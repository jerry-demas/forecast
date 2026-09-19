import { ITaskCode } from "@/entities/interfaces/ITaskCode";
import { taskCodeSearchRequest } from "@/app/task-codes/taskCodeSearchRequest";
import {
  ADDTASKCODE,
  DELETETASKCODE,
  GETTASKCODES,
  UPDATETASKCODE,
} from "@/lib/apiEndPointBaseConstants";

const apiBaseUrl = process.env.NEXT_PUBLIC_API_URL ?? "";

class TaskCodeService {
  getTaskCodes = async (
    request?: taskCodeSearchRequest,
  ): Promise<ITaskCode[]> => {
    const taskCodesUrl = `${apiBaseUrl}${GETTASKCODES}`;
    try {
      const queryParams = new URLSearchParams();

      if (request?.searchText) {
        queryParams.append("searchText", request.searchText);
      }

      if (request?.activeOnly) {
        queryParams.append("activeOnly", "true");
      }

      const response = await fetch(
        `${taskCodesUrl}?${queryParams.toString()}`,
        {
          credentials: "include",
        },
      );

      if (!response.ok) {
        console.warn("Unable to load task codes", {
          url: taskCodesUrl,
          status: response.status,
          statusText: response.statusText,
        });

        return [];
      }

      const taskCodes: ITaskCode[] = await response.json();

      return taskCodes;
    } catch (error) {
      console.error("Error loading task codes", {
        url: taskCodesUrl,
        error,
      });

      return [];
    }
  };

  deleteTaskCode = async (taskCodeId: number): Promise<boolean> => {
    const taskCodesUrl = `${apiBaseUrl}${DELETETASKCODE}/${taskCodeId}`;
    try {
      const response = await fetch(taskCodesUrl, {
        method: "DELETE",
        credentials: "include",
      });

      if (!response.ok) {
        console.warn("Unable to delete task code", {
          url: taskCodesUrl,
          status: response.status,
          statusText: response.statusText,
        });

        return false;
      }

      return true;
    } catch (error) {
      console.error("Error deleting task code", {
        url: taskCodesUrl,
        error,
      });

      return false;
    }
  };

  updateTaskCode = async (taskCode: ITaskCode): Promise<boolean> => {
    const taskCodesUrl = `${apiBaseUrl}${UPDATETASKCODE}`;

    try {
      const response = await fetch(taskCodesUrl, {
        method: "PUT",
        credentials: "include",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(taskCode),
      });

      if (!response.ok) {
        console.warn("Unable to update task code", {
          url: taskCodesUrl,
          status: response.status,
          statusText: response.statusText,
        });

        return false;
      }

      return true;
    } catch (error) {
      console.error("Error updating task code", {
        url: taskCodesUrl,
        error,
      });

      return false;
    }
  };

  addTaskCode = async (taskCode: ITaskCode): Promise<ITaskCode> => {
    const taskCodesUrl = `${apiBaseUrl}${ADDTASKCODE}`;
    try {
      const response = await fetch(taskCodesUrl, {
        method: "POST",
        credentials: "include",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(taskCode),
      });

      if (!response.ok) {
        const errorMessage = await response.text();
        throw new Error(errorMessage);
      }

      return await response.json();
    } catch (error) {
      console.error("Error adding task code", {
        url: taskCodesUrl,
        message: error instanceof Error ? error.message : error,
      });

      throw error;
    }
  };
}

export const taskCodeService = new TaskCodeService();
