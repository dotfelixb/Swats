import { Breadcrumb, Checkbox } from "antd";
import dayjs from "dayjs";
import { FC, useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import { PageView } from "../../components";
import { useApp, useAuth } from "../../context";
import { IFetchBusinessHour, ISingleResult } from "../../interfaces";

interface IViewHour {}

const ViewHour: FC<IViewHour> = () => {
  const { user } = useAuth();
  const { get, dateFormats } = useApp();
  const { id } = useParams();
  const [hour, setHour] = useState<IFetchBusinessHour>();

  useEffect(() => {
    const load = async () => {
      const g: Response = await get(`methods/businesshour.get?id=${id}`);
      const d: ISingleResult<IFetchBusinessHour> = await g.json();

      if (g.status === 200 && d.ok) {
        setHour(d.data);
      } else {
        // TODO: display error to user
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
        <Link to="/admin/businesshour">Business Hours</Link>
      </Breadcrumb.Item>
      <Breadcrumb.Item>{hour?.name ?? ""}</Breadcrumb.Item>
    </Breadcrumb>
  );

  return (
    <PageView title={hour?.name ?? ""} breadcrumbs={<Breadcrumbs />}>
      <div className="w-full flex flex-row ">
        <div style={{ width: "220px" }} className="">
          <div className="pr-2">
            <div className="bg-gray-200 rounded-sm w-28 h-28"></div>
          </div>
          <ul className="edit-sidebar py-5">
            <li>
              <div>Created By</div>
              <div>{hour?.createdByName ?? ""}</div>
            </li>
            <li>
              <div>Created At</div>
              <div>
                {dayjs(hour?.createdAt ?? new Date()).format(
                  dateFormats.shortDateFormat
                )}
              </div>
            </li>
            <li>
              <div>Updated By</div>
              <div>{hour?.updatedByName ?? ""}</div>
            </li>
            <li>
              <div>Updated At</div>
              <div>
                {dayjs(hour?.updatedAt ?? new Date()).format(
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
              <div className="form-data">{hour?.status}</div>
            </div>
            <div>
              <label className="form-label">Timezone</label>
              <div className="form-data">{hour?.timezone ?? "N/A"}</div>
            </div>
          </div>

          <div className="grid grid-cols-1 gap-5 py-4">
            <div>
              <label className="form-label">Open hours</label>
              {hour?.openHours.map((h) => {
                return (
                  <div
                    key={h.id}
                    className="grid grid-cols-4 gap-5 py-4 text-xs"
                  >
                    <div>
                      <div>
                        <Checkbox checked={h.enabled}>
                          <span className="text-sm">{h.name}</span>
                        </Checkbox>
                      </div>
                    </div>
                    <div>
                      <div>
                        <Checkbox checked={h.fullDay}>
                          <span className="text-sm">Open 24 Hours</span>
                        </Checkbox>
                      </div>
                    </div>
                    <div>
                      <div className="text-sm">
                        From: {dayjs(h.fromTime).format("HH:mm")}
                      </div>
                    </div>
                    <div>
                      <div className="text-sm">
                        To: {dayjs(h.toTime).format("HH:mm")}
                      </div>
                    </div>
                  </div>
                );
              })}
            </div>
          </div>

          <div className="grid grid-cols-1 gap-5 py-4">
            <div>
              <label className="form-label">Description</label>
              <div className="form-data">{hour?.description ?? "N/A"}</div>
            </div>
          </div>
        </div>
      </div>
    </PageView>
  );
};

export default ViewHour;
