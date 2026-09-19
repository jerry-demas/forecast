import { IChange } from "./IChange";

export interface IAuditLog {
  id: number;
  clientHoursId: number;
  logCategory: string;
  changedByEmployeeNumber: number;
  changedByEmployeeName: string;
  changedByEmployeeDomain: string;
  changeDescription: string;
  changes: IChange[];
  createdDateTime: Date;
}
