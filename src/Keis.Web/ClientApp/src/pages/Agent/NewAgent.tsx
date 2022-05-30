import {  Breadcrumb } from "antd";
import { FC, useEffect, useState } from "react";
import { Link, Outlet, useNavigate } from "react-router-dom";
import { PageView } from "../../components";
import { useApp, useAuth } from "../../context";
import {
  IFetchDepartment,
  IFetchTeam,
  IFetchType,
  IListResult,
  ISingleResult,
} from "../../interfaces";
import AgentForm from "./AgentForm";

interface INew {}

interface IFormData {
  firstname: string;
  lastname: string;
  email: string;
  mobile: string;
  telephone: string;
  timezone: string;
  department: string;
  team: string;
  type: string;
  mode: string;
  status: string;
  note: string;
}

const New: FC<INew> = () => {
  const { user } = useAuth();
  const { get } = useApp();
  const navigate = useNavigate();
  const [departmentList, setDepartmentList] = useState<IFetchDepartment[]>([]);
  const [teamList, setTeamList] = useState<IFetchTeam[]>([]);
  const [typeList, setTypeList] = useState<IFetchType[]>([]);
  const [hasFormErrors, setHasFormErrors] = useState(false);
  const [formErrors, setFormErrors] = useState<string[]>([]);

  useEffect(() => {
    const loadDepartment = async () => {
      const g: Response = await get(`methods/department.list`);

      if (g != null) {
        const d: IListResult<IFetchDepartment[]> = await g.json();

        if (g.status === 200 && d.ok) {
          setDepartmentList(d.data);
        } else {
          // TODO: display error to user
        }
      }
    };

    const loadTeam = async () => {
      const g: Response = await get(`methods/team.list`);

      if (g != null) {
        const d: IListResult<IFetchTeam[]> = await g.json();

        if (g.status === 200 && d.ok) {
          setTeamList(d.data);
        } else {
          // TODO: display error to user
        }
      }
    };

    const loadType = async () => {
      const g: Response = await get(`methods/tickettype.list`);

      if (g != null) {
        const d: IListResult<IFetchType[]> = await g.json();

        if (g.status === 200 && d.ok) {
          setTypeList(d.data);
        } else {
          // TODO: display error to user
        }
      }
    };

    if (user != null) {
      loadDepartment();
      loadTeam();
      loadType();
    }
  }, [user, get]);

  const onFinish = async (values: IFormData) => {
    const body = new FormData();
    body.append("firstname", values.firstname ?? "");
    body.append("lastname", values.lastname ?? "");
    body.append("email", values.email ?? "");
    body.append("mobile", values.mobile ?? "");
    body.append("telephone", values.telephone ?? "");
    body.append("timezone", values.timezone ?? "");
    body.append("department", values.department ?? "");
    body.append("team", values.team ?? "");
    body.append("tickettype", values.type ?? "");
    body.append("mode", values.mode ?? "");
    body.append("status", values.status ?? "");
    body.append("note", values.note ?? "");

    const headers = new Headers();
    headers.append("Authorization", `Bearer ${user?.token ?? ""}`);

    const f = await fetch("methods/agent.create", {
      method: "POST",
      body,
      headers,
    });

    const result: ISingleResult<string> = await f.json();

    if (f.status === 201 && result.ok) {
      navigate(`/admin/agent/${result.data}`, { replace: true });
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
        <Link to="/admin/agent">Agents</Link>
      </Breadcrumb.Item>
      <Breadcrumb.Item>New Agent</Breadcrumb.Item>
    </Breadcrumb>
  );

  return (
    <PageView title="New Agent" breadcrumbs={<Breadcrumbs />}>
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-3">
        <div>
          <AgentForm
            hasFormErrors={hasFormErrors}
            formErrors={formErrors}
            departmentList={departmentList}
            teamList={teamList}
            typeList={typeList}
            onFinish={onFinish}
          />
        </div>

        <div></div>
      </div>

      <Outlet />
    </PageView>
  );
};

export default New;
