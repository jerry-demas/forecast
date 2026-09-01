import { toast } from "sonner";

export const notifySuccess = (message: string) =>
  toast.success(message, {
    position: "top-right",
    className:
      "!bg-green-600 !text-white !font-medium !rounded-lg !min-w-[400px]",
  });

export const notifyError = (message: string) =>
  toast.error(message, {
    position: "top-right",
    className:
      "!bg-red-600 !text-white !font-medium !rounded-lg !min-w-[400px]",
  });

export const notifyWarning = (message: string) =>
  toast.warning(message, {
    position: "top-right",
    className:
      "!bg-yellow-600 !text-white !font-medium !rounded-lg !min-w-[400px]",
  });

const toastAlert = {
  notifySuccess,
  notifyError,
  notifyWarning,
};

export default toastAlert;
