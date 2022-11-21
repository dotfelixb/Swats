import { Breadcrumb, Button, Drawer } from "antd";
import dayjs from "dayjs";
import React, { FC, useCallback, useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import { PageView } from "../../components";
import { useApp, useAuth } from "../../context";
import { IFetchEmail, ISingleResult } from "../../interfaces";
import EmailForm from "./EmailForm";

interface IViewEmail {}

interface IFormData {
  address: string;
  name: string;
  username: string;
  password: string;

  inhost: string;
  inprotocol: string;
  inport: string;
  insecurity: string;

  outhost: string;
  outprotocol: string;
  outport: string;
  outsecurity: string;

  status: string;
  note: string;
}

const ViewEmail: FC<IViewEmail> = () => {
  const { user } = useAuth();
  const { get, patch, dateFormats } = useApp();
  const { id } = useParams();
  const [email, setEmail] = useState<IFetchEmail>();
  const [showEditForm, setEditForm] = useState(false);
  const [hasFormErrors, setHasFormErrors] = useState(false);
  const [formErrors, setFormErrors] = useState<string[]>([]);

  const loadEmail = useCallback(async () => {
    const g: Response = await get(`methods/email.get?id=${id}`);
    const d: ISingleResult<IFetchEmail> = await g.json();

    if (g != null) {
      if (g.status === 200 && d.ok) {
        setEmail(d.data);
      } else {
        // TODO: display error to user
      }
    }
  }, [id, get]);

  useEffect(() => {
    if (user != null && user.token && id) {
      loadEmail();
    }
  }, [user, id, loadEmail]);

  const onFinish = async (values: IFormData) => {
    const body = new FormData();
    body.append("id", email?.id ?? "");
    body.append("address", values.address ?? "");
    body.append("name", values.name ?? "");
    body.append("username", values.username ?? "");
    body.append("password", values.password ?? "");
    body.append("inhost", values.inhost ?? "");
    body.append("inprotocol", values.inprotocol ?? "");
    body.append("inport", values.inport ?? "");
    body.append("insecurity", values.insecurity ?? "");
    body.append("outhost", values.outhost ?? "");
    body.append("outprotocol", values.outprotocol ?? "");
    body.append("outport", values.outport ?? "");
    body.append("outsecurity", values.outsecurity ?? "");
    body.append("status", values.status ?? "");
    body.append("note", values.note ?? "");

    const headers = new Headers();
    headers.append("Authorization", `Bearer ${user?.token ?? ""}`);

    const f = await patch("methods/email.update", body);

    const result: ISingleResult<string> = await f.json();

    if (f.status === 201 && result.ok) {
      loadEmail();
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
        <Link to="/admin/email">Email</Link>
      </Breadcrumb.Item>
      <Breadcrumb.Item>{email?.address ?? ""}</Breadcrumb.Item>
    </Breadcrumb>
  );

  return (
    <PageView
      title={email?.address ?? ""}
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
              <div>{email?.createdByName ?? ""}</div>
            </li>
            <li>
              <div>Created At</div>
              <div>
                {dayjs(email?.createdAt ?? new Date()).format(
                  dateFormats.shortDateFormat
                )}
              </div>
            </li>
            <li>
              <div>Updated By</div>
              <div>{email?.updatedByName ?? ""}</div>
            </li>
            <li>
              <div>Updated At</div>
              <div>
                {dayjs(email?.updatedAt ?? new Date()).format(
                  dateFormats.shortDateFormat
                )}
              </div>
            </li>
          </ul>
        </div>

        <div className="w-full bg-white border border-gray-200 rounded-sm px-10 py-10">
          <div className="grid grid-cols-1 md:grid-cols-2 gap-3">
            <div>
              <label className="form-label">Display</label>
              <div className="form-data">{email?.name}</div>
            </div>
            <div>
              <label className="form-label">Username</label>
              <div className="form-data">{email?.username}</div>
            </div>
            <div>
              <label className="form-label">Incoming Host Name</label>
              <div className="form-data">{email?.inHost}</div>
            </div>

            <div>
              <label className="form-label">Incoming Protocol</label>
              <div className="form-data">{email?.inProtocol}</div>
            </div>
            <div>
              <label className="form-label">Incoming Port</label>
              <div className="form-data">{email?.inPort}</div>
            </div>
            <div>
              <label className="form-label">Encryption</label>
              <div className="form-data">{email?.inSecurity}</div>
            </div>

            <div>
              <label className="form-label">Outgoing Host Name</label>
              <div className="form-data">{email?.outHost}</div>
            </div>

            <div>
              <label className="form-label">Outgoing Protocol</label>
              <div className="form-data">{email?.outProtocol}</div>
            </div>
            <div>
              <label className="form-label">Outgoing Port</label>
              <div className="form-data">{email?.outPort}</div>
            </div>
            <div>
              <label className="form-label">Encryption</label>
              <div className="form-data">{email?.outSecurity}</div>
            </div>
          </div>

          <div className="grid grid-cols-1 gap-5 py-4">
            <div>
              <label className="form-label">Description</label>
              <div className="form-data">{email?.note ?? "N/A"}</div>
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
        <EmailForm
          hasFormErrors={hasFormErrors}
          formErrors={formErrors}
          email={email}
          onFinish={onFinish}
        />
      </Drawer>
    </PageView>
  );
};

export default ViewEmail;
