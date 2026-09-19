import { Change } from "./Change";
import { IAuditLog } from "../interfaces/IAuditLog";

export class AuditLog implements IAuditLog {
  id: number = 0;
  clientHoursId: number = 0;
  logCategory: string = "";
  changedByEmployeeNumber: number = 0;
  changedByEmployeeName: string = "";
  changedByEmployeeDomain: string = "";
  changeDescription: string = "";
  changes: Change[] = [];
  createdDateTime: Date = new Date();
}
