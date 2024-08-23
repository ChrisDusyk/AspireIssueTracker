import {
  Box,
  Button,
  Flex,
  HStack,
  IconButton,
  Stack,
  useColorModeValue,
  useDisclosure,
} from "@chakra-ui/react";
import { Outlet } from "react-router-dom";
import { CloseIcon, HamburgerIcon } from "@chakra-ui/icons";
import NavLink from "../Nav/NavLink";
import { useAuth0 } from "@auth0/auth0-react";

const Links = [
  {
    to: "/",
    label: "Home",
  },
];

export default function UnauthenticatedNav() {
  const { isOpen, onOpen, onClose } = useDisclosure();
  const { loginWithRedirect, isLoading } = useAuth0();

  return (
    <>
      <Box bg={useColorModeValue("gray.100", "gray.900")} px={4} w="100%">
        <Flex h={16} alignItems={"center"} justifyContent={"space-between"}>
          <IconButton
            size={"md"}
            icon={isOpen ? <CloseIcon /> : <HamburgerIcon />}
            aria-label={"Open Menu"}
            display={{ md: "none" }}
            onClick={isOpen ? onClose : onOpen}
          />
          <HStack spacing={8} alignItems={"center"}>
            <Box>Logo</Box>
            <HStack
              as={"nav"}
              spacing={4}
              display={{ base: "none", md: "flex" }}
            >
              {Links.map((link) => (
                <NavLink key={link.to} label={link.label} to={link.to} />
              ))}
            </HStack>
          </HStack>
          <Flex alignItems={"center"}>
            <Button
              variant="solid"
              size="md"
              colorScheme="teal"
              onClick={() => loginWithRedirect()}
              isLoading={isLoading}
            >
              Login
            </Button>
          </Flex>
        </Flex>

        {isOpen ? (
          <Box pb={4} display={{ md: "none" }}>
            <Stack as={"nav"} spacing={4}>
              {Links.map((link) => (
                <NavLink key={link.to} label={link.label} to={link.to} />
              ))}
            </Stack>
          </Box>
        ) : null}
      </Box>

      <Box p={4}>
        <Outlet />
      </Box>
    </>
  );
}
