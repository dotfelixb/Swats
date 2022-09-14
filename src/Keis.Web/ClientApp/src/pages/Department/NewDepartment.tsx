import { Breadcrumb } from "antd";
import { FC, useEffect, useState } from "react";
import { Link, Outlet, useNavigate } from "react-router-dom";
import { PageView } from "../../components";
import { useApp, useAuth } from "../../context";
import {
  IFetchAgent,
  IFetchBusinessHour,
  IFetchDepartment,
  IListResult,
  ISingleResult,
} from "../../interfaces";
import DepartmentForm from "./DepartmentForm";

interface INewDepartment {}

interface IFormData {
  name: string;
  type: string;
  manager: string;
  businessHour: string;
  outgoingEmail: string;
  status: string;
  response: string;
}

const NewDepartment: FC<INewDepartment> = () => {
  const { user } = useAuth();
  const { get, post } = useApp();
  const navigate = useNavigate();
  const [agentList, setAgentList] = useState<IFetchAgent[]>([]);
  const [hourList, setHourList] = useState<IFetchBusinessHour[]>([]);
  const [hasFormErrors, setHasFormErrors] = useState(false);
  const [formErrors, setFormErrors] = useState<string[]>([]);

  useEffect(() => {
    const loadAgent = async () => {
      const g: Response = await get(`methods/agent.list`);

      if (g != null) {
        const d: IListResult<IFetchAgent[]> = await g.json();

        if (g.status === 200 && d.ok) {
          setAgentList(d.data);
        } else {
          // TODO: display error to user
        }
      }
    };

    const loadHour = async () => {
      const g: Response = await get(`methods/businesshour.list`);

      if (g != null) {
        const d: IListResult<IFetchBusinessHour[]> = await g.json();

        if (g.status === 200 && d.ok) {
          setHourList(d.data);
        } else {
          // TODO: display error to user
        }
      }
    };

    if (user != null) {
      loadAgent();
      loadHour();
    }
  }, [user, get]);

  const onFinish = async ({
    name,
    type,
    manager,
    businessHour,
    outgoingEmail,
    status,
    response,
  }: IFormData) => {
    const body = new FormData();
    body.append("name", name ?? "");
    body.append("type", type ?? "");
    body.append("manager", manager ?? "");
    body.append("businessHour", businessHour ?? "");
    body.append("outgoingEmail", outgoingEmail ?? "");
    body.append("status", status ?? 1);
    body.append("response", response ?? "");

    const headers = new Headers();
    headers.append("Authorization", `Bearer ${user?.token ?? ""}`);

    const f = await post("methods/department.create",  body );

    const result: ISingleResult<string> = await f.json();

    if (f.status === 201 && result.ok) {
      navigate(`/admin/department/${result.data}`, { replace: true });
    }

    setHasFormErrors(true);
    setFormErrors(result?.errors);
  };

  const Breadcrumbs: FC = () => (
    <Breadcrumb separator="/">
      <Breadcrumb.Item>
        <Link to="/">Home</Link>
      </Breadcrumb.Item>
      <Breadcrumb.Item>
        <Link to="/admin">Admin</Link>
      </Breadcrumb.Item>
      <Breadcrumb.Item>
        <Link to="/admin/department">Departments</Link>
      </Breadcrumb.Item>
      <Breadcrumb.Item>New Team</Breadcrumb.Item>
    </Breadcrumb>
  );

  return (
    <PageView title="New Department" breadcrumbs={<Breadcrumbs />}>
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-3">
        <div>
          <DepartmentForm
            hasFormErrors={hasFormErrors}
            formErrors={formErrors}
            agentList={agentList}
            hourList={hourList}
            onFinish={onFinish}
          />
        </div>

        <div></div>
      </div>

      <Outlet />
    </PageView>
  );
};

export default NewDepartment;
