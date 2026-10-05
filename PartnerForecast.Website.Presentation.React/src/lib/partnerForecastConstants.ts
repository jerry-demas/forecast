export const LogTypes = {
  ClientHours: "ClientHours",
  EqrUsers: "EqrUsers",
  TaskCodes: "TaskCodes",
} as const;
export type LogType = (typeof LogTypes)[keyof typeof LogTypes];

export const modes = {
  Add: "add",
  Edit: "edit",
} as const;
export type Mode = (typeof modes)[keyof typeof modes];

export const AuditLogChangeTypes = {
  Add: "Added",
  Update: "Updated",
  Delete: "Deleted",
} as const;

export type AuditLogChangeType =
  (typeof AuditLogChangeTypes)[keyof typeof AuditLogChangeTypes];

export const HoursPageHeaderCaptions = {
  Billable: "Billable",
  NonBillable: "Non Billable",
} as const;

export type HoursPageHeaderCaption =
  (typeof HoursPageHeaderCaptions)[keyof typeof HoursPageHeaderCaptions];

export const monthsToShow = 13;

export const months: { value: number; viewValue: string }[] = [
  { value: 1, viewValue: "January" },
  { value: 2, viewValue: "February" },
  { value: 3, viewValue: "March" },
  { value: 4, viewValue: "April" },
  { value: 5, viewValue: "May" },
  { value: 6, viewValue: "June" },
  { value: 7, viewValue: "July" },
  { value: 8, viewValue: "August" },
  { value: 9, viewValue: "September" },
  { value: 10, viewValue: "October" },
  { value: 11, viewValue: "November" },
  { value: 12, viewValue: "December" },
];

export function getMonthViewValue(value: number): string | undefined {
  return months.find((month) => month.value === value)?.viewValue;
}
