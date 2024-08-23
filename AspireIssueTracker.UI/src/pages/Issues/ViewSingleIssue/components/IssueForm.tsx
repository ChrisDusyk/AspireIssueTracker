import { useContext, useState } from "react";
import { UserContext } from "../../../../auth/userContext";
import {
  Box,
  Button,
  FormControl,
  FormLabel,
  IconButton,
  Input,
  Link as ChakraLink,
  Select,
  Textarea,
} from "@chakra-ui/react";
import { EditIcon, ArrowBackIcon } from "@chakra-ui/icons";
import { useAuth0 } from "@auth0/auth0-react";
import { Link as RouterLink } from "react-router-dom";
import {
  Issue,
  IssuePriority,
  issuePriorityMap,
  IssueStatus,
  issueStatusMap,
  Person,
  User,
} from "../../../../types";
import CommentList from "./CommentList";
import { SubmitHandler, useForm } from "react-hook-form";
import { useQuery } from "@tanstack/react-query";
import { fetchUsers } from "../../../../services/userService";
import ErrorView from "../../../../components/ErrorView";

type FormData = {
  title: string;
  description: string;
  status: IssueStatus;
  priority: IssuePriority;
  assignedTo?: Person;
};

export type IssueFormProps = {
  issue: Issue;
  onSave: () => void;
  canEdit: boolean;
};

export default function IssueForm({ canEdit, issue }: IssueFormProps) {
  const [isEditing, setIsEditing] = useState(false);
  const { getAccessTokenSilently } = useAuth0();

  const { register, handleSubmit } = useForm<FormData>({
    defaultValues: {
      title: issue.title,
      description: issue.description,
      status: issue.status,
      priority: issue.priority,
      assignedTo: issue.assignedTo,
    },
  });

  const { data, isError, error } = useQuery<User[]>({
    queryKey: ["users"],
    queryFn: async () => {
      const token = await getAccessTokenSilently();
      const response = await fetchUsers(token);
      return response;
    },
  });

  const onSubmit: SubmitHandler<FormData> = (data: FormData) => {
    console.log(data);
    setIsEditing(false);
  };

  if (isError) {
    return <ErrorView error={error} />;
  }

  return (
    <>
      <Box p={4} borderWidth="1px" borderRadius="md">
        <IconButton
          icon={<ArrowBackIcon />}
          aria-label="Back"
          as={RouterLink}
          to="/issues"
        >
          Back to queue
        </IconButton>
        <Box display="flex" justifyContent="flex-end">
          <IconButton
            icon={<EditIcon />}
            aria-label="Edit"
            disabled={!canEdit}
            onClick={() => setIsEditing(!isEditing)}
          />
        </Box>
        <FormControl>
          <FormLabel>Title</FormLabel>
          <Input
            type="text"
            {...register("title")}
            isReadOnly={!isEditing}
            isDisabled={!isEditing}
          />
        </FormControl>
        <FormControl mt={4}>
          <FormLabel>Description</FormLabel>
          <Textarea
            {...register("description")}
            isReadOnly={!isEditing}
            isDisabled={!isEditing}
          />
        </FormControl>
        <FormControl mt={4} isReadOnly={!isEditing}>
          <FormLabel>Status</FormLabel>
          <Select {...register("status")} isReadOnly={!isEditing}>
            {Object.entries(issueStatusMap).map(([key, value]) => (
              <option key={key} value={key}>
                {value}
              </option>
            ))}
          </Select>
        </FormControl>
        <FormControl mt={4}>
          <FormLabel>Priority</FormLabel>
          <Select
            {...register("priority")}
            isReadOnly={!isEditing}
            isDisabled={!isEditing}
          >
            {Object.entries(issuePriorityMap).map(([key, value]) => (
              <option key={key} value={key}>
                {value}
              </option>
            ))}
          </Select>
        </FormControl>
        <FormControl mt={4}>
          <FormLabel>Assigned To</FormLabel>
          <Select
            {...register("assignedTo")}
            isReadOnly={!isEditing}
            isDisabled={!isEditing}
          >
            {data?.map((user) => (
              <option key={user.id} value={user.id}>
                {`${user.firstName} ${user.lastName}`}
              </option>
            ))}
          </Select>
        </FormControl>
        <Button mt={4} onClick={handleSubmit(onSubmit)} isDisabled={!isEditing}>
          Save
        </Button>
      </Box>
      <Box mt={4} p={4} borderWidth="1px" borderRadius="md">
        <CommentList comments={issue.comments} issueId={issue.id} />
      </Box>
    </>
  );
}
