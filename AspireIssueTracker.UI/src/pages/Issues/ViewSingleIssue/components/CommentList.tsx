import { Box, Button, Text, useDisclosure } from "@chakra-ui/react";
import { useMutation } from "@tanstack/react-query";
import { IssueComment } from "../../../../types";
import Comment from "./Comment";
import NewCommentModal from "./NewCommentModal";
import { useAuth0 } from "@auth0/auth0-react";
import { useContext, useState } from "react";
import { UserContext } from "../../../../auth/userContext";
import { addComment, updateComment } from "../../../../services/issueService";

export type CommentListProps = {
  comments: IssueComment[];
  issueId: string;
};

export default function CommentList({ comments, issueId }: CommentListProps) {
  const { isOpen, onOpen, onClose } = useDisclosure();
  const { getAccessTokenSilently } = useAuth0();
  const user = useContext(UserContext);
  const [activeComments, setActiveComments] =
    useState<IssueComment[]>(comments);

  const newCommentMutation = useMutation({
    mutationKey: ["add-comment"],
    mutationFn: async (comment: string) => {
      const token = await getAccessTokenSilently();
      const result = await addComment(issueId, comment, token, user);
      setActiveComments(
        [...activeComments, result].sort((a, b) => b.sortOrder - a.sortOrder)
      );
    },
  });

  const editCommentMutation = useMutation({
    mutationKey: ["edit-comment"],
    mutationFn: async (comment: IssueComment) => {
      const token = await getAccessTokenSilently();
      const updatedComment = await updateComment(issueId, comment, token, user);
      setActiveComments(
        activeComments.map((c) =>
          c.id === updatedComment.id ? updatedComment : c
        )
      );
    },
  });

  const handleSave = (comment: string) => {
    newCommentMutation.mutate(comment);
    onClose();
  };

  const handleUpdateComment = (comment: IssueComment) => {
    editCommentMutation.mutate(comment);
  };

  return (
    <Box>
      <Button onClick={onOpen} mb={4}>
        Add Comment
      </Button>
      <NewCommentModal isOpen={isOpen} onClose={onClose} onSave={handleSave} />
      {activeComments.length === 0 ? (
        <Text>No comments yet</Text>
      ) : (
        <Box p={4} borderWidth="1px" borderRadius="md">
          {activeComments.map((comment) => (
            <Comment
              key={comment.id}
              comment={comment}
              onSave={handleUpdateComment}
            />
          ))}
        </Box>
      )}
    </Box>
  );
}
