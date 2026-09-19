import { IChange } from "../interfaces/IChange";

export class Change implements IChange {
  field: string = "";
  valueFrom: string = "";
  valueTo: string = "";
}
