import { ReactNode } from "react";

interface ModalProps {
  open: boolean;
  title?: string;
  icon?: ReactNode;
  onClose: () => void;
  children: ReactNode;
  width?: string;
}

export default function Modal({
  open,
  title,
  icon,
  onClose,
  children,
  width = "max-w-3xl",
}: ModalProps) {
  if (!open) return null;

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/50">
      <div className={`bg-white rounded-lg shadow-xl w-full mx-4 ${width}`}>
        <div className="flex items-center justify-between border-b px-4 py-3">
          {icon}
          <h2 className="text-lg font-semibold">{title}</h2>

          <button onClick={onClose} className="text-gray-500 hover:text-black">
            ✕
          </button>
        </div>

        <div className="p-4">{children}</div>
      </div>
    </div>
  );
}
