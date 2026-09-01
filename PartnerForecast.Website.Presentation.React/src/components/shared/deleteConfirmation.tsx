interface ConfirmDeleteProps {
  open: boolean;
  onConfirm: () => void;
  onCancel: () => void;
}

export default function ConfirmDelete({
  open,
  onConfirm,
  onCancel,
}: ConfirmDeleteProps) {
  if (!open) return null;

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/50">
      <div className="w-96 rounded-lg bg-white p-6 shadow-lg">
        <h2 className="text-lg font-semibold">
          Delete Record
        </h2>

        <p className="mt-2 text-sm text-gray-600">
          Are you sure you want to delete this record?
        </p>

        <div className="mt-6 flex justify-end gap-2">
          <button
            className="rounded border px-4 py-2"
            onClick={onCancel}
          >
            Cancel
          </button>

          <button
            className="rounded bg-red-600 px-4 py-2 text-white"
            onClick={onConfirm}
          >
            Delete
          </button>
        </div>
      </div>
    </div>
  );
}