export interface IModalProps {
  open: boolean;
  title?: string;
  onClose: () => void;
  content: React.ReactNode;
}
