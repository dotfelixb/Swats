import { Breadcrumb, Button, Drawer } from "antd";
import dayjs from "dayjs";
import React, { FC, useCallback, useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import { PageView } from "../../components";
import { useApp, useAuth } from "../../context";
import {
  IFetchAgent,
  IFetchDepartment,
  IFetchTeam,
  IListResult,
  ISingleResult,
} from "../../interfaces";
import TeamForm from "./TeamForm";

interface IViewTeam {}

interface IFormData {
  name: string;
  department: string;
  manager: string;
  status: string;
  response: string;
}

const ViewTeam: FC<IViewTeam> = () => {
  const { user } = useAuth();
  const { get, patch, dateFormats } = useApp();
  const { id } = useParams();
  const [team, setTeam] = useState<IFetchTeam>();
  const [departmentList, setDepartmentList] = useState<IFetchDepartment[]>([]);
  const [agentList, setAgentList] = useState<IFetchAgent[]>([]);
  const [showEditForm, setEditForm] = useState(false);
  const [hasFormErrors, setHasFormErrors] = useState(false);
  const [formErrors, setFormErrors] = useState<string[]>([]);

  const load = useCallback(async () => {
    const g: Response = await get(`methods/team.get?id=${id}`);
    const d: ISingleResult<IFetchTeam> = await g.json();

    if (g != null) {
      if (g.status === 200 && d.ok) {
        setTeam(d.data);
      } else {
        // TODO: display error to user
      }
    }
  }, [id, get]);

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
    if (user != null && user.token && id) {
      load();
      loadDepartment();
      loadAgent();
    }
  }, [user, id, get, load, loadDepartment, loadAgent]);

  const onFinish = async (values: IFormData) => {
    const body = new FormData();
    body.append("id", team?.id ?? "");
    body.append("name", values.name ?? "");
    body.append("department", values.department ?? "");
    body.append("manager", values.manager ?? "");
    body.append("status", values.status ?? 1);
    body.append("response", values.response ?? "");

    const headers = new Headers();
    headers.append("Authorization", `Bearer ${user?.token ?? ""}`);

    const f = await patch("methods/team.update",   body );

    const result: ISingleResult<string> = await f.json();

    if (f.status === 201 && result.ok) {
      load();
      setEditForm(false)
    }

    setHasFormErrors(true);
    setFormErrors(result?.errors);
  };

  const Buttons: FC = () => (
    <div className="space-x-2">
      <Button type="primary" onClick={() => setEditForm(true)}>
        Edit
      </Button>
    </div>
  );

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
      <Breadcrumb.Item>{team?.name ?? ""}</Breadcrumb.Item>
    </Breadcrumb>
  );

  return (
    <PageView
      title={team?.name ?? ""}
      buttons={<Buttons />}
      breadcrumbs={<Breadcrumbs />}
    >
      <div className="w-full flex flex-row">
        <div style={{ width: "220px" }} className="">
          <div className="pr-2">
            <div className="bg-gray-200 rounded-sm w-28 h-28"></div>
          </div>
          <ul className="edit-sidebar py-5">
            <li>
              <div>Created By</div>
              <div>{team?.createdByName ?? ""}</div>
            </li>
            <li>
              <div>Created At</div>
              <div>
                {dayjs(team?.createdAt ?? new Date()).format(
                  dateFormats.shortDateFormat
                )}
              </div>
            </li>
            <li>
              <div>Updated By</div>
              <div>{team?.updatedByName ?? ""}</div>
            </li>
            <li>
              <div>Updated At</div>
              <div>
                {dayjs(team?.updatedAt ?? new Date()).format(
                  dateFormats.shortDateFormat
                )}
              </div>
            </li>
          </ul>
        </div>

        <div className="w-full bg-white border border-gray-200 rounded-sm px-10 py-10">
          <div className="grid grid-cols-1 md:grid-cols-2 gap-3">
            <div>
              <label className="form-label">Lead</label>
              <div className="form-data">{team?.managerName ?? "N/A"}</div>
            </div>
            <div>
              <label className="form-label">Department</label>
              <div className="form-data">{team?.departmentName ?? "N/A"}</div>
            </div>
            <div>
              <label className="form-label">Status</label>
              <div className="form-data">{team?.status ?? "N/A"}</div>
            </div>
          </div>

          <div className="grid grid-cols-1 gap-5 py-4">
            <div>
              <label className="form-label">Default Response</label>
              <div className="form-data">{team?.response ?? "N/A"}</div>
            </div>
          </div>
        </div>
      </div>

      <Drawer
        visible={showEditForm}
        title="Editing Team"
        placement="right"
        width={640}
        destroyOnClose={true}
        onClose={() => setEditForm(false)}
      >
        <TeamForm
          hasFormErrors={hasFormErrors}
          formErrors={formErrors}
          team={team}
          departmentList={departmentList}
          agentList={agentList}
          onFinish={onFinish}
        />
      </Drawer>
    </PageView>
  );
};

export default ViewTeam;
