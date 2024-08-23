import { User } from "../types/User";

const BASE_URL = import.meta.env.VITE_USERS_API_URL as string;

export async function fetchUser(authId: string, token: string): Promise<User> {
  const response = await fetch(`${BASE_URL}/api/users/${authId}`, {
    headers: {
      Authorization: `Bearer ${token}`,
    },
  });
  if (!response.ok) {
    throw new Error(`Failed to fetch user: ${response.statusText}`);
  }
  return response.json();
}

export async function fetchUsers(token: string): Promise<User[]> {
  const response = await fetch(`${BASE_URL}/api/users`, {
    headers: {
      Authorization: `Bearer ${token}`,
    },
  });
  if (!response.ok) {
    throw new Error(`Failed to fetch users: ${response.statusText}`);
  }
  return response.json();
}
