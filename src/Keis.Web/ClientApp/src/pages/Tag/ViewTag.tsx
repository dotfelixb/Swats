import { Breadcrumb, Button, Drawer } from "antd";
import dayjs from "dayjs";
import React, { FC, useCallback, useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import { PageView } from "../../components";
import { useApp, useAuth } from "../../context";
import { IFetchTag, ISingleResult } from "../../interfaces";
import TagForm from "./TagForm";

interface IViewTag {}

interface IFormData {
  name: string;
  color: string;
  status: string;
  visibility: string;
  note: string;
}

const ViewTag: FC<IViewTag> = () => {
  const { user } = useAuth();
  const { get, dateFormats } = useApp();
  const { id } = useParams();
  const [tag, setTag] = useState<IFetchTag>();
  const [showEditForm, setEditForm] = useState(false);
  const [hasFormErrors, setHasFormErrors] = useState(false);
  const [formErrors, setFormErrors] = useState<string[]>([]);

  const loadTag = useCallback(async () => {
    const g: Response = await get(`methods/tag.get?id=${id}`);
    const d: ISingleResult<IFetchTag> = await g.json();

    if (g != null) {
      if (g.status === 200 && d.ok) {
        setTag(d.data);
      } else {
        // TODO: display error to user
      }
    }
  }, [id, get]);

  const onFinish = async ({
    name,
    color,
    status,
    visibility,
    note,
  }: IFormData) => {
    const body = new FormData();
    body.append("id", tag?.id ?? "");
    body.append("name", name ?? "");
    body.append("color", color ?? "");
    body.append("status", status ?? "");
    body.append("visibility", visibility ?? "");
    body.append("note", note ?? "");

    const headers = new Headers();
    headers.append("Authorization", `Bearer ${user?.token ?? ""}`);

    const f = await fetch("methods/tag.update", {
      method: "PATCH",
      body,
      headers,
    });

    const result: ISingleResult<string> = await f.json();

    if (f.status === 201 && result.ok) {
      loadTag();
      setEditForm(false);
    }

    setHasFormErrors(true);
    setFormErrors(result?.errors);
  };

  useEffect(() => {
    if (user != null && user.token && id) {
      loadTag();
    }
  }, [user, id, loadTag]);

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
        <Link to="/admin/tag">Tags</Link>
      </Breadcrumb.Item>
      <Breadcrumb.Item>{tag?.name ?? ""}</Breadcrumb.Item>
    </Breadcrumb>
  );

  return (
    <PageView
      title={tag?.name ?? ""}
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
              <div>{tag?.createdByName ?? ""}</div>
            </li>
            <li>
              <div>Created At</div>
              <div>
                {dayjs(tag?.createdAt ?? new Date()).format(
                  dateFormats.shortDateFormat
                )}
              </div>
            </li>
            <li>
              <div>Updated By</div>
              <div>{tag?.updatedByName ?? ""}</div>
            </li>
            <li>
              <div>Updated At</div>
              <div>
                {dayjs(tag?.updatedAt ?? new Date()).format(
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
              <div className="form-data">{tag?.status}</div>
            </div>
            <div>
              <label className="form-label">Visibility</label>
              <div className="form-data">{tag?.visibility ?? "N/A"}</div>
            </div>
            <div>
              <label className="form-label">Color</label>
              <div className="form-data">{tag?.color ?? "N/A"}</div>
            </div>
          </div>

          <div className="grid grid-cols-1 gap-5 py-4">
            <div>
              <label className="form-label">Description</label>
              <div className="form-data">{tag?.note ?? "N/A"}</div>
            </div>
          </div>
        </div>
      </div>

      <Drawer
        visible={showEditForm}
        title="Editing Tag"
        placement="right"
        width={640}
        destroyOnClose={true}
        onClose={() => setEditForm(false)}
      >
        <TagForm
          hasFormErrors={hasFormErrors}
          formErrors={formErrors}
          tag={tag}
          onFinish={onFinish}
        />
      </Drawer>
    </PageView>
  );
};

export default ViewTag;
