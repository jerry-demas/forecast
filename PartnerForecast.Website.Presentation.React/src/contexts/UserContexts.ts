"use client";

import {
  createElement,
  createContext,
  useContext,
  useEffect,
  useState,
  ReactNode,
} from "react";
import type { User } from "@/entities/models/User";
import { GETCURRENTUSER } from "@/lib/apiEndPointBaseConstants";

const UserContext = createContext<User | null>(null);

export function UserProvider({ children }: { children: ReactNode }) {
  const [user, setUser] = useState<User | null>(null);
  const apiBaseUrl = process.env.NEXT_PUBLIC_API_URL ?? "";

  useEffect(() => {
    const loadUser = async () => {
      const currentUserUrl = `${apiBaseUrl}${GETCURRENTUSER}`;

      try {
        const response = await fetch(currentUserUrl, {
          credentials: "include",
        });

        if (!response.ok) {
          console.warn("Unable to load current user", {
            url: currentUserUrl,
            status: response.status,
            statusText: response.statusText,
          });
          return;
        }

        const currentUser: User = await response.json();

        setUser(currentUser);
      } catch (error) {
        console.error("Error loading current user", {
          url: currentUserUrl,
          error,
        });
      }
    };

    void loadUser();
  }, [apiBaseUrl]);

  return createElement(UserContext.Provider, { value: user }, children);
}

export function useUser() {
  return useContext(UserContext);
}
