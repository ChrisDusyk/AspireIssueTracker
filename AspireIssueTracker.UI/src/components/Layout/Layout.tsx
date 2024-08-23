import { useAuth0 } from "@auth0/auth0-react";
import AuthenticatedLayout from "./AuthenticatedLayout";
import UnauthenticatedLayout from "./UnauthenticatedLayout";

export default function Layout() {
  const { isAuthenticated } = useAuth0();

  return isAuthenticated ? <AuthenticatedLayout /> : <UnauthenticatedLayout />;
}
