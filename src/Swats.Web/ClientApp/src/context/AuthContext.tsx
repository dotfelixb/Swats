import { createContext, FC, useContext, useState } from "react";
import { IAuthContext, IUser, IViewProps } from "../interfaces";

export const AuthContext = createContext<IAuthContext>(null!);

export const useAuth = () => useContext(AuthContext);

export const AuthProvider : FC<IViewProps> = ({ children }) => {
  const [isAuthenticated, setAuthentication] = useState(false);
  const [user, setUser] = useState<IUser>(null!);

  return (
  <AuthContext.Provider value={{ isAuthenticated, user }}>
    {children}
  </AuthContext.Provider>);
}