import { LdapUserLookup } from "@/components/shared/ldapUserLookup";
import { INaoUser } from "@/entities/interfaces/INaoUser";
import { IUserLookupResult } from "@/entities/interfaces/IldapSearchUsersResult";
import { modes } from "@/lib/partnerForecastConstants";
import { FormEvent, useState } from "react";

interface NaoUserFormProps {
  mode: (typeof modes)[keyof typeof modes];
  user: INaoUser | null;
  onSave: (user: INaoUser) => void;
  onCancel: () => void;
}

export default function NaoUserForm({
  mode,
  user,
  onSave,
  onCancel,
}: NaoUserFormProps) {
  const [formData, setFormData] = useState<INaoUser>({
    id: user?.id || 0,
    employeeNumber: user?.employeeNumber || 0,
    employeeName: user?.employeeName || "",
    employeeDomain: user?.employeeDomain || "",
    title: user?.title || "",
    emailAddress: user?.emailAddress || "",
    isAdmin: user?.isAdmin || false,
    isInactive: user?.isInactive || false,
  });

  // Add state for LDAP lookup
  const [selectedLdapUser, setSelectedLdapUser] =
    useState<IUserLookupResult | null>(null);

  const handleSubmit = (e: FormEvent) => {
    e.preventDefault();
    onSave(formData);
  };

  const handleChange = (
    field: keyof INaoUser,
    value: string | number | boolean,
  ) => {
    setFormData((prev) => ({ ...prev, [field]: value }));
  };

  // Handle LDAP user selection
  const handleLdapUserSelect = (ldapUser: IUserLookupResult | null) => {
    setSelectedLdapUser(ldapUser);

    if (ldapUser) {
      // Populate form with LDAP user data
      setFormData((prev) => ({
        ...prev,
        employeeNumber: ldapUser.employeeNumber,
        employeeName: ldapUser.employeeName,
        employeeDomain: ldapUser.employeeDomain,
        title: ldapUser.title,
        emailAddress: ldapUser.emailAddress,
      }));
    }
  };

  return (
    <form className="space-y-4" onSubmit={handleSubmit}>
      {mode === modes.Add && (
        <div>
          <label className="block mb-1 font-medium">Search LDAP User</label>
          <LdapUserLookup
            value={selectedLdapUser}
            onChange={handleLdapUserSelect}
            placeholder="Search user by name..."
          />
        </div>
      )}

      <div>
        <label className="block mb-1 font-medium">Employee Number</label>
        <input
          type="number"
          value={formData.employeeNumber}
          onChange={(e) =>
            handleChange("employeeNumber", Number(e.target.value))
          }
          className="w-full border rounded px-2 py-1"
          required
          disabled={mode === modes.Edit}
        />
      </div>

      <div>
        <label className="block mb-1 font-medium">Employee Name</label>
        <input
          type="text"
          value={formData.employeeName}
          onChange={(e) => handleChange("employeeName", e.target.value)}
          className="w-full border rounded px-2 py-1"
          required
          disabled={mode === modes.Edit}
        />
      </div>

      <div>
        <label className="block mb-1 font-medium">Domain</label>
        <input
          type="text"
          value={formData.employeeDomain}
          onChange={(e) => handleChange("employeeDomain", e.target.value)}
          className="w-full border rounded px-2 py-1"
          required
          disabled={mode === modes.Edit}
        />
      </div>

      <div>
        <label className="block mb-1 font-medium">Title</label>
        <input
          type="text"
          value={formData.title}
          onChange={(e) => handleChange("title", e.target.value)}
          className="w-full border rounded px-2 py-1"
          required
        />
      </div>

      <div>
        <label className="block mb-1 font-medium">Email</label>
        <input
          type="email"
          value={formData.emailAddress}
          onChange={(e) => handleChange("emailAddress", e.target.value)}
          className="w-full border rounded px-2 py-1"
          required
        />
      </div>

      <div className="flex items-center gap-2">
        <label htmlFor="isAdmin" className="font-medium">
          Admin
        </label>
        <input
          type="checkbox"
          id="isAdmin"
          checked={formData.isAdmin}
          onChange={(e) => handleChange("isAdmin", e.target.checked)}
          className="w-4 h-4"
        />
      </div>

      <div className="flex items-center gap-2">
        <label htmlFor="isInactive" className="font-medium">
          Inactive
        </label>
        <input
          type="checkbox"
          id="isInactive"
          checked={formData.isInactive}
          onChange={(e) => handleChange("isInactive", e.target.checked)}
          className="w-4 h-4"
        />
      </div>

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
