export interface ModalProps {
  open: boolean;
  title?: string;
  onClose: () => void;
  content: React.ReactNode;
}
