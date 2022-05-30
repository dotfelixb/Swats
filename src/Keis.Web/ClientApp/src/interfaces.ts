/// interface use by keis.web
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
  email: string;
  permissions: string[];
}

export interface ILogin {
  username: string;
  password: string;
  remember: boolean;
}

export interface ILoginResult {
  fullname: string;
  email: string;
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
  signOut: () => void;
}

export interface IAppContext {
  post: (endPoint: string, body: FormData) => Promise<any>;
  get: (endPoint: string) => Promise<any>;
  patch: (endPoint: string, body: FormData) => Promise<any>;
  dateFormats: { longDateFormat: string; shortDateFormat: string };
  editorFormats: string[];
  editorModels: any;
}

export interface IViewProps {
  children?: JSX.Element;
}

export interface IOpenHour {
  id: number;
  name: string;
  enabled: boolean;
  fullDay: boolean;
  fromTime: string | undefined;
  toTime: string | undefined;
}

export enum ControlType {
  Input = 1,
  Select,
  Multiselect,
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
  openHours: IOpenHour[];
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
  name: string;
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
  name: string;
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

export interface IFetchSla extends IDataAudit {
  id: string;
  name: string;
  note: string;
  businessHour: string;
  businessHourName: string;
  responsePeriod: number;
  responseFormat: string;
  responseNotify: boolean;
  responseEmail: boolean;
  resolvePeriod: number;
  resolveFormat: string;
  resolveNotify: boolean;
  resolveEmail: boolean;
}

export interface IList {
  id: string;
  name: string;
}

export interface IWorkflowEvent {
  name: string;
  type: string;
}

export interface IWorkflowCriteria {
  key: number;
  name?: string;
  criteria?: number;
  condition?: number;
  match?: string;
  control?: ControlType;
  type?: string;
  link?: string;
}

export interface IWorkflowAction {
  key: number;
  name?: string;
  actionFrom?: number;
  actionTo?: string;
  control?: ControlType;
  link?: string;
}

export interface IFetchWorkflow extends IDataAudit {
  id: string;
  name: string;
  priority: string;
  note: string;
  events: IWorkflowEvent[];
  criteria: IWorkflowCriteria[];
  actions: IWorkflowAction[];
}

interface ITicketComment {
  id: string;
  ticket: string;
  body: string;
  fromEmail: string;
  fromName: string;
  createdAt: string;
}

export interface IFetchTicket extends IDataAudit {
  id: string;
  subject: string;
  code: string;
  requester: string;
  requesterName: string;
  assignedTo: string;
  assignedToName: string;
  department: string;
  departmentName: string;
  team: string;
  teamName: string;
  sla: string;
  slaName: string;
  ticketType: string;
  ticketTypeName: string;
  helpTopic: string;
  helpTopicName: string;
  source: string;
  priority: string;
  dueAt: string;
  ticketComments: ITicketComment[];
}

export interface IDashboardCount {
  myTicket: number;
  myOverDue: number;
  myDueToday: number;
  openTickets: number;
  openTicketsDue: number;
}
