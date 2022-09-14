import { Breadcrumb } from "antd";
import React, { FC, useState } from "react";
import { Link, Outlet, useNavigate } from "react-router-dom";
import { PageView } from "../../components";
import { useApp, useAuth } from "../../context";
import { ISingleResult } from "../../interfaces";
import EmailForm from "./EmailForm";

interface IFormData {
  address: string;
  name: string;
  username: string;
  password: string;
  
  inhost: string;
  inprotocol: string;
  inport: string;
  insecurity:string;

  outhost: string;
  outprotocol: string;
  outport: string;
  outsecurity:string;

  status: string;
  note: string;
}

interface INewEmail {}

const NewEmail: FC<INewEmail> = () => {
  const { user } = useAuth();
  const {post} = useApp();
  const navigate = useNavigate();
  const [hasFormErrors, setHasFormErrors] = useState(false);
  const [formErrors, setFormErrors] = useState<string[]>([]);

  const onFinish = async (values: IFormData) => {
    const body = new FormData();
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

    const f = await post("methods/email.create", body );

    const result: ISingleResult<string> = await f.json();

    if (f.status === 201 && result.ok) {
      navigate(`/admin/email/${result.data}`, { replace: true });
    } else {
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
        <Link to="/admin/email">Email Settings</Link>
      </Breadcrumb.Item>
      <Breadcrumb.Item>New Email Settings</Breadcrumb.Item>
    </Breadcrumb>
  );

  return (
    <PageView title="New Email" breadcrumbs={<Breadcrumbs />}>
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-3">
        <div>
          <EmailForm
            hasFormErrors={hasFormErrors}
            formErrors={formErrors}
            onFinish={onFinish}
          />
        </div>

        <div></div>
      </div>

      <Outlet />
    </PageView>
  );
};

export default NewEmail;
