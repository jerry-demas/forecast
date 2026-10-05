"use client";

import { Button } from "@headlessui/react";
import NaoUsersHeader from "./naoUsersHeader";
import {
  UsersIcon,
  PlusIcon,
  ListBulletIcon,
  ChevronUpIcon,
  ChevronDownIcon,
} from "@heroicons/react/24/solid";

import { usersService } from "../../../src/services/userServices";
import { INaoUser } from "@/entities/interfaces/INaoUser";
import { NaoUserSearchRequest } from "@/app/nao-users/naoSearchRequest";

import { useEffect, useState } from "react";
import { ScrollArea } from "@/components/ui/scroll-area";

import {
  flexRender,
  getCoreRowModel,
  useReactTable,
  getSortedRowModel,
  SortingState,
} from "@tanstack/react-table";

import { naoUsersColumns } from "./naoUsersColumnDef";
import { UserFilterBar } from "@/components/shared/filterBar";
import ConfirmDelete from "@/components/shared/deleteConfirmation";
import Modal from "@/components/shared/modalPopUp";
import NaoUserForm from "./naoUserForm";
import toastAlert from "@/components/shared/toastAlert";
import AuditLogForm from "@/components/shared/audit-log/auditLogForm";
import { LogTypes, modes } from "@/lib/partnerForecastConstants";

export default function NAOUsersPage() {
  const [naoUsers, setUsers] = useState<INaoUser[]>([]);
  const [searchText, setSearchText] = useState("");

  const [filters, setFilters] = useState({
    adminOnly: false,
    activeOnly: false,
  });
  const [isDeleteConfirmationOpen, setIsDeleteConfirmationOpen] =
    useState(false);
  const [selectedUserIdToDelete, setSelectedUserIdToDelete] = useState<
    number | null
  >(null);
  const [sorting, setSorting] = useState<SortingState>([]);
  const [showUserModal, setShowUserModal] = useState(false);
  const [showLogModal, setShowLogModal] = useState(false);
  const [selectedUser, setSelectedUser] = useState<INaoUser | null>(null);

  useEffect(() => {
    loadUsers();
  }, [filters]);

  function loadUsers(): void {
    const request: NaoUserSearchRequest = {
      searchText: searchText,
      adminOnly: filters.adminOnly,
      activeOnly: filters.activeOnly,
    };
    usersService.getNaoUsers(request).then((data) => {
      setUsers(data);
    });
  }

  function clearFilters(): void {
    setFilters({
      adminOnly: false,
      activeOnly: false,
    });
    setSearchText("");
    loadUsers();
  }

  function filterUsers(): void {
    loadUsers();
  }

  function handleDelete(id: number): void {
    setSelectedUserIdToDelete(id);
    setIsDeleteConfirmationOpen(true);
  }

  function handleConfirmDelete(): void {
    if (selectedUserIdToDelete === null) {
      return;
    }
    console.log("Selected user ID to delete:", selectedUserIdToDelete);
    usersService.deleteNaoUser(selectedUserIdToDelete).then(() => {
      toastAlert.notifySuccess("User deleted successfully");
      loadUsers();
    });
    setIsDeleteConfirmationOpen(false);
    setSelectedUserIdToDelete(null);
  }

  function handleCancelDelete(): void {
    setIsDeleteConfirmationOpen(false);
    setSelectedUserIdToDelete(null);
  }

  function handleSelectUser(user: INaoUser): void {
    setSelectedUser(user);
    setShowUserModal(true);
    console.log("Selected user:", user);
  }

  function handleAddNewUser(): void {
    setSelectedUser(null);
    setShowUserModal(true);
    console.log("Add new user:");
  }

  function handleViewLog(user: INaoUser): void {
    setSelectedUser(user);
    setShowLogModal(true);
    // Implement the logic to view the log for the selected user
  }

  async function handleSaveUser(user: INaoUser): Promise<void> {
    try {
      console.log("Saving user:", user);
      if (selectedUser) {
        await usersService.updateNaoUser(user);
      } else {
        await usersService.addNaoUser(user);
      }
      setShowUserModal(false);
      setSelectedUser(null);

      toastAlert.notifySuccess(
        `User ${selectedUser ? "updated" : "added"} successfully`,
      );
      loadUsers();
    } catch (error) {
      console.error("Error saving user:", error);

      toastAlert.notifyError(
        error instanceof Error ? error.message : "Error saving user",
      );
    }
  }

  const columns = naoUsersColumns(
    (user) => handleSelectUser(user),
    (user) => handleDelete(user.id),
    (user) => handleViewLog(user),
  );

  const table = useReactTable({
    data: naoUsers,
    columns: columns,
    state: {
      sorting,
    },
    onSortingChange: setSorting,
    getCoreRowModel: getCoreRowModel(),
    getSortedRowModel: getSortedRowModel(),
  });

  return (
    <div className="space-y-2">
      <div className="mb-5">
        <NaoUsersHeader />
      </div>
      <div className="flex h-14 pt-2 rounded-xl border border-border w-full px-2">
        <span id="newUserSpan">
          <Button
            onClick={() => handleAddNewUser()}
            title="Add New User"
            className="flex w-50 h-10 mr-3 items-center gap-2  bg-blue-600 text-white px-3 py-2 rounded-lg hover:bg-blue-700"
          >
            <PlusIcon className={`w-5 h-5`} />
            <UsersIcon className={`w-5 h-5`} />
            Add NAO User
          </Button>
        </span>

        <UserFilterBar
          searchText={searchText}
          searchTextPlaceholder="Search by name or number"
          adminOnly={filters.adminOnly}
          activeOnly={filters.activeOnly}
          showAdminFilter={true}
          showActiveFilter={true}
          onSearchChange={(value) => setSearchText(value)}
          onAdminChange={(value) =>
            setFilters({ ...filters, adminOnly: value })
          }
          onActiveChange={(value) =>
            setFilters({ ...filters, activeOnly: value })
          }
          onFilter={filterUsers}
          onClear={clearFilters}
        />

        <span id="auditLogSpan" className="ml-auto">
          <Button title="Audit Log">
            <ListBulletIcon className={`w-5 h-5`} />
          </Button>
        </span>
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
        open={showUserModal}
        title={selectedUser ? "Edit NAO User" : "Add New NAO User"}
        icon={<UsersIcon className="h-6 w-6 text-blue-600" />}
        onClose={() => setShowUserModal(false)}
      >
        <NaoUserForm
          mode={selectedUser ? modes.Edit : modes.Add}
          user={selectedUser}
          onSave={handleSaveUser}
          onCancel={() => setShowUserModal(false)}
        />
      </Modal>

      <Modal
        open={showLogModal}
        title={"Audit log"}
        icon={<ListBulletIcon className="h-6 w-6 text-blue-600" />}
        onClose={() => setShowLogModal(false)}
      >
        <AuditLogForm
          id={selectedUser?.id ?? 0}
          category={LogTypes.EqrUsers}
          onClose={() => setShowLogModal(false)}
        />
      </Modal>
    </div>
  );
}
