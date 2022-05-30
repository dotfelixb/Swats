import { Breadcrumb } from "antd";
import { FC, useState } from "react";
import { Link, Outlet, useNavigate } from "react-router-dom";
import { PageView } from "../../components";
import { useAuth } from "../../context";
import { ISingleResult } from "../../interfaces";
import TagForm from "./TagForm";

interface IFormData {
  name: string;
  color: string;
  status: string;
  visibility: string;
  note: string;
}

interface INewTag {}

const NewTag: FC<INewTag> = () => {
  const { user } = useAuth();
  const navigate = useNavigate();
  const [hasFormErrors, setHasFormErrors] = useState(false);
  const [formErrors, setFormErrors] = useState<string[]>([]);

  const onFinish = async ({
    name,
    color,
    status,
    visibility,
    note,
  }: IFormData) => {
    const body = new FormData();
    body.append("name", name ?? "");
    body.append("color", color ?? "");
    body.append("status", status ?? "");
    body.append("visibility", visibility ?? "");
    body.append("note", note ?? "");

    const headers = new Headers();
    headers.append("Authorization", `Bearer ${user?.token ?? ""}`);

    const f = await fetch("methods/tag.create", {
      method: "POST",
      body,
      headers,
    });

    const result: ISingleResult<string> = await f.json();

    if (f.status === 201 && result.ok) {
      navigate(`/admin/tag/${result.data}`, { replace: true });
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
        <Link to="/admin/tag">Tags</Link>
      </Breadcrumb.Item>
      <Breadcrumb.Item>New Tag</Breadcrumb.Item>
    </Breadcrumb>
  );

  return (
    <PageView title="New Tag" breadcrumbs={<Breadcrumbs />} >
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-3">
        <div>
          <TagForm
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

export default NewTag;
