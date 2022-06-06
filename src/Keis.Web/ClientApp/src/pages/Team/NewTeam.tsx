import { Breadcrumb } from "antd";
import { FC, useCallback, useEffect, useState } from "react";
import { Link, Outlet, useNavigate } from "react-router-dom";
import { PageView } from "../../components";
import { useApp, useAuth } from "../../context";
import { IFetchAgent, IFetchDepartment, IListResult, ISingleResult } from "../../interfaces";
import TeamForm from "./TeamForm";

interface INewTeam {}

interface IFormData {
  name: string;
  department: string;
  manager: string;
  status: string;
  response: string;
}

const NewTeam: FC<INewTeam> = () => {
  const { user } = useAuth();
  const { get } = useApp();
  const navigate = useNavigate();
  const [departmentList, setDepartmentList] = useState<IFetchDepartment[]>([]);
  const [agentList, setAgentList] = useState<IFetchAgent[]>([]);
  const [hasFormErrors, setHasFormErrors] = useState(false);
  const [formErrors, setFormErrors] = useState<string[]>([]);

  const loadDepartment = useCallback(async () => {
    const g: Response = await get(`methods/department.list`);

    if (g != null) {
      const d: IListResult<IFetchDepartment[]> = await g.json();

      if (g.status === 200 && d.ok) {
        setDepartmentList(d.data);
      } else {
        // TODO: display error to user
      }
    }
  }, [get]);

  const loadAgent = useCallback(async () => {
    const g: Response = await get(`methods/agent.list`);

    if (g != null) {
      const d: IListResult<IFetchAgent[]> = await g.json();

      if (g.status === 200 && d.ok) {
        setAgentList(d.data);
      } else {
        // TODO: display error to user
      }
    }
  }, [get]);

  useEffect(() => {
    if (user != null) {
      loadDepartment();
      loadAgent();
    }
  }, [user, get, loadAgent, loadDepartment]);

  const onFinish = async ({
    name,
    department,
    status,
    manager,
    response,
  }: IFormData) => {
    const body = new FormData();
    body.append("name", name ?? "");
    body.append("department", department ?? "");
    body.append("manager", manager ?? "");
    body.append("status", status ?? 1);
    body.append("response", response ?? "");

    const headers = new Headers();
    headers.append("Authorization", `Bearer ${user?.token ?? ""}`);

    const f = await fetch("methods/team.create", {
      method: "POST",
      body,
      headers,
    });

    const result: ISingleResult<string> = await f.json();

    if (f.status === 201 && result.ok) {
      navigate(`/admin/team/${result.data}`, { replace: true });
    }else{
      setHasFormErrors(true);
      setFormErrors(result?.errors);
    }
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
        <Link to="/admin/team">Teams</Link>
      </Breadcrumb.Item>
      <Breadcrumb.Item>New Team</Breadcrumb.Item>
    </Breadcrumb>
  );

  return (
    <PageView title="New Team" breadcrumbs={<Breadcrumbs />}>
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-3">
        <div>
          <TeamForm
            hasFormErrors={hasFormErrors}
            formErrors={formErrors}
            departmentList={departmentList}
            agentList={agentList}
            onFinish={onFinish}
          />
        </div>

        <div></div>
      </div>

      <Outlet />
    </PageView>
  );
};

export default NewTeam;
