import { ADDHOUR, GETHOURS, SAVEHOUR } from "@/lib/apiEndPointBaseConstants";

import { HoursSearchRequest } from "@/components/shared/hours/hoursSearchRequest";
import { ClientHoursPagedResponse } from "@/entities/clientHoursPagedResponse";
import { IHour } from "@/entities/interfaces/IHour";

const apiBaseUrl = process.env.NEXT_PUBLIC_API_URL ?? "";

class HoursService {
  getHours = async (
    request?: HoursSearchRequest,
  ): Promise<ClientHoursPagedResponse> => {
    const hoursUrl = `${apiBaseUrl}${GETHOURS}`;
    try {
      const queryParams = new URLSearchParams();

      queryParams.append(
        "hourRequest.isEqr",
        request?.isNao ? "true" : "false",
      );

      queryParams.append(
        "hourRequest.isNonBillable",
        request?.isNonBillable ? "true" : "false",
      );
      queryParams.append(
        "hourRequest.forNewHours",
        request?.forNewHours ? "true" : "false",
      );

      if (request?.searchText) {
        queryParams.append("hourRequest.searchText", request.searchText);
      }

      if (request?.year && request.year > 0) {
        queryParams.append("hourRequest.year", request.year.toString());
      }

      if (request?.month && request.month > 0) {
        queryParams.append("hourRequest.month", request.month.toString());
      }

      if (request?.clientNumber) {
        queryParams.append("hourRequest.clientNumber", request.clientNumber);
      }
      if (request?.employeeNumber) {
        queryParams.append(
          "hourRequest.employeeNumber",
          request.employeeNumber.toString(),
        );
      }
      if (request?.taskCode) {
        queryParams.append("hourRequest.taskCode", request.taskCode);
      }

      if (
        request?.pagedParameters.pageNumber &&
        request.pagedParameters.pageNumber > 0
      ) {
        queryParams.append(
          "pageNumber",
          request.pagedParameters.pageNumber.toString(),
        );
      }

      if (
        request?.pagedParameters.pageSize &&
        request.pagedParameters.pageSize > 0
      ) {
        queryParams.append(
          "pageSize",
          request.pagedParameters.pageSize.toString(),
        );
      }

      const ss = `${hoursUrl}?${queryParams.toString()}`;

      console.log(ss);
      const response = await fetch(`${hoursUrl}?${queryParams.toString()}`, {
        credentials: "include",
      });

      if (!response.ok) {
        console.warn("Unable to load hours", {
          url: hoursUrl,
          status: response.status,
          statusText: response.statusText,
        });

        return {
          pageNumber: request?.pagedParameters.pageNumber ?? 1,
          pageSize: request?.pagedParameters.pageSize ?? 10,
          totalRecords: 0,
          records: [],
        };
      }

      const payload: ClientHoursPagedResponse = await response.json();
      return payload; //.records ?? [];
    } catch (error) {
      console.error("Error loading hours", {
        url: hoursUrl,
        error,
      });

      return {
        pageNumber: request?.pagedParameters.pageNumber ?? 1,
        pageSize: request?.pagedParameters.pageSize ?? 10,
        totalRecords: 0,
        records: [],
      };
    }
  };

  updateHour = async (hour: IHour): Promise<boolean> => {
    const hoursUrl = `${apiBaseUrl}${SAVEHOUR}`;
    try {
      const response = await fetch(hoursUrl, {
        method: "PUT",
        credentials: "include",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(hour),
      });

      if (!response.ok) {
        console.warn("Unable to update client hour", {
          url: hoursUrl,
          status: response.status,
          statusText: response.statusText,
        });

        return false;
      }

      return true;
    } catch (error) {
      console.error("Error updating client hour", {
        url: hoursUrl,
        error,
      });

      return false;
    }
  };

  deleteHour = async (hourId: number): Promise<boolean> => {
    const hoursUrl = `${apiBaseUrl}${SAVEHOUR}/${hourId}`;
    try {
      const response = await fetch(hoursUrl, {
        method: "DELETE",
        credentials: "include",
      });

      if (!response.ok) {
        console.warn("Unable to delete client hour", {
          url: hoursUrl,
          status: response.status,
          statusText: response.statusText,
        });

        return false;
      }

      return true;
    } catch (error) {
      console.error("Error deleting client hour", {
        url: hoursUrl,
        error,
      });

      return false;
    }
  };

  addHour = async (hour: IHour): Promise<IHour> => {
    const hoursUrl = `${apiBaseUrl}${ADDHOUR}`;
    try {
      const response = await fetch(hoursUrl, {
        method: "POST",
        credentials: "include",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(hour),
      });

      if (!response.ok) {
        const errorMessage = await response.text();
        throw new Error(errorMessage);
      }

      return await response.json();
    } catch (error) {
      console.error("Error adding hour", {
        url: hoursUrl,
        message: error instanceof Error ? error.message : error,
      });

      throw error;
    }
  };
}

export const hoursService = new HoursService();
