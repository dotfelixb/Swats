import { Breadcrumb, Button, Drawer } from "antd";
import dayjs from "dayjs";
import React, { FC, useCallback, useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import { PageView } from "../../components";
import { useApp, useAuth } from "../../context";
import { IFetchTemplate, IListResult, ISingleResult } from "../../interfaces";
import TemplateForm from "./TemplateForm";

interface IViewTemplate {}

interface IFormData {
  name: string;
  status: string;
  subject: string;
  mergetags: string[];
  body: string;
}

const ViewTemplate: FC<IViewTemplate> = () => {
  const { user } = useAuth();
  const { get, patch, dateFormats } = useApp();
  const { id } = useParams();
  const [template, setTemplate] = useState<IFetchTemplate>();
  const [mergeList, setMergeList] = useState<string[]>([]);
  const [showEditForm, setEditForm] = useState(false);
  const [hasFormErrors, setHasFormErrors] = useState(false);
  const [formErrors, setFormErrors] = useState<string[]>([]);

  const loadTemplate = useCallback(async () => {
    const g: Response = await get(`methods/template.get?id=${id}`);
    const d: ISingleResult<IFetchTemplate> = await g.json();

    if (g != null) {
      if (g.status === 200 && d.ok) {
        setTemplate(d.data);
      } else {
        // TODO: display error to user
      }
    }
  }, [id, get]);

  const loadTemplateMergeTags = useCallback(async () => {
    const g: Response = await get(`methods/template.mergetags`);

    if (g != null) {
      const d: IListResult<string[]> = await g.json();

      if (g.status === 200 && d.ok) {
        setMergeList(d.data);
      } else {
        // TODO: display error to user
      }
    }
  }, [get]);

  useEffect(() => {
    if (user != null && user.token && id) {
      loadTemplate();
      loadTemplateMergeTags();
    }
  }, [user, id, loadTemplate, loadTemplateMergeTags]);

  const onFinish = async (values: IFormData) => {
    const body = new FormData();
    body.append("id", template?.id ?? "");
    body.append("name", values.name ?? "");
    body.append("subject", values.subject ?? "");
    body.append("status", values.status ?? "");
    body.append("body", values.body ?? "");

    for (let index = 0; index < values.mergetags.length; index++) {
      body.append(`mergetags[${index}]`, values.mergetags[index]);
    }

    const headers = new Headers();
    headers.append("Authorization", `Bearer ${user?.token ?? ""}`);

    const f = await patch("methods/template.update", body)

    const result: ISingleResult<string> = await f.json();

    if (f.status === 201 && result.ok) {
      loadTemplate();
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
        <Link to="/admin/template">Template</Link>
      </Breadcrumb.Item>
      <Breadcrumb.Item>{template?.name ?? ""}</Breadcrumb.Item>
    </Breadcrumb>
  );

  return (
    <PageView
      title={template?.name ?? ""}
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
              <div>{template?.createdByName ?? ""}</div>
            </li>
            <li>
              <div>Created At</div>
              <div>
                {dayjs(template?.createdAt ?? new Date()).format(
                  dateFormats.shortDateFormat
                )}
              </div>
            </li>
            <li>
              <div>Updated By</div>
              <div>{template?.updatedByName ?? ""}</div>
            </li>
            <li>
              <div>Updated At</div>
              <div>
                {dayjs(template?.updatedAt ?? new Date()).format(
                  dateFormats.shortDateFormat
                )}
              </div>
            </li>
          </ul>
        </div>

        <div className="w-full bg-white border border-gray-200 rounded-sm px-10 py-10">
          <div className="grid grid-cols-1 md:grid-cols-2 gap-3">
            <div>
              <label className="form-label">Subject</label>
              <div className="form-data">{template?.subject ?? "N/A"}</div>
            </div>
            <div>
              <label className="form-label">Merge Tags</label>
              <div className="form-data">{template?.mergeTags.map(t => (<span className="space-x-2" key={t}>{t}</span>))}</div>
            </div>
            <div>
              <label className="form-label">Status</label>
              <div className="form-data">{template?.status}</div>
            </div>
          </div>

          <div className="grid grid-cols-1 gap-5 py-4">
            <div>
              <label className="form-label">Description</label>
              <div className="form-data">{template?.body ?? "N/A"}</div>
            </div>
          </div>
        </div>
      </div>

      <Drawer
        visible={showEditForm}
        title="Editing Template"
        placement="right"
        width={640}
        destroyOnClose={true}
        onClose={() => setEditForm(false)}
      >
        <TemplateForm formErrors={formErrors} hasFormErrors={hasFormErrors} template={template} mergeList={mergeList} onFinish={onFinish}  />
      </Drawer>         

    </PageView>
  );
};

export default ViewTemplate;
