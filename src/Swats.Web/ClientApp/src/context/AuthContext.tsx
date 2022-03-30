import { createContext, FC, useContext, useState } from "react";
import { login } from "../functions";
import { IAuthContext, ILogin, IUser, IViewProps } from "../interfaces";

export const AuthContext = createContext<IAuthContext>(null!);

export const useAuth = () => useContext(AuthContext);

export const AuthProvider : FC<IViewProps> = ({ children }) => {
  const [isAuthenticated, setAuthentication] = useState(false);
  const [user, setUser] = useState<IUser>(null!);

  const signIn = async ({username, password, remember}: ILogin) : Promise<boolean> => {
    const result = await login({username, password, remember});
    if(result !== null && result.ok){
      setAuthentication(true)
      setUser({fullname: result.fullname, token: result.token, permissions: result.permissions})

      return true;
    }

    console.log(result);
    
    return false;
  }

  const signOut = () =>{
    return false;
  }

  return (
  <AuthContext.Provider value={{ isAuthenticated, user, signIn, signOut }}>
    {children}
  </AuthContext.Provider>);
}