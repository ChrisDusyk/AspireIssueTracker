import { useQuery } from "@tanstack/react-query";
import type { Issue } from "../../../types";
import { useAuth0 } from "@auth0/auth0-react";
import QueueTable from "./components/QueueTable";
import { Loader } from "../../../components/Loader";
import ErrorView from "../../../components/ErrorView";
import { fetchUnclaimedIssues } from "../../../services/issueService";

export default function UnclaimedQueue() {
  const { getAccessTokenSilently } = useAuth0();
  const { data, isLoading, error } = useQuery<Issue[]>({
    queryKey: ["issues"],
    queryFn: async () => {
      const token = await getAccessTokenSilently();
      return fetchUnclaimedIssues(token);
    },
  });

  if (isLoading) return <Loader />;
  if (error) return <ErrorView error={error} />;

  return <QueueTable data={data || []} queueName="Unclaimed Issues" />;
}
