import { createContext, FC, useContext, useState } from "react";
import { IAppContext, IUser, IViewProps } from "../interfaces";

export const AppContext = createContext<IAppContext>(null!);

export const useApp = () => useContext(AppContext);

export const AppProvider : FC<IViewProps> = ({ children }) => {

  return (
  <AppContext.Provider value={{ }}>
    {children}
  </AppContext.Provider>);
}