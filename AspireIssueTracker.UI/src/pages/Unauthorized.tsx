import { Text } from "@chakra-ui/react";

export default function Unauthorized() {
  return (
    <Text fontSize="medium">
      You are not authorized to view this page. Please log in or check your
      permissions.
    </Text>
  );
}
