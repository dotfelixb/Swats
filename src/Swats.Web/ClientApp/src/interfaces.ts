/// interface use by swats.web

export interface IUser {
  token: string;
  fullname: string;
  permissions: string[];
}

export interface ILogin {
  username: string;
  password: string;
  remember: boolean;
}


export interface ILoginResult {
  fullname:string;
  token:string;
  permissions: string[];
  errors: string[]
  ok: boolean;
  ts:Number;
}

export interface IAuthContext {
  isAuthenticated: boolean;
  user: IUser | null;
  signIn: ({ username, password, remember }: ILogin) => Promise<ILoginResult | null>;
  signOut: () => boolean;
}

export interface IAppContext {}

export interface IViewProps {
  children?: JSX.Element;
}
