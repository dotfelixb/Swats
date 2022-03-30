import React, { FC } from "react";
import { Navigate, useLocation } from "react-router-dom";
import { useAuth } from "../context";
import { IViewProps } from "../interfaces";

const RequiredAuth: FC<IViewProps> = ({ children }) => {
  const { isAuthenticated } = useAuth();
  const location = useLocation();

  if (!isAuthenticated) {
    return <Navigate to="/login" state={{ from: location }} replace />
  }
  return <>{children}</>
}

export default RequiredAuth;