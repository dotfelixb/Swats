import { createContext, FC, useContext, useState } from "react";
import { login } from "../functions";
import { IAuthContext, ILogin, ILoginResult, IUser, IViewProps } from "../interfaces";

export const AuthContext = createContext<IAuthContext>(null!);

export const useAuth = () => useContext(AuthContext);

export const AuthProvider : FC<IViewProps> = ({ children }) => {
  const [isAuthenticated, setAuthentication] = useState(false);
  const [user, setUser] = useState<IUser>(null!);

  const signIn = async ({username, password, remember}: ILogin) : Promise<ILoginResult | null> => {
    const result = await login({username, password, remember});
    if(result !== null && result.ok){
      setAuthentication(true)
      setUser({fullname: result.fullname, token: result.token, permissions: result.permissions})

      return result;
    }
    
    return result;
  }

  const signOut = () =>{
    return false;
  }

  return (
  <AuthContext.Provider value={{ isAuthenticated, user, signIn, signOut }}>
    {children}
  </AuthContext.Provider>);
}