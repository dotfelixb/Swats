import { Breadcrumb, Button, Drawer } from "antd";
import dayjs from "dayjs";
import React, { FC, useCallback, useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
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

interface IViewDepartment {}

interface IFormData {
  name: string;
  type: string;
  manager: string;
  businessHour: string;
  outgoingEmail: string;
  status: string;
  response: string;
}

const ViewDepartment: FC<IViewDepartment> = () => {
  const { user } = useAuth();
  const { get, dateFormats } = useApp();
  const { id } = useParams();
  const [department, setDepartment] = useState<IFetchDepartment>();
  const [agentList, setAgentList] = useState<IFetchAgent[]>([]);
  const [hourList, setHourList] = useState<IFetchBusinessHour[]>([]);
  const [showEditForm, setEditForm] = useState(false);
  const [hasFormErrors, setHasFormErrors] = useState(false);
  const [formErrors, setFormErrors] = useState<string[]>([]);

  const load = useCallback(async () => {
    const g: Response = await get(`methods/department.get?id=${id}`);
    const d: ISingleResult<IFetchDepartment> = await g.json();

    if (g != null) {
      if (g.status === 200 && d.ok) {
        setDepartment(d.data);
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

  const loadHour = useCallback(async () => {
    const g: Response = await get(`methods/businesshour.list`);

    if (g != null) {
      const d: IListResult<IFetchBusinessHour[]> = await g.json();

      if (g.status === 200 && d.ok) {
        setHourList(d.data);
      } else {
        // TODO: display error to user
      }
    }
  }, [get]);

  useEffect(() => {
    if (user != null && user.token && id) {
      load();
      loadAgent();
      loadHour();
    }
  }, [user, id, get]);

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
    body.append("id", department?.id ?? "");
    body.append("name", name ?? "");
    body.append("type", type ?? "");
    body.append("manager", manager ?? "");
    body.append("businessHour", businessHour ?? "");
    body.append("outgoingEmail", outgoingEmail ?? "");
    body.append("status", status ?? 1);
    body.append("response", response ?? "");

    const headers = new Headers();
    headers.append("Authorization", `Bearer ${user?.token ?? ""}`);

    const f = await fetch("methods/department.update", {
      method: "PATCH",
      body,
      headers,
    });

    const result: ISingleResult<string> = await f.json();

    if (f.status === 201 && result.ok) {
      load();
      setEditForm(false);
    } else {
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
        <Link to="/admin/department">Departments</Link>
      </Breadcrumb.Item>
      <Breadcrumb.Item>{department?.name ?? ""}</Breadcrumb.Item>
    </Breadcrumb>
  );

  return (
    <PageView
      title={department?.name ?? ""}
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
              <div>{department?.createdByName ?? ""}</div>
            </li>
            <li>
              <div>Created At</div>
              <div>
                {dayjs(department?.createdAt ?? new Date()).format(
                  dateFormats.shortDateFormat
                )}
              </div>
            </li>
            <li>
              <div>Updated By</div>
              <div>{department?.updatedByName ?? ""}</div>
            </li>
            <li>
              <div>Updated At</div>
              <div>
                {dayjs(department?.updatedAt ?? new Date()).format(
                  dateFormats.shortDateFormat
                )}
              </div>
            </li>
          </ul>
        </div>

        <div className="w-full bg-white border border-gray-200 rounded-sm px-10 py-10">
          <div className="grid grid-cols-1 md:grid-cols-2 gap-3">
            <div>
              <label className="form-label">Name</label>
              <div className="form-data">{department?.name ?? "N/A"}</div>
            </div>
            <div>
              <label className="form-label">Type</label>
              <div className="form-data">{department?.type ?? "N/A"}</div>
            </div>
            <div>
              <label className="form-label">Manager</label>
              <div className="form-data">
                {department?.managerName ?? "N/A"}
              </div>
            </div>
            <div>
              <label className="form-label">Business Hour</label>
              <div className="form-data">
                {department?.businessHourName ?? "N/A"}
              </div>
            </div>
            <div>
              <label className="form-label">Status</label>
              <div className="form-data">{department?.status ?? "N/A"}</div>
            </div>
            <div>
              <label className="form-label">Outgoing Email</label>
              <div className="form-data">
                {department?.outgoingEmail ?? "N/A"}
              </div>
            </div>
          </div>

          <div className="grid grid-cols-1 gap-5 py-4">
            <div>
              <label className="form-label">Default Response</label>
              <div className="form-data">{department?.response ?? "N/A"}</div>
            </div>
          </div>
        </div>
      </div>

      <Drawer
        visible={showEditForm}
        title="Editing Department"
        placement="right"
        width={640}
        destroyOnClose={true}
        onClose={() => setEditForm(false)}
      >
        <DepartmentForm
          hasFormErrors={hasFormErrors}
          formErrors={formErrors}
          department={department}
          agentList={agentList}
          hourList={hourList}
          onFinish={onFinish}
        />
      </Drawer>
    </PageView>
  );
};

export default ViewDepartment;
