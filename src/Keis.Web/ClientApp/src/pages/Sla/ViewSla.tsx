import { Breadcrumb, Checkbox } from 'antd';
import dayjs from 'dayjs';
import React, { FC, useEffect, useState } from 'react';
import { Link, useParams } from 'react-router-dom';
import { PageView } from '../../components';
import { useApp, useAuth } from '../../context';
import { IFetchSla, ISingleResult } from '../../interfaces';

interface IViewSla { }

const ViewSla: FC<IViewSla> = () => {
  const { user } = useAuth();
  const { get, dateFormats } = useApp();
  const { id } = useParams();
  const [sla, setSla] = useState<IFetchSla>();

  useEffect(() => {
    const load = async () => {
      const g: Response = await get(`methods/sla.get?id=${id}`);
      const d: ISingleResult<IFetchSla> = await g.json();

      if (g.status === 200 && d.ok) {
        setSla(d.data);
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
        <Link to="/admin/sla">SLA</Link>
      </Breadcrumb.Item>
      <Breadcrumb.Item>{sla?.name ?? "View Sla"}
      </Breadcrumb.Item>
    </Breadcrumb>
  );

  return (<PageView title={sla?.name ?? "View Sla"} breadcrumbs={<Breadcrumbs />}>
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
              <div className="form-data"><Checkbox checked={sla?.responseNotify} /></div>
            </div>

            <div>
              <label className="form-label">Escalate Email</label>
              <div className="form-data"><Checkbox checked={sla?.responseEmail} /></div>
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
              <div className="form-data"><Checkbox checked={sla?.resolveNotify} /></div>
            </div>

            <div>
              <label className="form-label">Escalate Email</label>
              <div className="form-data"><Checkbox checked={sla?.resolveEmail} /></div>
            </div>
          </div>

          <div className="grid grid-cols-1 gap-5 py-4">
            <div>
              <label className="form-label">Description</label>
              <div className="form-data">{sla?.description ?? "N/A"}</div>
            </div>
          </div>

        </div>
      </div>
  </PageView>)
}

export default ViewSla;