import { IHour } from "./interfaces/IHour";

export interface ClientHoursPagedResponse {
  pageNumber: number;
  pageSize: number;
  totalRecords: number;
  records: IHour[];
}
