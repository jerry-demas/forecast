import { useEffect, useRef, useState } from "react";
import { usersService } from "@/services/userServices";
import { IUserLookupResult } from "@/entities/interfaces/IldapSearchUsersResult";
import { ThinkingIndicator } from "./spinner";
import toastAlert from "./toastAlert";

interface UserLookupProps {
  value?: IUserLookupResult | null;
  onChange: (user: IUserLookupResult | null) => void;
  onSelect?: (user: IUserLookupResult) => void;
  //searchUsers: (searchText: string) => Promise<IUserLookupResult[]>;
  placeholder?: string;
}

export function LdapUserLookup({
  value,
  onChange,
  //searchUsers,
  onSelect,
  placeholder,
}: UserLookupProps) {
  const [searchText, setSearchText] = useState(value?.employeeName ?? "");
  const [users, setUsers] = useState<IUserLookupResult[]>([]);
  const [showDropdown, setShowDropdown] = useState(false);
  const [showSpinner, setShowSpinner] = useState(false);
  const selectedRef = useRef(false);

  useEffect(() => {
    if (selectedRef.current) {
      selectedRef.current = false;
      return;
    }

    if (searchText.length < 3) {
      setUsers([]);
      return;
    }

    const timeout = setTimeout(async () => {
      setShowSpinner(true);
      const results = await usersService.searchLdapUser(searchText);
      setShowSpinner(false);
      setUsers(results);
      if (results.length === 0) {
        toastAlert.notifyWarning(
          `No users found for the search term ${searchText}.`,
        );
      }
      setShowDropdown(true);
    }, 300);

    return () => clearTimeout(timeout);
  }, [searchText]);

  return (
    <div className="relative w-full">
      <label className="block mb-1 font-medium">User</label>
      <input
        type="text"
        value={searchText}
        placeholder={placeholder}
        className="w-full rounded border px-3 py-2"
        onChange={(e) => {
          setSearchText(e.target.value);
          onChange(null);
        }}
      />

      {showSpinner && <ThinkingIndicator text="Searching..." />}

      {showDropdown && users.length > 0 && (
        <ul className="absolute z-50 mt-1 max-h-60 w-full overflow-auto rounded border bg-white shadow">
          {users.map((user) => (
            <li
              key={user.employeeNumber}
              className="cursor-pointer px-3 py-2 hover:bg-gray-100"
              onClick={() => {
                selectedRef.current = true;
                setSearchText(user.employeeName);
                onChange(user);
                onSelect?.(user);
                setShowDropdown(false);
              }}
            >
              {user.employeeName} ({user.employeeNumber})
            </li>
          ))}
        </ul>
      )}
    </div>
  );
}
