/// interface use by swats.web

export interface IUser {
  token:string;
  username:string;
  email:string;
  permissions: string[];
}

export interface IAuthContext {
  isAuthenticated: boolean;
  user: IUser | null;
}

export interface IAppContext {
  
}

export interface IViewProps {
  children?: JSX.Element
}

