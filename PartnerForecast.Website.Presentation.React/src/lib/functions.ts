import { IHour } from "@/entities/interfaces/IHour";

export function IsHourPast(hour: IHour): boolean {
  const today = new Date();
  const cliHourDate = new Date(hour.year, hour.month - 1, 1);
  const thisMonthDate = new Date(today.getFullYear(), today.getMonth(), 1);
  return cliHourDate < thisMonthDate;
}
