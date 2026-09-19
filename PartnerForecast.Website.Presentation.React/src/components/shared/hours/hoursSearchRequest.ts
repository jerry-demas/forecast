import { IPagedParameters } from "@/entities/interfaces/IPagedParameters";

export interface HoursSearchRequest {
  pagedParameters: IPagedParameters;
  searchText?: string;
  isNao: boolean;
  isNonBillable: boolean;
  year: number;
  month: number;
  forNewHours: boolean;
  clientNumber?: string;
  employeeNumber?: number;
  taskCode?: string;
}
