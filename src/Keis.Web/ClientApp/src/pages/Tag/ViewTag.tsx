import { Breadcrumb } from "antd";
import dayjs from "dayjs";
import React, { FC, useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import { PageView } from "../../components";
import { useApp, useAuth } from "../../context";
import { IFetchTag, ISingleResult } from "../../interfaces";

interface IViewTag { }

const ViewTag: FC<IViewTag> = () => {
  const { user } = useAuth();
  const { get, dateFormats } = useApp();
  const { id } = useParams();
  const [tag, setTag] = useState<IFetchTag>();

  useEffect(() => {
    const load = async () => {
      const g: Response = await get(`methods/tag.get?id=${id}`);
      const d: ISingleResult<IFetchTag> = await g.json();

      if (g != null) {
        if (g.status === 200 && d.ok) {
          setTag(d.data);
        } else {
          // TODO: display error to user
        }
      }
    };

    if (user != null && user.token && id) {
      load();
    }
  }, [user, id, get]);

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
    <PageView title={tag?.name ?? ""} breadcrumbs={<Breadcrumbs />}>
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
    </PageView>
  );
};

export default ViewTag;
