import { ArrowPathIcon } from "@heroicons/react/24/outline";

interface ThinkingIndicatorProps {
  text?: string;
}

export function ThinkingIndicator({
  text = "Thinking...",
}: ThinkingIndicatorProps) {
  return (
    <div className="flex items-center gap-2 text-sm text-muted-foreground">
      <ArrowPathIcon className="h-5 w-5 animate-spin" />
      <span>{text}</span>
    </div>
  );
}
