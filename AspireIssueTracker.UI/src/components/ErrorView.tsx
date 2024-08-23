import { CloseIcon } from "@chakra-ui/icons";
import { Box, Flex, Heading, Link as NavLink, Text } from "@chakra-ui/react";
import { NavLink as RouterLink } from "react-router-dom";

export type ErrorViewProps = {
  error: Error | null | undefined;
};

const ErrorView = ({ error }: ErrorViewProps) => (
  <Box textAlign="center" py={10} px={6}>
    <Box display="inline-block">
      <Flex
        flexDirection="column"
        justifyContent="center"
        alignItems="center"
        bg={"red.500"}
        rounded={"50px"}
        w={"55px"}
        h={"55px"}
        textAlign="center"
      >
        <CloseIcon boxSize={"20px"} color={"white"} />
      </Flex>
    </Box>
    <Heading as="h2" size="xl" mt={6} mb={2}>
      An error occurred!
    </Heading>
    <Text color={"gray.500"}>
      {error?.message || JSON.stringify(error, null, 2)}
    </Text>
    <NavLink as={RouterLink} to="/" colorScheme="teal" mt={8}>
      Back to Home
    </NavLink>
  </Box>
);

export default ErrorView;
