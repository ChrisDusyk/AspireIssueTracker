import { withAuthenticationRequired } from "@auth0/auth0-react";
import { Roles } from "../../auth/Roles";
import { useContext, useEffect, useState } from "react";
import Unauthorized from "../../pages/Unauthorized";
import { UserContext } from "../../auth/userContext";

export interface AuthenticationGuardProps {
  component: React.ComponentType<unknown>;
  returnTo: string;
  requiredRoles?: Roles[];
}

export default function AuthenticationGuard({
  component,
  returnTo,
  requiredRoles,
}: AuthenticationGuardProps) {
  const [isAuthorized, setIsAuthorized] = useState(false);
  const userInfo = useContext(UserContext);

  const Component = withAuthenticationRequired(component, {
    onRedirecting: () => <div>Loading...</div>,
    returnTo,
  });

  useEffect(() => {
    if (userInfo && requiredRoles) {
      const roles = userInfo.roles;
      const hasRequiredRoles = requiredRoles.some((role) =>
        roles.includes(role)
      );
      setIsAuthorized(hasRequiredRoles);
    } else {
      setIsAuthorized(true);
    }
  }, [userInfo, requiredRoles]);

  return <>{isAuthorized ? <Component /> : <Unauthorized />}</>;
}
