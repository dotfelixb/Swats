import { Breadcrumb } from "antd";
import { FC, useCallback, useEffect, useState } from "react";
import { Link, Outlet, useNavigate } from "react-router-dom";
import { PageView } from "../../components";
import { useApp, useAuth } from "../../context";
import {
  IFetchBusinessHour,
  IListResult,
  ISingleResult,
} from "../../interfaces";
import SlaForm from "./SlaForm";

interface INewSla { }

interface IFormData {
  name: string;
  hour: string;

  responseperiod: string;
  responseformat: string;
  responsenotify: string;
  responseemail: string;

  resolveperiod: string;
  resolveformat: string;
  resolvenotify: string;
  resolveemail: string;

  status: string;
  note: string;
}

const NewSla: FC<INewSla> = () => {
  const { user } = useAuth();
  const { get, post } = useApp();
  const navigate = useNavigate();
  const [hourList, setHourList] = useState<IFetchBusinessHour[]>([]);
  const [hasFormErrors, setHasFormErrors] = useState(false);
  const [formErrors, setFormErrors] = useState<string[]>([]);

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
    if (user != null) {
      loadHour();
    }
  }, [user, get, loadHour]);

  const onFinish = async (values: IFormData) => {
    const body = new FormData();
    body.append("name", values.name ?? "");
    body.append("businesshour", values.hour ?? "");
    body.append("responseperiod", values.responseperiod ?? "");
    body.append("responseformat", values.responseformat ?? "");
    body.append("responsenotify", values.responsenotify ?? false);
    body.append("responseemail", values.responseemail ?? false);
    body.append("resolveperiod", values.resolveperiod ?? "");
    body.append("resolveformat", values.resolveformat ?? "");
    body.append("resolvenotify", values.resolvenotify ?? false);
    body.append("resolveemail", values.resolveemail ?? false);
    body.append("status", values.status ?? "");
    body.append("note", values.note ?? "");

    const headers = new Headers();
    headers.append("Authorization", `Bearer ${user?.token ?? ""}`);

    const f = await post("methods/sla.create",   body );

    const result: ISingleResult<string> = await f.json();

    if (f.status === 201 && result.ok) {
      navigate(`/admin/sla/${result.data}`, { replace: true });
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
        <Link to="/admin/sla">SLA</Link>
      </Breadcrumb.Item>
      <Breadcrumb.Item>New SLA</Breadcrumb.Item>
    </Breadcrumb>
  );

  return (
    <PageView title="New SLA" breadcrumbs={<Breadcrumbs />}>
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-3">
        <div>
          <SlaForm
            hasFormErrors={hasFormErrors}
            formErrors={formErrors}
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

export default NewSla;
