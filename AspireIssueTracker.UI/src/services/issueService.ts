import { UserContextType } from "../auth/userContext";
import { Issue, IssueComment } from "../types";

const baseIssueUrl = import.meta.env.VITE_ISSUES_API_URL as string;

export async function fetchUnclaimedIssues(token: string): Promise<Issue[]> {
  const response = await fetch(`${baseIssueUrl}/api/issues/unclaimed`, {
    headers: { Authorization: `Bearer ${token}` },
  });
  if (!response.ok) {
    throw new Error(`Error fetching unclaimed issues: ${response.statusText}`);
  }
  return response.json();
}

export async function fetchIssueById(
  issueId: string,
  token: string
): Promise<Issue> {
  const response = await fetch(`${baseIssueUrl}/api/issues/${issueId}`, {
    headers: { Authorization: `Bearer ${token}` },
  });

  if (!response.ok) {
    throw new Error(
      `Error fetching issue: ${response.statusText || response.status}`
    );
  }

  return response.json();
}

export async function addComment(
  issueId: string,
  comment: string,
  token: string,
  user: UserContextType | null
): Promise<IssueComment> {
  const result = await fetch(`${baseIssueUrl}/api/issues/${issueId}/comments`, {
    method: "POST",
    body: JSON.stringify({
      comment,
      userId: user?.user.internalId,
    }),
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
  });

  if (!result.ok) {
    throw new Error(`Failed to add comment: ${result.statusText}`);
  }

  return result.json();
}

export async function updateComment(
  issueId: string,
  comment: IssueComment,
  token: string,
  user: UserContextType | null
): Promise<IssueComment> {
  const result = await fetch(
    `${baseIssueUrl}/api/issues/${issueId}/comments/${comment.id}`,
    {
      method: "PUT",
      body: JSON.stringify({
        comment: comment.comment,
        userId: user?.user.internalId,
      }),
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
    }
  );

  if (!result.ok) {
    throw new Error(`Failed to update comment: ${result.statusText}`);
  }

  return result.json();
}
