import { IHour } from "@/entities/interfaces/IHour";
import { useEffect, useState } from "react";
import { YearSelect } from "../yearSelect";
import { MonthSelect } from "../monthSelect";
import { LdapUserLookup } from "../ldapUserLookup";
import { IUserLookupResult } from "@/entities/interfaces/IldapSearchUsersResult";
import { ClientLookup } from "../clientLookup";
import { IClient } from "@/entities/interfaces/IClient";
import { HoursSearchRequest } from "./hoursSearchRequest";
import { TaskCodeLookup } from "../taskCodeLookup";
import { ITaskCode } from "@/entities/interfaces/ITaskCode";
import { HoursInput } from "./hourInput";

interface HoursFormProps {
  mode: "add" | "edit";
  isNonBillable: boolean;
  isNao: boolean;
  hour: IHour | null;
  existingHours: IHour[] | null;
  showUnsavedHoursIndicator: boolean;
  onSave: (hour: IHour) => void;
  onCancel: () => void;
  //onMonthChange: (month: number) => void;
  //onYearChange: (year: number) => void;
  onRequestHours: (request: HoursSearchRequest) => void; // NEW
}

export default function HoursForm({
  mode,
  isNonBillable,
  isNao,
  hour,
  existingHours,
  showUnsavedHoursIndicator,
  onSave,
  onCancel,
  //onMonthChange,
  //onYearChange,
  onRequestHours,
}: HoursFormProps) {
  const [formData, setFormData] = useState<IHour>({
    id: hour?.id || 0,
    employeeNumber: hour?.employeeNumber || 0,
    employeeNameAssigned: hour?.employeeNameAssigned || "",
    employeeDomainAssigned: hour?.employeeDomainAssigned || "",
    customerName: hour?.customerName || "",
    customerNumber: hour?.customerNumber || "",
    month: hour?.month || 0,
    year: hour?.year || 0,
    hours: hour?.hours || 0,
    isDeleted: hour?.isDeleted || false,
    createdDateTime: hour?.createdDateTime || new Date(),
    lastUpdatedDateTime: hour?.lastUpdatedDateTime || new Date(),
    taskCode: hour?.taskCode || "",
    isEQR: hour?.isEQR || false,
    isNonBillable: hour?.isNonBillable || false,
    employeeNumberAssigned: hour?.employeeNumberAssigned || 0,
  });

  useEffect(() => {
    setHoursToAddUpdate(existingHours ?? []);
  }, [existingHours]);

  const handleSubmit = (event: React.FormEvent) => {
    event.preventDefault();
    onSave(formData);
  };

  const handleChange = (
    field: keyof IHour,
    value: string | number | boolean,
  ) => {
    setFormData((prev) => ({ ...prev, [field]: value }));
  };

  const handleHourChange = (index: number, updatedHour: IHour | null) => {
    if (!updatedHour) return;
    setHoursToAddUpdate((prev) => {
      const newHours = [...prev];
      console.log(
        "OriginalHours",
        newHours.forEach((h) => console.log(h)),
      );
      newHours[index] = updatedHour;
      setHoursToAddUpdate(newHours);
      console.log(
        "Updated",
        newHours.forEach((h) => console.log(h)),
      );
      return newHours;
    });
  };

  const [selectedLdapUser, setSelectedLdapUser] =
    useState<IUserLookupResult | null>(null);

  const [selectedClient, setSelectedClient] = useState<IClient | null>(null);
  const [selectedTaskCode, setSelectedTaskCode] = useState<ITaskCode | null>(
    null,
  );
  const [hoursToAddUpdate, setHoursToAddUpdate] = useState<IHour[]>([]);

  const handleLdapUserSelect = (ldapUser: IUserLookupResult | null) => {
    setSelectedLdapUser(ldapUser);

    if (ldapUser) {
      setFormData((prev) => ({
        ...prev,
        employeeNameAssigned: ldapUser.employeeName,
        employeeDomainAssigned: ldapUser.employeeDomain,
        employeeNumberAssigned: ldapUser.employeeNumber,
      }));
    }

    if (mode === "add") {
      loadExistingHours(ldapUser, selectedClient, formData.taskCode);
      //buildHoursToAddUpdate();
    }
  };

  const handleClientSelect = (client: IClient | null) => {
    setSelectedClient(client);

    if (mode === "add") {
      loadExistingHours(selectedLdapUser, client, formData.taskCode);
      //buildHoursToAddUpdate();
    }
  };

  const handleCodeSelect = (code: string) => {
    setFormData((prev) => ({ ...prev, taskCode: code }));

    if (mode === "add") {
      loadExistingHours(selectedLdapUser, selectedClient, code);
      //buildHoursToAddUpdate();
    }
  };

  async function loadExistingHours(
    ldapUser: IUserLookupResult | null,
    client: IClient | null,
    taskCode: string,
  ) {
    if (!ldapUser) return;

    if (!isNonBillable && !client) return;

    const request: HoursSearchRequest = {
      pagedParameters: { pageNumber: 1, pageSize: 10 },
      isNao,
      isNonBillable,
      year: 0,
      month: 0,
      forNewHours: true,
      clientNumber: client?.clientNumber ?? "",
      employeeNumber: ldapUser?.employeeNumber ?? 0,
      taskCode: isNonBillable ? taskCode : undefined,
    };

    await onRequestHours(request);
  }

  return (
    <form className="space-y-4" onSubmit={handleSubmit}>
      {mode === "edit" && (
        <div>
          <div>
            <MonthSelect
              onChange={(month) => handleChange("month", month)}
              value={formData.month}
            />
          </div>
          <div>
            <YearSelect
              onChange={(year) => handleChange("year", year)}
              value={formData.year}
            />
          </div>
        </div>
      )}
      {isNonBillable && (
        <TaskCodeLookup
          placeholder={formData.taskCode || "Search by Task Code..."}
          value={formData.taskCode ? formData.taskCode : null}
          onChange={(e) => {
            handleCodeSelect(e?.code ?? "");
          }}
          onSelect={(e) => {
            handleCodeSelect(e?.code ?? "");
          }}
        />
      )}
      {!isNonBillable && (
        <ClientLookup
          placeholder={formData.customerName || "Search client by name..."}
          value={
            selectedClient || {
              clientName: formData.customerName,
              clientNumber: formData.customerNumber,
            }
          }
          onChange={(e) => {
            console.log("ClientLookup onChange", e);
            handleClientSelect(e);
          }}
          onSelect={(e) => {
            handleClientSelect(e);
          }}
        />
      )}
      <div>
        <LdapUserLookup
          placeholder={
            formData.employeeNameAssigned || "Search user by name..."
          }
          value={selectedLdapUser}
          onChange={(e) => {
            handleLdapUserSelect(e);
          }}
          onSelect={(e) => {
            handleLdapUserSelect(e);
          }}
        ></LdapUserLookup>
      </div>
      {mode === "edit" && (
        <div>
          <label className="block mb-1 font-medium">Hours</label>
          <input
            type="number"
            value={formData.hours}
            onChange={(e) => handleChange("hours", parseFloat(e.target.value))}
            className="w-full border rounded px-2 py-1"
            required
          />
        </div>
      )}

      {mode === "add" && existingHours && existingHours.length > 0 && (
        <div className="grid grid-cols-13 gap-1">
          {existingHours.map((hour, index) => (
            <HoursInput
              key={index}
              value={hour}
              onChange={(updatedHour) => handleHourChange(index, updatedHour)}
            />
          ))}
        </div>
      )}
      {showUnsavedHoursIndicator && (
        <div className="font-semibold text-red-500">
          * Red hours indicate unsaved hours
        </div>
      )}
      <div className="flex justify-end gap-2">
        <button
          type="button"
          onClick={onCancel}
          className="border rounded px-3 py-2 hover:bg-gray-100"
        >
          Cancel
        </button>

        <button
          type="submit"
          className="bg-blue-600 text-white rounded px-3 py-2 hover:bg-blue-700"
        >
          Save
        </button>
      </div>
    </form>
  );
}
