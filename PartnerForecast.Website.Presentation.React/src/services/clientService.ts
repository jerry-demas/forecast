import { IClient } from "@/entities/interfaces/IClient";
import { GETCLIENTS } from "@/lib/apiEndPointBaseConstants";

const apiBaseUrl = process.env.NEXT_PUBLIC_API_URL ?? "";

class ClientService {
  getClients = async (searchText: string): Promise<IClient[]> => {
    const clientsUrl = `${apiBaseUrl}${GETCLIENTS}`;
    try {
      const response = await fetch(`${clientsUrl}/${searchText}`, {
        credentials: "include",
      });

      if (!response.ok) {
        console.warn("Unable to load clients", {
          url: clientsUrl,
          status: response.status,
          statusText: response.statusText,
        });

        return [];
      }

      const clients: IClient[] = await response.json();

      return clients;
    } catch (error) {
      console.error("Error loading clients", {
        url: clientsUrl,
        error,
      });

      return [];
    }
  };
}
export const clientService = new ClientService();
