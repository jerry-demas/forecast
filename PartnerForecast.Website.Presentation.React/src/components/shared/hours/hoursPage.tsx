"use client";

import { HoursPageProps } from "@/components/shared/hours/hoursPageProps";
import HoursHeader from "./hoursHeader";
import { Button } from "@headlessui/react";
import {
  PlusIcon,
  ClockIcon,
  ChevronDownIcon,
  ChevronUpIcon,
  ListBulletIcon,
  ArrowDownTrayIcon,
  ArrowDownOnSquareIcon,
} from "@heroicons/react/24/outline";
import { UserFilterBar } from "../filterBar";
import { useEffect, useState } from "react";
import { ScrollArea } from "@/components/ui/scroll-area";
import {
  flexRender,
  getCoreRowModel,
  getSortedRowModel,
  SortingState,
  useReactTable,
} from "@tanstack/react-table";

import {
  hoursBillableColumns,
  hoursNonBillableColumns,
} from "./hoursColumnDef";
import { IHour } from "@/entities/interfaces/IHour";
import { hoursService } from "@/services/hoursService";
import { HoursSearchRequest } from "./hoursSearchRequest";
import { UserPagedControl } from "../pagedControl";
import { ClientHoursPagedResponse } from "@/entities/clientHoursPagedResponse";
import Modal from "@/components/shared/modalPopUp";
import AuditLogForm from "@/components/shared/audit-log/auditLogForm";
import {
  HoursPageHeaderCaptions,
  LogTypes,
  monthsToShow,
  modes,
} from "@/lib/partnerForecastConstants";
import HoursForm from "./hoursForm";
import toastAlert from "../toastAlert";
import ConfirmDelete from "../deleteConfirmation";
import { useUser } from "@/contexts/UserContexts";
import { IClient } from "@/entities/interfaces/IClient";
import { IUserLookupResult } from "@/entities/interfaces/IldapSearchUsersResult";

import * as XLSX from "xlsx";
import { saveAs } from "file-saver";

export function HoursPage({ isNao, headerCaption }: HoursPageProps) {
  const currentUser = useUser();
  const [searchText, setSearchText] = useState("");
  const [hoursData, setHoursData] = useState<ClientHoursPagedResponse | null>(
    null,
  );

  const [addHoursData, setAddHoursData] = useState<IHour[] | null>(null);
  const [showUnsavedMessage, setShowUnsavedMessage] = useState(false);
  const [sorting, setSorting] = useState<SortingState>([]);
  const [isNonBillable, setIsNonBillable] = useState(false);
  const [showLogModal, setShowLogModal] = useState(false);
  const [filters, setFilters] = useState({
    year: 0,
    month: 0,
    isNao: isNao,
    isNonBillable: isNonBillable,
    pageNumber: 1,
    pageSize: monthsToShow,
    lastPage: 1,
  });

  const [selectedHour, setSelectedHour] = useState<IHour | null>(null);
  const [showHoursModal, setShowHoursModal] = useState(false);
  const [isDeleteConfirmationOpen, setIsDeleteConfirmationOpen] =
    useState(false);

  useEffect(() => {
    loadHours();
  }, [filters, isNonBillable]);

  function loadHours(): void {
    const request: HoursSearchRequest = {
      pagedParameters: {
        pageNumber: filters.pageNumber,
        pageSize: filters.pageSize,
      },
      searchText: searchText,
      isNao: isNao,
      isNonBillable: isNonBillable,
      year: filters.year,
      month: filters.month,
      forNewHours: false,
      employeeNumber: currentUser?.isAdmin ? 0 : currentUser?.employeeNumber,
    };
    hoursService.getHours(request).then((data) => {
      setHoursData(data);
    });
  }

  function loadNewHours(
    request: HoursSearchRequest,
    selectedClient: IClient | null,
    selectedLdapUser: IUserLookupResult | null,
  ): void {
    setAddHoursData(null);
    hoursService.getHours(request).then((data) => {
      setAddHoursData(
        buildHoursToAddUpdate(
          data.records,
          selectedClient,
          selectedLdapUser,
          request.taskCode ?? "",
        ),
      );
    });
  }

  function buildHoursToAddUpdate(
    data: IHour[],
    selectedClient: IClient | null,
    selectedLdapUser: IUserLookupResult | null,
    taskCode: string,
  ): IHour[] {
    setAddHoursData(null);
    setShowUnsavedMessage(data.length < monthsToShow);

    const hoursToAddUpdate: IHour[] = [];

    const today = new Date();
    const startDate = new Date(today.getFullYear(), today.getMonth(), 1);

    for (let i = 0; i < monthsToShow; i++) {
      const currentDate = new Date(
        startDate.getFullYear(),
        startDate.getMonth() + i,
        1,
      );

      const month = currentDate.getMonth() + 1;
      const year = currentDate.getFullYear();

      const existingHour = data?.find(
        (h) => h.month === month && h.year === year,
      );

      const hourToAdd: IHour = existingHour ?? {
        id: 0,
        employeeNumber: currentUser!.employeeNumber,
        employeeNameAssigned: !currentUser!.isAdmin
          ? currentUser!.employeeName
          : (selectedLdapUser?.employeeName ?? ""),
        employeeDomainAssigned: !currentUser!.isAdmin
          ? currentUser!.employeeDomain
          : (selectedLdapUser?.employeeDomain ?? ""),
        customerName: selectedClient?.clientName ?? "",
        customerNumber: selectedClient?.clientNumber ?? "",
        month,
        year,
        hours: 0,
        isDeleted: false,
        createdDateTime: new Date(),
        lastUpdatedDateTime: new Date(),
        taskCode: taskCode, //'',
        isEQR: isNao,
        isNonBillable: isNonBillable,
        employeeNumberAssigned: !currentUser!.isAdmin
          ? currentUser!.employeeNumber
          : (selectedLdapUser?.employeeNumber ?? 0),
      };

      hoursToAddUpdate.push(hourToAdd);
    }
    return hoursToAddUpdate;
  }

  function HeaderCaption(): string {
    return `${headerCaption} ${
      !isNao
        ? ""
        : isNonBillable
          ? HoursPageHeaderCaptions.NonBillable
          : HoursPageHeaderCaptions.Billable
    }`;
  }

  function clearFilters(): void {
    setFilters({
      year: 0,
      month: 0,
      isNao: isNao,
      isNonBillable: isNonBillable,
      pageNumber: 1,
      pageSize: monthsToShow,
      lastPage: 1,
    });
    setSearchText("");
    loadHours();
  }
  function filterHours(): void {
    loadHours();
  }

  function handleSelectHour(hour: IHour): void {
    setShowUnsavedMessage(false);
    setSelectedHour(hour);
    setAddHoursData(null);
    setShowHoursModal(true);
  }

  function HoursFormTitle(): string {
    return `${selectedHour ? "Edit" : "Add"} ${isNao ? "NAO" : ""} ${
      isNonBillable ? "Non-Billable" : "Billable"
    } Hours`;
  }

  function handleDelete(hour: IHour): void {
    setSelectedHour(hour);
    setIsDeleteConfirmationOpen(true);
  }

  function handleViewLog(hour: IHour): void {
    setSelectedHour(hour);
    setShowLogModal(true);
  }

  async function handleSaveHour(hour: IHour | IHour[]): Promise<void> {
    try {
      const hoursToSave = Array.isArray(hour)
        ? hour.filter((h) => h.isModified)
        : [hour];
      await Promise.all(
        hoursToSave.map((h) =>
          h.id === 0 ? hoursService.addHour(h) : hoursService.updateHour(h),
        ),
      );
      setShowHoursModal(false);
      setSelectedHour(null);
      toastAlert.notifySuccess(`Client Hours saved successfully`);
      loadHours();
    } catch (error) {
      console.error("Error saving Client Hours:", error);
      toastAlert.notifyError(
        error instanceof Error ? error.message : "Error saving Client Hours",
      );
    }
  }

  function handleAddNewHour(): void {
    resetAddHoursData();
    setShowHoursModal(true);
  }

  function resetAddHoursData(): void {
    setSelectedHour(null);
    setShowUnsavedMessage(false);
    setAddHoursData(null);
  }

  const columns = isNonBillable
    ? hoursNonBillableColumns(
        (hour) => handleSelectHour(hour),
        (hour) => handleDelete(hour),
        (hour) => handleViewLog(hour),
      )
    : hoursBillableColumns(
        (hour) => handleSelectHour(hour),
        (hour) => handleDelete(hour),
        (hour) => handleViewLog(hour),
      );

  const table = useReactTable({
    data: hoursData?.records ?? [],
    columns: columns,
    state: {
      sorting,
    },
    onSortingChange: setSorting,
    getCoreRowModel: getCoreRowModel(),
    getSortedRowModel: getSortedRowModel(),
  });

  function handleConfirmDelete(): void {
    if (selectedHour === null) {
      return;
    }
    console.log("Selected hour to delete:", selectedHour.id);
    hoursService.deleteHour(selectedHour.id).then(() => {
      toastAlert.notifySuccess("Client hour deleted successfully");
      loadHours();
    });
    setIsDeleteConfirmationOpen(false);
    setSelectedHour(null);
  }

  function handleCancelDelete(): void {
    setIsDeleteConfirmationOpen(false);
    setSelectedHour(null);
  }

  function NoAccessMessage(): string {
    if (currentUser?.isInactive) {
      return `Your account is inactive or not setup. Please contact PartnerForecast administrator.`;
    }
    if (!currentUser?.isEQRUser && isNao) {
      return `You are not an NAO user. Please contact PartnerForecast administrator to be added to the NAO user list.`;
    }
    return "";
  }

  const ExportToExcel = () => {
    const exportData = hoursData?.records?.map((h) => ({
      ClientName: h.customerName,
      ClientNumber: h.customerNumber,
      Employee: h.employeeNameAssigned,
      Month: h.month,
      Year: h.year,
      Hours: h.hours,
      TaskCode: h.taskCode,
    }));
    const worksheet = XLSX.utils.json_to_sheet(exportData ?? []);
    const workbook = XLSX.utils.book_new();

    XLSX.utils.book_append_sheet(workbook, worksheet, "Hours");

    const excelBuffer = XLSX.write(workbook, {
      bookType: "xlsx",
      type: "array",
    });

    const file = new Blob([excelBuffer], {
      type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
    });

    saveAs(file, `Hours_${new Date().toISOString().split("T")[0]}.xlsx`);
  };

  return (
    <div className="space-y-2">
      <div className="mb-5">
        <HoursHeader headerCaption={HeaderCaption()} />
      </div>

      {currentUser?.isInactive || (!currentUser?.isEQRUser && isNao) ? (
        <div className="flex h-14 pt-2 rounded-xl border border-border w-full px-2">
          <span className="text-red-600 font-bold text-center w-full">
            {NoAccessMessage()}
          </span>
        </div>
      ) : (
        <div>
          <div className="flex h-14 pt-2 rounded-xl border border-border w-full px-2">
            <span id="newHoursSpan">
              <Button
                onClick={() => handleAddNewHour()}
                title="Add Hours"
                className="flex w-50 h-10 mr-3 items-center gap-2  bg-blue-600 text-white px-3 py-2 rounded-lg hover:bg-blue-700"
              >
                <PlusIcon className={`w-5 h-5`} />
                <ClockIcon className={`w-5 h-5`} />
                Add {isNao ? "NAO" : ""} Hours
              </Button>
            </span>

            <UserFilterBar
              searchText={searchText}
              searchTextPlaceholder={
                isNonBillable
                  ? "Search: task code or description"
                  : "Search: client name or number"
              }
              showMonthSelect={true}
              showYearSelect={true}
              onSearchChange={(value) => setSearchText(value)}
              onFilter={filterHours}
              onClear={clearFilters}
              onMonthChange={(month) =>
                setFilters({ ...filters, month: month })
              }
              onYearChange={(year) => setFilters({ ...filters, year: year })}
              yearValue={filters.year}
              monthValue={filters.month}
            />
            {isNao && (
              <Button
                title={HoursPageHeaderCaptions.Billable}
                className="flex w-50 h-10 mr-3 items-center gap-2  bg-blue-600 text-white px-3 py-2 rounded-lg hover:bg-blue-700"
                onClick={() => setIsNonBillable(false)}
              >
                {HoursPageHeaderCaptions.Billable}
              </Button>
            )}
            {isNao && (
              <Button
                title={HoursPageHeaderCaptions.NonBillable}
                className="flex w-50 h-10 mr-3 items-center gap-2  bg-blue-600 text-white px-3 py-2 rounded-lg hover:bg-blue-700"
                onClick={() => setIsNonBillable(true)}
              >
                {HoursPageHeaderCaptions.NonBillable}
              </Button>
            )}
            <ArrowDownTrayIcon
              className="text-green-600"
              title="Download Hours"
              onClick={() => ExportToExcel()}
            />
          </div>
          <div className="flex flex-col border rounded-lg ">
            <ScrollArea className="h-[700px] w-full">
              <table className="min-w-full border">
                <thead className="sticky top-0 z-10 bg-white">
                  {table.getHeaderGroups().map((headerGroup) => (
                    <tr key={headerGroup.id}>
                      {headerGroup.headers.map((header) => (
                        <th
                          key={header.id}
                          className="border p-2 cursor-pointer select-none text-left"
                          onClick={header.column.getToggleSortingHandler()}
                        >
                          <div className="flex items-center gap-1">
                            {flexRender(
                              header.column.columnDef.header,
                              header.getContext(),
                            )}
                            {header.column.getIsSorted() === "asc" && (
                              <ChevronUpIcon className="h-4 w-4" />
                            )}

                            {header.column.getIsSorted() === "desc" && (
                              <ChevronDownIcon className="h-4 w-4" />
                            )}
                          </div>
                        </th>
                      ))}
                    </tr>
                  ))}
                </thead>
                <tbody>
                  {hoursData?.records.length === 0 && (
                    <tr>
                      <td
                        colSpan={table.getAllColumns().length}
                        className="text-red-600 font-bold text-center"
                      >
                        No records found
                      </td>
                    </tr>
                  )}
                  {table.getRowModel().rows.map((row) => (
                    <tr key={row.id}>
                      {row.getVisibleCells().map((cell) => (
                        <td key={cell.id} className="border p-2">
                          {flexRender(
                            cell.column.columnDef.cell,
                            cell.getContext(),
                          )}
                        </td>
                      ))}
                    </tr>
                  ))}
                </tbody>
              </table>
            </ScrollArea>
            <div className="flex h-14 pt-2 rounded-xl border border-border w-full px-2">
              <UserPagedControl
                recordCount={hoursData?.totalRecords ?? 0}
                currentPage={filters.pageNumber}
                totalPages={Math.max(
                  1,
                  Math.ceil((hoursData?.totalRecords ?? 0) / filters.pageSize),
                )}
                totalRecordsPerPage={filters.pageSize}
                onChangePage={(page) =>
                  setFilters({ ...filters, pageNumber: page, lastPage: page })
                }
                onChangeRecordsPerPage={(records) =>
                  setFilters({ ...filters, pageSize: records })
                }
                previousPage={filters.lastPage}
              />
            </div>
          </div>

          <ConfirmDelete
            open={isDeleteConfirmationOpen}
            onConfirm={handleConfirmDelete}
            onCancel={handleCancelDelete}
          />
          <Modal
            open={showHoursModal}
            title={HoursFormTitle()}
            icon={<ClockIcon className="h-6 w-6 text-blue-600" />}
            onClose={() => setShowHoursModal(false)}
            width={selectedHour ? "max-w-3xl" : "max-w-7xl"}
          >
            <HoursForm
              mode={selectedHour ? modes.Edit : modes.Add}
              isNao={isNao}
              isNonBillable={isNonBillable}
              hour={selectedHour}
              onSave={handleSaveHour}
              onRequestHours={loadNewHours}
              addhours={addHoursData ?? null}
              showUnsavedHoursIndicator={showUnsavedMessage}
              onCancel={() => {
                setAddHoursData(null);
                setShowHoursModal(false);
              }}
            />
          </Modal>

          <Modal
            open={showLogModal}
            title={"Audit log"}
            icon={<ListBulletIcon className="h-6 w-6 text-blue-600" />}
            onClose={() => setShowLogModal(false)}
          >
            <AuditLogForm
              id={selectedHour?.id ?? 0}
              category={LogTypes.ClientHours}
              onClose={() => setShowLogModal(false)}
            />
          </Modal>
        </div>
      )}
    </div>
  );
}
