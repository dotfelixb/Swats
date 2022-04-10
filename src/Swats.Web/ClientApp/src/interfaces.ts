/// interface use by swats.web
interface IResult {
  ok: boolean;
  type: string;
  ts: number;
  errors: string[];
}

export interface ISingleResult<T> extends IResult {
  data: T;
}

export interface IListResult<T> extends IResult {
  data: T;
}

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
  fullname: string;
  token: string;
  permissions: string[];
  errors: string[];
  ok: boolean;
  ts: Number;
}

export interface IAuthContext {
  isAuthenticated: boolean;
  browserLoaded: boolean;
  user: IUser | null;
  signIn: ({
    username,
    password,
    remember,
  }: ILogin) => Promise<ILoginResult | null>;
  signOut: () => boolean;
}

export interface IAppContext {
  post: (endPoint: string, body: FormData) => Promise<any>;
  get: (endPoint: string) => Promise<any>;
  dateFormats: { longDateFormat: string; shortDateFormat: string };
}

export interface IViewProps {
  children?: JSX.Element;
}

export enum ControlType {
  Input,
  Select,
  Multiselect,
}

export interface IPickerType {
  value: string;
  text: string;
  control: ControlType;
}

interface IDataAudit {
  status: string;

  createdBy: string;
  createdByName: string;
  createdAt: Date;

  updatedBy: string;
  updatedByName: string;
  updatedAt: Date;
}

export interface IFetchBusinessHour extends IDataAudit {
  id: string;
  name: string;
  description: string;
  timezone: string;
}

export interface IFetchTag extends IDataAudit {
  id: string;
  name: string;
  note: string;
  visibility: string;
  color: string;
}

export interface IFetchTopic extends IDataAudit {
  id: string;
  topic: string;
  department: string;
  departmentName: string;
  type: string;
  note: string;
}

export interface IFetchDepartment extends IDataAudit {
  id: string;
  code: string;
  name: string;
  manager: string;
  managerName: string;
  businessHour: string;
  businessHourName: string;
  outgoingEmail: string;
  type: string;
  response: string;
}

export interface IFetchTeam extends IDataAudit {
  id: string;
  name: string;
  department: string;
  departmentName: string;
  lead: string;
  leadName: string;
  note: string;
}

export interface IFetchType extends IDataAudit {
  id: string;
  name: string;
  description: string;
  visibility: string;
  color: string;
}

export interface IFetchAgent extends IDataAudit {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  mobile: string;
  telephone: string;
  timezone: string;
  department: string;
  departmentName: string;
  team: string;
  teamName: string;
  type: string;
  typeName: string;
  mode: string;
  note: string;
}
