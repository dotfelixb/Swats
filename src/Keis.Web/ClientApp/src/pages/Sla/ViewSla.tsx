import { Breadcrumb, Button, Checkbox, Drawer } from "antd";
import dayjs from "dayjs";
import React, { FC, useCallback, useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import { PageView } from "../../components";
import { useApp, useAuth } from "../../context";
import { IFetchBusinessHour, IFetchSla, IListResult, ISingleResult } from "../../interfaces";
import SlaForm from "./SlaForm";

interface IViewSla {}

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

const ViewSla: FC<IViewSla> = () => {
  const { user } = useAuth();
  const { get, dateFormats } = useApp();
  const { id } = useParams();
  const [sla, setSla] = useState<IFetchSla>();
  const [hourList, setHourList] = useState<IFetchBusinessHour[]>([]);
  const [showEditForm, setEditForm] = useState(false);
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
  
  const load = useCallback(async () => {
    const g: Response = await get(`methods/sla.get?id=${id}`);
    const d: ISingleResult<IFetchSla> = await g.json();

    if (g.status === 200 && d.ok) {
      setSla(d.data);
    } else {
      // TODO: display error to user
    }
  }, [get, id]);

  useEffect(() => {

    if (user != null && user.token && id) {
      load();
      loadHour();
    }
  }, [user, id, get, load, loadHour]);

  const onFinish = async (values: IFormData) => {
    const body = new FormData();
    body.append("id", sla?.id ?? "");
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

    const f = await fetch("methods/sla.update", {
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
        <Link to="/admin/sla">SLA</Link>
      </Breadcrumb.Item>
      <Breadcrumb.Item>{sla?.name ?? "View Sla"}</Breadcrumb.Item>
    </Breadcrumb>
  );

  return (
    <PageView
      title={sla?.name ?? "View Sla"}
      buttons={<Buttons />}
      breadcrumbs={<Breadcrumbs />}
    >
      <div className="w-full flex flex-row ">
        <div style={{ width: "220px" }} className="">
          <div className="pr-2">
            <div className="bg-gray-200 rounded-sm w-28 h-28"></div>
          </div>
          <ul className="edit-sidebar py-5">
            <li>
              <div>Created By</div>
              <div>{sla?.createdByName ?? ""}</div>
            </li>
            <li>
              <div>Created At</div>
              <div>
                {dayjs(sla?.createdAt ?? new Date()).format(
                  dateFormats.shortDateFormat
                )}
              </div>
            </li>
            <li>
              <div>Updated By</div>
              <div>{sla?.updatedByName ?? ""}</div>
            </li>
            <li>
              <div>Updated At</div>
              <div>
                {dayjs(sla?.updatedAt ?? new Date()).format(
                  dateFormats.shortDateFormat
                )}
              </div>
            </li>
          </ul>
        </div>
        <div className="w-full bg-white border border-gray-200 rounded-sm px-10 py-10">
          <div className="grid grid-cols-1 md:grid-cols-2 gap-3">
            <div>
              <label className="form-label">Status</label>
              <div className="form-data">{sla?.status}</div>
            </div>
          </div>

          <div className="grid grid-cols-1 md:grid-cols-4 gap-3 pt-2">
            <div>
              <label className="form-label">Response Period</label>
              <div className="form-data">{sla?.responsePeriod}</div>
            </div>

            <div>
              <label className="form-label">Response Format</label>
              <div className="form-data">{sla?.responseFormat}</div>
            </div>

            <div>
              <label className="form-label">Inapp Notify</label>
              <div className="form-data">
                <Checkbox checked={sla?.responseNotify} />
              </div>
            </div>

            <div>
              <label className="form-label">Escalate Email</label>
              <div className="form-data">
                <Checkbox checked={sla?.responseEmail} />
              </div>
            </div>
          </div>

          <div className="grid grid-cols-1 md:grid-cols-4 gap-3">
            <div>
              <label className="form-label">Resolve Period</label>
              <div className="form-data">{sla?.resolvePeriod}</div>
            </div>

            <div>
              <label className="form-label">Resolve Format</label>
              <div className="form-data">{sla?.resolveFormat}</div>
            </div>

            <div>
              <label className="form-label">Inapp Notify</label>
              <div className="form-data">
                <Checkbox checked={sla?.resolveNotify} />
              </div>
            </div>

            <div>
              <label className="form-label">Escalate Email</label>
              <div className="form-data">
                <Checkbox checked={sla?.resolveEmail} />
              </div>
            </div>
          </div>

          <div className="grid grid-cols-1 gap-5 py-4">
            <div>
              <label className="form-label">Note</label>
              <div className="form-data">{sla?.note ?? "N/A"}</div>
            </div>
          </div>
        </div>
      </div>

      <Drawer
        visible={showEditForm}
        title="Editing Sla"
        placement="right"
        width={640}
        destroyOnClose={true}
        onClose={() => setEditForm(false)}
          >
              <SlaForm hasFormErrors={hasFormErrors}
                  formErrors={formErrors}
                  sla={sla}
                  hourList={hourList}
                  onFinish={onFinish} />
      </Drawer>
    </PageView>
  );
};

export default ViewSla;
