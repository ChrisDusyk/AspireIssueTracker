import { createContext } from "react";
import { Roles } from "./Roles";

export type UserContextType = {
  user: {
    sub: string;
    internalId: string;
    email: string;
    username: string;
    firstName: string;
    lastName: string;
  };
  roles: Roles[];
  error: unknown;
};

export const UserContext = createContext<UserContextType | null>(null);
