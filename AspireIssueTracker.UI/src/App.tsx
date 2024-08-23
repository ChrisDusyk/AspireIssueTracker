import { createBrowserRouter, RouterProvider } from "react-router-dom";
import { ChakraProvider } from "@chakra-ui/react";
import AuthenticationGuard from "./components/Nav/AuthenticationGuard";
import Layout from "./components/Layout/Layout";
import Home from "./pages/Home";
import TestAuth from "./pages/TestAuth";
import { Roles } from "./auth/Roles";
import { useAuth0 } from "@auth0/auth0-react";
import { UserContext, UserContextType } from "./auth/userContext";
import { extractRolesFromToken } from "./auth/extractRolesFromToken";
import { useEffect, useState } from "react";
import { fetchUser } from "./services/userService";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import UnclaimedQueue from "./pages/Issues/ViewIssues/UnclaimedQueue";
import ErrorPage from "./pages/ErrorPage";
import ViewSingleIssue from "./pages/Issues/ViewSingleIssue/ViewSingleIssue";

const router = createBrowserRouter([
  {
    path: "/",
    element: <Layout />,
    errorElement: <ErrorPage />,
    children: [
      {
        path: "/",
        element: <Home />,
      },
      {
        path: "/test",
        element: (
          <AuthenticationGuard
            component={TestAuth}
            returnTo="/test"
            requiredRoles={[Roles.Admin]}
          />
        ),
      },
      {
        path: "/issues",
        element: (
          <AuthenticationGuard
            component={UnclaimedQueue}
            returnTo="/issues"
            requiredRoles={[Roles.IssueViewer, Roles.Admin]}
          />
        ),
      },
      {
        path: "/issues/:id",
        element: (
          <AuthenticationGuard
            component={ViewSingleIssue}
            returnTo="/issues"
            requiredRoles={[Roles.IssueViewer, Roles.Admin]}
          />
        ),
      },
    ],
  },
]);

const queryClient = new QueryClient();

function App() {
  const { user, isAuthenticated, getIdTokenClaims, getAccessTokenSilently } =
    useAuth0();
  const [userInfo, setUserInfo] = useState<UserContextType | null>(null);

  useEffect(() => {
    if (isAuthenticated && user) {
      (async () => {
        const idToken = await getIdTokenClaims();
        if (idToken) {
          const token = await getAccessTokenSilently();
          const extendedUserData = await fetchUser(user.sub!, token);
          const roles = extractRolesFromToken(idToken);
          setUserInfo({
            user: {
              sub: user.sub || "",
              internalId: extendedUserData.id,
              email: user.email || "",
              username: extendedUserData.username,
              firstName: extendedUserData.firstName,
              lastName: extendedUserData.lastName,
            },
            roles,
            error: null,
          });
        }
      })();
    }
  }, [isAuthenticated, user, getIdTokenClaims, getAccessTokenSilently]);

  return (
    <ChakraProvider>
      <QueryClientProvider client={queryClient}>
        <UserContext.Provider value={userInfo}>
          <RouterProvider router={router} />
        </UserContext.Provider>
      </QueryClientProvider>
    </ChakraProvider>
  );
}

export default App;
