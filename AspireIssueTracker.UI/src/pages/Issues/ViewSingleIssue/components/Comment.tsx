import { Box, IconButton, useDisclosure } from "@chakra-ui/react";
import { EditIcon } from "@chakra-ui/icons";
import type { IssueComment } from "../../../../types";
import EditCommentModal from "./EditCommentModal";

export type CommentProps = {
  comment: IssueComment;
  onSave: (comment: IssueComment) => void;
};

export default function Comment({ comment, onSave }: CommentProps) {
  const { isOpen, onOpen, onClose } = useDisclosure();

  const handleSave = (comment: IssueComment) => {
    onSave(comment);
    onClose();
  };

  return (
    <Box p={4} mt={4}>
      <IconButton icon={<EditIcon />} aria-label="Edit" onClick={onOpen} />
      <EditCommentModal
        isOpen={isOpen}
        onClose={onClose}
        onSave={handleSave}
        existingComment={comment}
      />
      <p>{comment.comment}</p>
      <p>{comment.commentedBy.name}</p>
    </Box>
  );
}
