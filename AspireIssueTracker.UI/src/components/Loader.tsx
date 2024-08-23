import { Box, Spinner } from "@chakra-ui/react";

export const Loader = () => (
  <Box
    display="flex"
    justifyContent="center"
    alignItems="center"
    height="100vh"
  >
    <Spinner size="xl" color="blue.500" />
  </Box>
);
