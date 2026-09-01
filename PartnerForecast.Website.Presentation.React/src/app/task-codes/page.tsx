"use client";

import {
  getCoreRowModel,
  getSortedRowModel,
  SortingState,
} from "@tanstack/table-core";
import TaskCodesHeader from "./taskCodesHeader";

import { Button } from "@headlessui/react";
import { taskCode } from "@/entities/taskCode";
import { useEffect, useState } from "react";
import { flexRender, useReactTable } from "@tanstack/react-table";

import { ScrollArea } from "@/components/ui/scroll-area";
import { taskCodeColumns } from "./taskCodesColumnDef";
import {
  PlusIcon,
  ListBulletIcon,
  ChevronUpIcon,
  ChevronDownIcon,
} from "@heroicons/react/24/solid";
import { taskCodeSearchRequest } from "@/entities/taskCodeSearchRequest";
import { taskCodeService } from "@/services/taskCodeServices";
import { UserFilterBar } from "@/components/shared/filterBar";
import Modal from "@/components/shared/modalPopUp";
import TaskCodeForm from "./taskCodeForm";
import toastAlert from "@/components/shared/toastAlert";
import ConfirmDelete from "@/components/shared/deleteConfirmation";

export default function TaskCodesPage() {
  const [searchText, setSearchText] = useState("");
  const [taskCodes, setCodes] = useState<taskCode[]>([]);
  const [sorting, setSorting] = useState<SortingState>([]);
  const [selectedTaskCode, setSelectedTaskCode] = useState<taskCode | null>(
    null,
  );
  const [showModal, setShowModal] = useState(false);
  const [filters, setFilters] = useState({
    activeOnly: false,
  });
  const [selectedTaskCodeToDelete, setSelectedTaskCodeToDelete] = useState<
    number | null
  >(null);
  const [isDeleteConfirmationOpen, setIsDeleteConfirmationOpen] =
    useState(false);
  useEffect(() => {
    loadTaskCodes();
  }, [filters]);

  function loadTaskCodes(): void {
    const request: taskCodeSearchRequest = {
      searchText: searchText,
      activeOnly: filters.activeOnly,
    };
    taskCodeService.getTaskCodes(request).then((data) => {
      setCodes(data);
    });
  }

  function handleDelete(id: number): void {
    console.log(`Delete task code with ID: ${id}`);
    setSelectedTaskCodeToDelete(id);
    setIsDeleteConfirmationOpen(true);
  }

  async function handleSaveTaskCode(code: taskCode): Promise<void> {
    try {
      if (selectedTaskCode) {
        await taskCodeService.updateTaskCode(code);
      } else {
        await taskCodeService.addTaskCode(code);
      }

      setShowModal(false);
      setSelectedTaskCode(null);

      toastAlert.notifySuccess(
        `Task code ${selectedTaskCode ? "updated" : "added"} successfully`,
      );

      loadTaskCodes();
    } catch (error) {
      console.error("Error saving task code:", error);

      toastAlert.notifyError(
        error instanceof Error ? error.message : "Error saving task code",
      );
    }
  }

  function filterTaskCodes(): void {
    loadTaskCodes();
  }

  function clearFilters(): void {
    setFilters({
      activeOnly: false,
    });
    setSearchText("");
    loadTaskCodes();
  }

  const columns = taskCodeColumns(
    (code) => handleSelectTaskCode(code),
    (code) => handleDelete(code.id),
  );

  const table = useReactTable({
    data: taskCodes,
    columns: columns,
    state: {
      sorting,
    },
    onSortingChange: setSorting,
    getCoreRowModel: getCoreRowModel(),
    getSortedRowModel: getSortedRowModel(),
  });

  function handleConfirmDelete(): void {
    if (selectedTaskCodeToDelete === null) {
      return;
    }
    console.log("Selected task code ID to delete:", selectedTaskCodeToDelete);
    taskCodeService.deleteTaskCode(selectedTaskCodeToDelete).then(() => {
      toastAlert.notifySuccess("Task code deleted successfully");
      loadTaskCodes();
    });
    setIsDeleteConfirmationOpen(false);
    setSelectedTaskCodeToDelete(null);
  }

  function handleCancelDelete(): void {
    setIsDeleteConfirmationOpen(false);
    setSelectedTaskCodeToDelete(null);
  }

  function handleSelectTaskCode(code: taskCode): void {
    console.log("Selected task code:", code);
    setSelectedTaskCode(code);
    setShowModal(true);
  }

  function handleAddNewCode(): void {
    console.log("Add new task code:");
    setSelectedTaskCode(null);
    setShowModal(true);
  }

  return (
    <div className="space-y-2">
      <div className="mb-5">
        <TaskCodesHeader />
      </div>

      <div className="flex h-14 pt-2 rounded-xl border border-border w-full px-2">
        <span id="newTaskCodeSpan">
          <Button
            onClick={() => handleAddNewCode()}
            title="Add New Task Code"
            className="flex w-50 h-10 mr-3 items-center gap-2  bg-blue-600 text-white px-3 py-2 rounded-lg hover:bg-blue-700"
          >
            <PlusIcon className={`w-5 h-5`} />
            <ListBulletIcon className={`w-5 h-5`} />
            Add Task Code
          </Button>
        </span>

        <UserFilterBar
          searchText={searchText}
          searchTextPlaceholder="Search by Code or Description"
          activeOnly={filters.activeOnly}
          showActiveFilter={true}
          onSearchChange={(value) => setSearchText(value)}
          onActiveChange={(value) =>
            setFilters({ ...filters, activeOnly: value })
          }
          onFilter={filterTaskCodes}
          onClear={clearFilters}
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
        <div>Records found: {table.getRowModel().rows.length}</div>
      </div>
      <ConfirmDelete
        open={isDeleteConfirmationOpen}
        onConfirm={handleConfirmDelete}
        onCancel={handleCancelDelete}
      />

      <Modal
        open={showModal}
        title={selectedTaskCode ? "Edit Task Code" : "Add New Task Code"}
        icon={<ListBulletIcon className="h-6 w-6 text-blue-600" />}
        onClose={() => setShowModal(false)}
      >
        <TaskCodeForm
          mode={selectedTaskCode ? "edit" : "add"}
          code={selectedTaskCode}
          onSave={handleSaveTaskCode}
          onCancel={() => setShowModal(false)}
        />
      </Modal>
    </div>
  );
}
