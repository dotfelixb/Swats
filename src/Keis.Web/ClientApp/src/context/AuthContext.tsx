import { createContext, FC, useContext, useEffect, useState } from "react";
import { login } from "../functions";
import {
  IAuthContext,
  ILogin,
  ILoginResult,
  IUser,
  IViewProps,
} from "../interfaces";

export const AuthContext = createContext<IAuthContext>(null!);

export const useAuth = () => useContext(AuthContext);

export const AuthProvider: FC<IViewProps> = ({ children }) => {
  const [isAuthenticated, setAuthentication] = useState(false);
  const [browserLoaded, setBrowserLoaded] = useState(false);
  const [user, setUser] = useState<IUser>(null!);
  const isAuthKey = "sw::auth::isauth";
  const authUserKey = "sw::auth::user";

  useEffect(() => {
    // load the app from browser state
    const getIsAuth = localStorage.getItem(isAuthKey);
    const getAuthUser = localStorage.getItem(authUserKey);

    if (getIsAuth != null && getAuthUser != null) {
      const isAuth = JSON.parse(getIsAuth);
      const authUser = JSON.parse(getAuthUser);

      if (isAuth) {
        setAuthentication(isAuth);
        setUser(authUser);
      }
    }

    setBrowserLoaded(true);
  }, []);

  const signIn = async ({
    username,
    password,
    remember,
  }: ILogin): Promise<ILoginResult | null> => {
    const result = await login({ username, password, remember });
    if (result !== null && result.ok) {
      const user: IUser = {
        fullname: result.fullname,
        token: result.token,
        permissions: result.permissions,
      };

      setAuthentication(true);
      setUser(user);

      // set set
      localStorage.setItem(isAuthKey, JSON.stringify(true));
      localStorage.setItem(authUserKey, JSON.stringify(user));
    }

    return result;
  };

  const signOut = () => {
    return false;
  };

  return (
    <AuthContext.Provider
      value={{ browserLoaded, isAuthenticated, user, signIn, signOut }}
    >
      {children}
    </AuthContext.Provider>
  );
};
