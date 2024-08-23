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

export type NewCommentModalProps = {
  isOpen: boolean;
  onClose: () => void;
  onSave: (comment: string) => void;
};

export default function NewCommentModal({
  isOpen,
  onClose,
  onSave,
}: NewCommentModalProps) {
  const [comment, setComment] = useState("");

  return (
    <Modal isOpen={isOpen} onClose={onClose}>
      <ModalOverlay />
      <ModalContent>
        <ModalHeader>Add Comment</ModalHeader>
        <ModalCloseButton />
        <ModalBody>
          <Textarea
            p={2}
            placeholder="Add your comment here"
            onChange={(e) => setComment(e.currentTarget.value)}
          />
        </ModalBody>
        <ModalFooter>
          <Button colorScheme="blue" mr={3} onClick={() => onSave(comment)}>
            Save
          </Button>
        </ModalFooter>
      </ModalContent>
    </Modal>
  );
}
