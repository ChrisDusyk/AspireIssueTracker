import {
  Button,
  Modal,
  ModalBody,
  ModalCloseButton,
  ModalContent,
  ModalFooter,
  ModalHeader,
  ModalOverlay,
  Textarea,
} from "@chakra-ui/react";
import { useState } from "react";
import { IssueComment } from "../../../../types";

export type EditCommentModalProps = {
  isOpen: boolean;
  onClose: () => void;
  onSave: (comment: IssueComment) => void;
  existingComment: IssueComment;
};

export default function EditCommentModal({
  isOpen,
  onClose,
  onSave,
  existingComment,
}: EditCommentModalProps) {
  const [comment, setComment] = useState(existingComment.comment);

  const handleSave = () => {
    onSave({ ...existingComment, comment });
    onClose();
  };

  return (
    <Modal isOpen={isOpen} onClose={onClose}>
      <ModalOverlay />
      <ModalContent>
        <ModalHeader>Edit Comment</ModalHeader>
        <ModalCloseButton />
        <ModalBody>
          <Textarea
            p={2}
            placeholder="Add your comment here"
            onChange={(e) => setComment(e.currentTarget.value)}
            value={comment}
          />
        </ModalBody>
        <ModalFooter>
          <Button colorScheme="blue" mr={3} onClick={handleSave}>
            Save
          </Button>
        </ModalFooter>
      </ModalContent>
    </Modal>
  );
}
