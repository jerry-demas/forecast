"use client";

import { IClient } from "@/entities/interfaces/IClient";
import { ThinkingIndicator } from "./spinner";
import { useEffect, useRef, useState } from "react";
import toastAlert from "./toastAlert";
import { clientService } from "@/services/clientService";

interface ClientLookupProps {
  value?: IClient | null;
  onChange: (client: IClient | null) => void;
  onSelect?: (client: IClient) => void;
  placeholder?: string;
}

interface ComponentState {
  showSpinner: boolean;
  showDropdown: boolean;
  searchText: string;
}

export function ClientLookup({
  onChange,
  onSelect,
  placeholder,
}: ClientLookupProps) {
  const [state, setState] = useState<ComponentState>({
    showDropdown: false,
    showSpinner: false,
    searchText: "",
  });
  const selectedRef = useRef(false);
  const [clients, setClients] = useState<IClient[]>([]);

  useEffect(() => {
    if (selectedRef.current) {
      selectedRef.current = false;
      return;
    }
    if (state.searchText.length < 3) {
      setClients([]);
      return;
    }

    const timeout = setTimeout(async () => {
      setState((prevState) => ({ ...prevState, showSpinner: true }));
      const results = await clientService.getClients(state.searchText);

      setState((prevState) => ({
        ...prevState,
        showSpinner: false,
      }));
      setClients(results);
      if (results.length === 0) {
        toastAlert.notifyWarning(
          `No clients found for the search term ${state.searchText}.`,
        );
      }
      setState((prevState) => ({ ...prevState, showDropdown: true }));
    }, 300);

    return () => clearTimeout(timeout);
  }, [state.searchText]);

  return (
    <div className="relative w-full">
      <label className="block mb-1 font-medium">Client</label>
      <input
        type="text"
        value={state.searchText}
        placeholder={placeholder}
        className="w-full rounded border px-3 py-2"
        onChange={(e) =>
          setState((prevState) => ({
            ...prevState,
            searchText: e.target.value,
          }))
        }
      />
      {state.showSpinner && <ThinkingIndicator text="Searching..." />}
      {state.showDropdown && clients.length > 0 && (
        <ul className="absolute z-50 mt-1 max-h-60 w-full overflow-auto rounded border bg-white shadow">
          {clients.map((client) => (
            <li
              key={client.clientNumber}
              className="cursor-pointer px-3 py-2 hover:bg-gray-100"
              onClick={() => {
                selectedRef.current = true;
                setState((prevState) => ({
                  ...prevState,
                  searchText: client.clientName,
                }));
                onChange(client);
                setState((prevState) => ({
                  ...prevState,
                  showDropdown: false,
                }));
              }}
              onSelect={() => (
                setState((prevState) => ({
                  ...prevState,
                  showDropdown: false,
                })),
                onSelect?.(client)
              )}
            >
              {client.clientName} ({client.clientNumber})
            </li>
          ))}
        </ul>
      )}
    </div>
  );
}
