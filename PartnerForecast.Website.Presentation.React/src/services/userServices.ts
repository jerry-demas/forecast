import {
  ADDNAOUSER,
  DELETENAOUSER,
  GETNAOUSERS,
  SEARCHLDAPUSER,
  UPDATENAOUSER,
} from "@/lib/apiEndPointBaseConstants";
import { INaoUser } from "@/entities/interfaces/INaoUser";
import { NaoUserSearchRequest } from "@/app/nao-users/naoSearchRequest";

const apiBaseUrl = process.env.NEXT_PUBLIC_API_URL ?? "";

class UsersService {
  getNaoUsers = async (request?: NaoUserSearchRequest): Promise<INaoUser[]> => {
    const naoUsersUrl = `${apiBaseUrl}${GETNAOUSERS}`;

    try {
      const queryParams = new URLSearchParams();
      if (request?.searchText) {
        queryParams.append("searchText", request.searchText);
      }

      if (request?.adminOnly) {
        queryParams.append("adminOnly", "true");
      }

      if (request?.activeOnly) {
        queryParams.append("activeOnly", "true");
      }

      const response = await fetch(`${naoUsersUrl}?${queryParams.toString()}`, {
        credentials: "include",
      });

      if (!response.ok) {
        console.warn("Unable to load NAO users", {
          url: naoUsersUrl,
          status: response.status,
          statusText: response.statusText,
        });

        return [];
      }

      const naoUsers: INaoUser[] = await response.json();

      return naoUsers;
    } catch (error) {
      console.error("Error loading NAO users", {
        url: naoUsersUrl,
        error,
      });

      return [];
    }
  };

  deleteNaoUser = async (userId: number): Promise<boolean> => {
    const naoUsersUrl = `${apiBaseUrl}${DELETENAOUSER}/${userId}`;
    try {
      const response = await fetch(naoUsersUrl, {
        method: "DELETE",
        credentials: "include",
      });

      if (!response.ok) {
        console.warn("Unable to delete NAO user", {
          url: naoUsersUrl,
          status: response.status,
          statusText: response.statusText,
        });

        return false;
      }

      return true;
    } catch (error) {
      console.error("Error deleting NAO user", {
        url: naoUsersUrl,
        error,
      });

      return false;
    }
  };

  updateNaoUser = async (user: INaoUser): Promise<boolean> => {
    const naoUsersUrl = `${apiBaseUrl}${UPDATENAOUSER}`;

    try {
      const response = await fetch(naoUsersUrl, {
        method: "PUT",
        credentials: "include",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(user),
      });

      if (!response.ok) {
        console.warn("Unable to update NAO user", {
          url: naoUsersUrl,
          status: response.status,
          statusText: response.statusText,
        });

        return false;
      }

      return true;
    } catch (error) {
      console.error("Error updating NAO user", {
        url: naoUsersUrl,
        error,
      });

      return false;
    }
  };

  addNaoUser = async (user: INaoUser): Promise<INaoUser> => {
    const naoUsersUrl = `${apiBaseUrl}${ADDNAOUSER}`;
    try {
      const response = await fetch(naoUsersUrl, {
        method: "POST",
        credentials: "include",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(user),
      });

      if (!response.ok) {
        const errorMessage = await response.text();
        throw new Error(errorMessage);
      }

      return await response.json();
    } catch (error) {
      console.error("Error adding NAO user", {
        url: naoUsersUrl,
        message: error instanceof Error ? error.message : error,
      });

      throw error;
    }
  };

  searchLdapUser = async (value: string): Promise<INaoUser[]> => {
    const searchLdapUserUrl = `${apiBaseUrl}${SEARCHLDAPUSER}?name=${encodeURIComponent(value)}`;
    try {
      const response = await fetch(searchLdapUserUrl, {
        method: "GET",
        credentials: "include",
      });

      if (!response.ok) {
        console.warn("Unable to search LDAP user", {
          url: searchLdapUserUrl,
          status: response.status,
          statusText: response.statusText,
        });

        return [];
      }
      const naoUsers: INaoUser[] = await response.json();
      return naoUsers;
    } catch (error) {
      console.error("Error searching LDAP user", {
        url: searchLdapUserUrl,
        error,
      });

      return [];
    }
  };
}

export const usersService = new UsersService();
