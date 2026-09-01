import { taskCode } from "@/entities/taskCode";
import { useState } from "react";

interface TaskCodeFormProps {
  mode: "add" | "edit";
  code: taskCode | null;
  onSave: (code: taskCode) => void;
  onCancel: () => void;
}

export default function TaskCodeForm({
  mode,
  code,
  onSave,
  onCancel,
}: TaskCodeFormProps) {
  const [formData, setFormData] = useState<taskCode>({
    id: code?.id || 0,
    code: code?.code || "",
    codeDescription: code?.codeDescription || "",
    isActive: code?.isActive || false,
    sort: code?.sort || 0,
  });

  const handleSubmit = (event: React.FormEvent) => {
    event.preventDefault();
    onSave(formData);
  };

  const handleChange = (
    field: keyof taskCode,
    value: string | number | boolean,
  ) => {
    setFormData((prev) => ({ ...prev, [field]: value }));
  };

  return (
    <form className="space-y-4" onSubmit={handleSubmit}>
      {/* Form fields for taskCode properties */}

      <div>
        <label className="block mb-1 font-medium">Task Code</label>
        <input
          type="text"
          value={formData.code}
          onChange={(e) => handleChange("code", e.target.value)}
          className="w-full border rounded px-2 py-1"
          required
          disabled={mode === "edit"}
        />
      </div>

      <div>
        <label className="block mb-1 font-medium">Task Code Description</label>
        <input
          type="text"
          value={formData.codeDescription}
          onChange={(e) => handleChange("codeDescription", e.target.value)}
          className="w-full border rounded px-2 py-1"
          required
        />
      </div>

      <div className="flex items-center gap-2">
        <label htmlFor="isActive" className="font-medium">
          Active
        </label>
        <input
          type="checkbox"
          id="isActive"
          checked={formData.isActive}
          onChange={(e) => handleChange("isActive", e.target.checked)}
          className="w-4 h-4"
        />
      </div>

      <div>
        <label className="block mb-1 font-medium">Sort</label>
        <input
          type="number"
          value={formData.sort}
          onChange={(e) => handleChange("sort", Number(e.target.value))}
          className="w-full border rounded px-2 py-1"
          required
          //disabled={mode === "edit"}
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
