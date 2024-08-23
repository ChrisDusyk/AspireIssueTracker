import { useColorModeValue, Link as ChakraLink } from "@chakra-ui/react";
import { Link as ReactRouterLink } from "react-router-dom";

export interface NavLinkProps {
  to: string;
  label: string;
}

const NavLink = (props: NavLinkProps) => {
  const { to, label } = props;

  return (
    <ChakraLink
      as={ReactRouterLink}
      px={2}
      py={1}
      rounded={"md"}
      _hover={{
        textDecoration: "none",
        bg: useColorModeValue("gray.200", "gray.700"),
      }}
      to={to}
    >
      {label}
    </ChakraLink>
  );
};

export default NavLink;
