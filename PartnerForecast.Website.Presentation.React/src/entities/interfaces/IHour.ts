export interface IHour {
  id: number;
  employeeNumber: number;
  employeeNameAssigned: string;
  employeeDomainAssigned: string;
  customerName: string;
  customerNumber: string;
  month: number;
  year: number;
  hours: number;
  isDeleted: boolean;
  createdDateTime: Date;
  lastUpdatedDateTime: Date;
  taskCode: string;
  isEQR: boolean;
  isNonBillable: boolean;
  employeeNumberAssigned: number;
}
