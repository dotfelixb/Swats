import { Breadcrumb, Button } from "antd";
import dayjs from "dayjs";
import React, { FC, useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import { PageView } from "../../components";
import { useApp, useAuth } from "../../context";
import { IFetchTopic, ISingleResult } from "../../interfaces";

interface IViewTopic {}

const ViewTopic: FC<IViewTopic> = () => {
  const { user } = useAuth();
  const { get, dateFormats } = useApp();
  const { id } = useParams();
  const [topic, setTopic] = useState<IFetchTopic>();
  // const [showEditForm, setEditForm] = useState(false);

  useEffect(() => {
    const load = async () => {
      const g: Response = await get(`methods/helptopic.get?id=${id}`);
      const d: ISingleResult<IFetchTopic> = await g.json();

      if (g.status === 200 && d.ok) {
        setTopic(d.data);
      } else {
        // TODO: display error to user
      }
    };

    if (user != null && user.token && id) {
      load();
    }
  }, [user, id, get]);

  const Buttons: FC = () => (
    <div className="space-x-2">
      <Button type="primary" >
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
        <Link to="/admin/helptopic">Help Topics</Link>
      </Breadcrumb.Item>
      <Breadcrumb.Item>{topic?.name ?? ""}</Breadcrumb.Item>
    </Breadcrumb>
  );

  return (
    <PageView
      title={topic?.name ?? ""}
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
              <div>{topic?.createdByName ?? ""}</div>
            </li>
            <li>
              <div>Created At</div>
              <div>
                {dayjs(topic?.createdAt ?? new Date()).format(
                  dateFormats.shortDateFormat
                )}
              </div>
            </li>
            <li>
              <div>Updated By</div>
              <div>{topic?.updatedByName ?? ""}</div>
            </li>
            <li>
              <div>Updated At</div>
              <div>
                {dayjs(topic?.updatedAt ?? new Date()).format(
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
              <div className="form-data">{topic?.status}</div>
            </div>
            <div>
              <label className="form-label">Type</label>
              <div className="form-data">{topic?.type ?? "N/A"}</div>
            </div>
            <div>
              <label className="form-label">Department</label>
              <div className="form-data">{topic?.departmentName ?? "N/A"}</div>
            </div>
          </div>

          <div className="grid grid-cols-1 gap-5 py-4">
            <div>
              <label className="form-label">Note</label>
              <div className="form-data">{topic?.note ?? "N/A"}</div>
            </div>
          </div>
        </div>
      </div>
    </PageView>
  );
};

export default ViewTopic;
