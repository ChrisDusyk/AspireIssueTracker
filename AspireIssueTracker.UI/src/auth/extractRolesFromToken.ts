import type { IdToken } from "@auth0/auth0-react";
import { Roles } from "./Roles";

const domain = import.meta.env.VITE_AUTH0_DOMAIN as string;
const namespace = domain.split(".")[0];

export function extractRolesFromToken(idToken: IdToken) {
  const roles = idToken[`${namespace}/roles`] as string[];
  return roles.map((role) => role as Roles);
}
