import { useAuth0 } from "@auth0/auth0-react";
import { Box, Button, Text, VStack } from "@chakra-ui/react";
import { useContext, useState } from "react";
import { UserContext } from "../auth/userContext";

export default function TestAuth() {
  const { getAccessTokenSilently } = useAuth0();
  const [accessToken, setAccessToken] = useState<string | null>(null);
  const userContext = useContext(UserContext);

  const getAccessToken = () => {
    getAccessTokenSilently()
      .then((token) => {
        setAccessToken(token);
      })
      .catch((error) => {
        console.error(error);
      });
  };
  return (
    <VStack spacing={4}>
      <Text>
        Welcome {userContext?.user.firstName}, {userContext?.user.internalId}.
        With roles: {userContext?.roles.join(", ")}
      </Text>
      <Button onClick={getAccessToken}>Click for token</Button>
      <Box w="100%" p={4} overflowWrap="anywhere">
        {accessToken && <Text align="center">Token: {accessToken}</Text>}
      </Box>
    </VStack>
  );
}
