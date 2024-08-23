export enum IssueStatus {
  open,
  inProgress,
  rejected,
  resolved,
  closed,
}

export enum IssuePriority {
  low,
  medium,
  high,
  critical,
}

export type Person = {
  id: string;
  name: string;
};

export type IssueComment = {
  id: string;
  sortOrder: number;
  comment: string;
  commentedBy: Person;
  commentedAt: Date;
  updatedAt: Date;
};

export type Issue = {
  id: string;
  title: string;
  description: string;
  status: IssueStatus;
  priority: IssuePriority;
  assignedTo?: Person;
  reportedBy?: Person;
  createdAt: Date;
  updatedAt: Date;
  comments: IssueComment[];
};

export const issuePriorityMap: Record<IssuePriority, string> = {
  [IssuePriority.low]: "Low",
  [IssuePriority.medium]: "Medium",
  [IssuePriority.high]: "High",
  [IssuePriority.critical]: "Critical",
};

export const issueStatusMap: Record<IssueStatus, string> = {
  [IssueStatus.open]: "Open",
  [IssueStatus.inProgress]: "In Progress",
  [IssueStatus.rejected]: "Rejected",
  [IssueStatus.resolved]: "Resolved",
  [IssueStatus.closed]: "Closed",
};
