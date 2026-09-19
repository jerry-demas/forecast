import { ITaskCode } from "@/entities/interfaces/ITaskCode";
import { taskCodeService } from "@/services/taskCodeServices";
import { useEffect, useRef, useState } from "react";
import { ThinkingIndicator } from "./spinner";
import toastAlert from "./toastAlert";
import { taskCodeSearchRequest } from "@/app/task-codes/taskCodeSearchRequest";

interface TaskCodeLookUpProps {
  value?: string | null;
  onChange: (taskCode: ITaskCode | null) => void;
  onSelect?: (taskCode: ITaskCode) => void;
  placeholder?: string;
}

export function TaskCodeLookup({
  value,
  onChange,
  onSelect,
  placeholder,
}: TaskCodeLookUpProps) {
  interface TaskCodeUiState {
    showDropdown: boolean;
    taskCodes: ITaskCode[];
    searchText: string;
    selectedTaskCode: ITaskCode | null;
    showSpinner: boolean;
  }

  const [taskCodeState, setTaskCodeState] = useState<TaskCodeUiState>({
    showDropdown: false,
    taskCodes: [],
    searchText: value ?? "",
    selectedTaskCode: null,
    showSpinner: false,
  });
  const selectedRef = useRef(false);

  useEffect(() => {
    if (selectedRef.current) {
      selectedRef.current = false;
      return;
    }

    const timeout = setTimeout(async () => {
      setTaskCodeState((prevState) => ({
        ...prevState,
        showSpinner: true,
      }));
      const request: taskCodeSearchRequest = {
        searchText: taskCodeState.searchText,
        activeOnly: true,
      };
      const results = await taskCodeService.getTaskCodes(request);
      setTaskCodeState((prevState) => ({
        ...prevState,
        showSpinner: false,
        taskCodes: results,
      }));
      if (results.length === 0) {
        toastAlert.notifyWarning(
          `No codes found for the search term ${taskCodeState.searchText}.`,
        );
      }
      setTaskCodeState((prevState) => ({
        ...prevState,
        showDropdown: true,
      }));
    }, 300);

    return () => clearTimeout(timeout);
  }, [taskCodeState.searchText]);

  return (
    <div className="relative w-full">
      <label className="block mb-1 font-medium">Task Code</label>
      <input
        type="text"
        value={
          taskCodeState.selectedTaskCode?.code
            ? `${taskCodeState.selectedTaskCode.code} ${taskCodeState.selectedTaskCode.codeDescription ?? ""}`.trim()
            : taskCodeState.searchText
        }
        placeholder={placeholder}
        className="w-full rounded border px-3 py-2"
        onChange={(e) => {
          setTaskCodeState((prevState) => ({
            ...prevState,
            searchText: e.target.value,
          }));
          onChange(null);
        }}
        onFocus={() =>
          setTaskCodeState((prevState) => ({
            ...prevState,
            showDropdown: true,
          }))
        }
      />
      {taskCodeState.showSpinner && <ThinkingIndicator text="Searching..." />}
      {taskCodeState.showDropdown && taskCodeState.taskCodes.length > 0 && (
        <ul className="absolute z-50 mt-1 max-h-60 w-full overflow-auto rounded border bg-white shadow">
          {taskCodeState.taskCodes.map((taskCode) => (
            <li
              key={taskCode.code}
              className="cursor-pointer px-3 py-2 hover:bg-gray-100"
              onClick={() => {
                setTaskCodeState((prevState) => ({
                  ...prevState,
                  searchText: taskCode.code,
                  selectedTaskCode: taskCode,
                }));
                onChange(taskCode);
                onSelect?.(taskCode);
                setTaskCodeState((prevState) => ({
                  ...prevState,
                  showDropdown: false,
                }));
              }}
            >
              {taskCode.code} ({taskCode.codeDescription})
            </li>
          ))}
        </ul>
      )}
    </div>
  );
}
