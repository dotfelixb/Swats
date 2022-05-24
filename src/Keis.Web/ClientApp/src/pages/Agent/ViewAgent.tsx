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
  IFetchType,
  IListResult,
  ISingleResult,
} from "../../interfaces";
import NewForm from "./AgentForm";

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

interface IViewAgent {}

const ViewAgent: FC<IViewAgent> = () => {
  const { user } = useAuth();
  const { get, dateFormats } = useApp();
  const { id } = useParams();
  const [agent, setAgent] = useState<IFetchAgent>();
  const [showEditForm, setEditForm] = useState(false);
  const [departmentList, setDepartmentList] = useState<IFetchDepartment[]>([]);
  const [teamList, setTeamList] = useState<IFetchTeam[]>([]);
  const [typeList, setTypeList] = useState<IFetchType[]>([]);
  const [hasFormErrors, setHasFormErrors] = useState(false);
  const [formErrors, setFormErrors] = useState<string[]>([]);

  const loadAgent = useCallback(async () => {
    const g: Response = await get(`methods/agent.get?id=${id}`);
    const d: ISingleResult<IFetchAgent> = await g.json();

    if (g != null) {
      if (g.status === 200 && d.ok) {
        setAgent(d.data);
      } else {
        // TODO: display error to user
      }
    }
  }, [get]);

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

  const loadTeam = useCallback(async () => {
    const g: Response = await get(`methods/team.list`);

    if (g != null) {
      const d: IListResult<IFetchTeam[]> = await g.json();

      if (g.status === 200 && d.ok) {
        setTeamList(d.data);
      } else {
        // TODO: display error to user
      }
    }
  }, [get]);

  const loadType = useCallback(async () => {
    const g: Response = await get(`methods/tickettype.list`);

    if (g != null) {
      const d: IListResult<IFetchType[]> = await g.json();

      if (g.status === 200 && d.ok) {
        setTypeList(d.data);
      } else {
        // TODO: display error to user
      }
    }
  }, [get]);

  useEffect(() => {
    if (user != null && user.token && id) {
      loadAgent();
      loadDepartment();
      loadTeam();
      loadType();
    }
  }, [user, id, get]);

  const onFinish = async (values: IFormData) => {
    const body = new FormData();
    body.append("id", agent?.id ?? "");
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

    const f = await fetch("methods/agent.update", {
      method: "PATCH",
      body,
      headers,
    });

    const result: ISingleResult<string> = await f.json();

    if (f.status === 201 && result.ok) {
      loadAgent();
      setEditForm(false);
    }else{
      setHasFormErrors(true);
      setFormErrors(result?.errors);
    }
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
        <Link to="/admin/agent">Agents</Link>
      </Breadcrumb.Item>
      <Breadcrumb.Item>
        {`${agent?.firstName ?? ""} ${agent?.lastName ?? ""}`}
      </Breadcrumb.Item>
    </Breadcrumb>
  );

  return (
    <PageView
      title={`${agent?.firstName ?? ""} ${agent?.lastName ?? ""}`}
      breadcrumbs={<Breadcrumbs />}
      buttons={<Buttons />}
    >
      <div className="w-full flex flex-row">
        <div style={{ width: "220px" }} className="">
          <div className="pr-2">
            <div className="bg-gray-200 rounded-sm w-28 h-28"></div>
          </div>
          <ul className="edit-sidebar py-5">
            <li>
              <div>Created By</div>
              <div>{agent?.createdByName ?? ""}</div>
            </li>
            <li>
              <div>Created At</div>
              <div>
                {dayjs(agent?.createdAt ?? new Date()).format(
                  dateFormats.shortDateFormat
                )}
              </div>
            </li>
            <li>
              <div>Updated By</div>
              <div>{agent?.updatedByName ?? ""}</div>
            </li>
            <li>
              <div>Updated At</div>
              <div>
                {dayjs(agent?.updatedAt ?? new Date()).format(
                  dateFormats.shortDateFormat
                )}
              </div>
            </li>
          </ul>
        </div>

        <div className="w-full bg-white border border-gray-200 rounded-sm px-10 py-10">
          <div className="grid grid-cols-1 md:grid-cols-2 gap-3">
            <div>
              <label className="form-label">Email</label>
              <div className="form-data">{agent?.email ?? "N/A"}</div>
            </div>
            <div>
              <label className="form-label">Mobile</label>
              <div className="form-data">{agent?.mobile ?? "N/A"}</div>
            </div>
            <div>
              <label className="form-label">First Name</label>
              <div className="form-data">{agent?.firstName ?? "N/A"}</div>
            </div>
            <div>
              <label className="form-label">Last Name</label>
              <div className="form-data">{agent?.lastName ?? "N/A"}</div>
            </div>
            <div>
              <label className="form-label">Department</label>
              <div className="form-data">{agent?.departmentName ?? "N/A"}</div>
            </div>
            <div>
              <label className="form-label">Team</label>
              <div className="form-data">{agent?.teamName ?? "N/A"}</div>
            </div>
            <div>
              <label className="form-label">Default Ticket Type</label>
              <div className="form-data">{agent?.ticketTypeName ?? "N/A"}</div>
            </div>
            <div>
              <label className="form-label">Timezone</label>
              <div className="form-data">{agent?.timezone ?? "N/A"}</div>
            </div>
            <div>
              <label className="form-label">Status</label>
              <div className="form-data">{agent?.status ?? "N/A"}</div>
            </div>
          </div>

          <div className="grid grid-cols-1 gap-5 py-4">
            <div>
              <label className="form-label">Default Response</label>
              <div className="form-data">{agent?.note ?? "N/A"}</div>
            </div>
          </div>
        </div>
      </div>

      <Drawer
        visible={showEditForm}
        title="Editing Agent"
        placement="right"
        width={640}
        destroyOnClose={true}
        onClose={() => setEditForm(false)}
      >
        <NewForm
          hasFormErrors={hasFormErrors}
          formErrors={formErrors}
          agent={agent}
          departmentList={departmentList}
          teamList={teamList}
          typeList={typeList}
          onFinish={onFinish}
        />
      </Drawer>

    </PageView>
  );
};

export default ViewAgent;
