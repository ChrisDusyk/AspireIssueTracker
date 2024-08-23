import { useParams } from "react-router-dom";
import { Issue } from "../../../types";
import { useQuery } from "@tanstack/react-query";
import { Loader } from "../../../components/Loader";
import ErrorView from "../../../components/ErrorView";
import { useAuth0 } from "@auth0/auth0-react";
import { useContext } from "react";
import { UserContext } from "../../../auth/userContext";
import { Roles } from "../../../auth/Roles";
import IssueForm from "./components/IssueForm";
import { fetchIssueById } from "../../../services/issueService";

export default function ViewSingleIssue() {
  const { id } = useParams<{ id: string }>();
  const { getAccessTokenSilently } = useAuth0();
  const userContext = useContext(UserContext);

  const { isLoading, data, error } = useQuery<Issue>({
    queryKey: ["issue", id],
    queryFn: async () => {
      const token = await getAccessTokenSilently();
      return fetchIssueById(id!, token);
    },
    staleTime: 120000,
  });

  const canEdit =
    data?.reportedBy?.id === userContext?.user?.internalId ||
    data?.assignedTo?.id === userContext?.user?.internalId ||
    userContext?.roles.includes(Roles.Admin) ||
    userContext?.roles.includes(Roles.IssueManager) ||
    false;

  if (isLoading) return <Loader />;
  if (error) return <ErrorView error={error} />;

  return (
    <>
      {data && <IssueForm issue={data} onSave={() => {}} canEdit={canEdit} />}
    </>
  );
}
