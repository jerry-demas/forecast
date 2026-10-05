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
import { HoursInput } from "./hourInput";
import { useUser } from "@/contexts/UserContexts";
import { monthsToShow, modes } from "@/lib/partnerForecastConstants";
import { toast } from "sonner";

interface HoursFormProps {
  mode: (typeof modes)[keyof typeof modes];
  isNonBillable: boolean;
  isNao: boolean;
  hour: IHour | null;
  addhours: IHour[] | null;
  showUnsavedHoursIndicator: boolean;
  onSave: (hour: IHour | IHour[]) => void;
  onCancel: () => void;

  onRequestHours: (
    request: HoursSearchRequest,
    selectedClient: IClient | null,
    selectedLdapUser: IUserLookupResult | null,
  ) => void; // NEW
}

export default function HoursForm({
  mode,
  isNonBillable,
  isNao,
  hour,
  addhours,
  showUnsavedHoursIndicator,
  onSave,
  onCancel,
  onRequestHours,
}: HoursFormProps) {
  const currentUser = useUser();

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
    if (mode === modes.Add && addhours && addhours.length > 0) {
      setHoursToAddUpdate(addhours);
    } else {
      setHoursToAddUpdate([]);
    }
  }, [addhours, mode]);

  const handleSubmit = (event: React.FormEvent) => {
    event.preventDefault();
    console.log("Submitting hours data:", addhours);
    if (noUpdatesMade()) {
      toast.message("No updates made. Please modify the hours before saving.");
      return;
    }
    onSave(mode === modes.Add ? addhours! : formData);
  };

  const noUpdatesMade = () => {
    if (mode === modes.Add) {
      return (
        !addhours ||
        addhours.length === 0 ||
        !addhours.some((hour) => hour.isModified)
      );
    }
    return !formData.isModified;
  };

  const handleChange = (
    field: keyof IHour,
    value: string | number | boolean,
  ) => {
    setFormData((prev) => ({ ...prev, [field]: value }));
  };

  const handleHourChange = (index: number, updatedHour: IHour | null) => {
    if (!updatedHour) return;
    addhours![index] = updatedHour;
    addhours![index].isModified = true;
    setHoursToAddUpdate([...addhours!]);
  };

  const [selectedLdapUser, setSelectedLdapUser] =
    useState<IUserLookupResult | null>(null);

  const [selectedClient, setSelectedClient] = useState<IClient | null>(null);
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

    if (mode === modes.Add) {
      loadExistingHours(ldapUser, selectedClient, formData.taskCode);
    }
  };

  const handleClientSelect = (client: IClient | null) => {
    setSelectedClient(client);

    if (mode === modes.Add) {
      loadExistingHours(selectedLdapUser, client, formData.taskCode);
    } else {
      setFormData((prev) => ({
        ...prev,
        customerName: client?.clientName ?? "",
        customerNumber: client?.clientNumber ?? "",
      }));
    }
  };

  const handleCodeSelect = (code: string) => {
    setFormData((prev) => ({ ...prev, taskCode: code }));
    if (mode === modes.Add) {
      loadExistingHours(selectedLdapUser, selectedClient, code);
    }
  };

  async function loadExistingHours(
    ldapUser: IUserLookupResult | null,
    client: IClient | null,
    taskCode: string,
  ) {
    if (currentUser?.isAdmin && !ldapUser) return;

    if (!isNonBillable && !client) return;

    const request: HoursSearchRequest = {
      pagedParameters: {
        pageNumber: 1,
        pageSize: monthsToShow,
      },
      isNao,
      isNonBillable,
      year: 0,
      month: 0,
      forNewHours: true,
      clientNumber: client?.clientNumber ?? "",
      employeeNumber: currentUser?.isAdmin
        ? ldapUser?.employeeNumber
        : (currentUser?.employeeNumber ?? 0),
      taskCode: isNonBillable ? taskCode : undefined,
    };

    await onRequestHours(request, client, ldapUser);
  }

  return (
    <form className="space-y-4" onSubmit={handleSubmit}>
      {mode === modes.Edit && (
        <div>
          <div>
            <label className="block mb-1 font-medium">Month</label>
            <MonthSelect
              onChange={(month) => handleChange("month", month)}
              value={formData.month}
            />
          </div>
          <div>
            <label className="block mb-1 font-medium">Year</label>
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
        {currentUser?.isAdmin && (
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
        )}
      </div>
      {mode === modes.Edit && (
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

      {mode === modes.Add &&
        hoursToAddUpdate &&
        hoursToAddUpdate.length > 0 && (
          <div className="grid grid-cols-13 gap-1">
            {hoursToAddUpdate.map((hour, index) => (
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
