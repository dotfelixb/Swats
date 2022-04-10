import { Breadcrumb } from "antd";
import dayjs from "dayjs";
import React, { FC, useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import { PageView } from "../../components";
import { useApp, useAuth } from "../../context";
import { IFetchTeam, ISingleResult } from "../../interfaces";

interface IViewTeam {}

const ViewTeam: FC<IViewTeam> = () => {
  const { user } = useAuth();
  const { get, dateFormats } = useApp();
  const { id } = useParams();
  const [team, setTeam] = useState<IFetchTeam>();

  useEffect(() => {
    const load = async () => {
      const g: Response = await get(`methods/team.get?id=${id}`);
      const d: ISingleResult<IFetchTeam> = await g.json();

      if (g != null) {
        if (g.status === 200 && d.ok) {
          setTeam(d.data);
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
        <Link to="/admin/team">Teams</Link>
      </Breadcrumb.Item>
      <Breadcrumb.Item>{team?.name ?? ""}</Breadcrumb.Item>
    </Breadcrumb>
  );

  return (
    <PageView title={team?.name ?? ""} breadcrumbs={<Breadcrumbs />}>
      <div className="w-full flex flex-row">
        <div style={{ width: "220px" }} className="">
          <div className="pr-2">
            <div className="bg-gray-200 rounded-sm w-28 h-28"></div>
          </div>
          <ul className="edit-sidebar py-5">
            <li>
              <div>Created By</div>
              <div>{team?.createdByName ?? ""}</div>
            </li>
            <li>
              <div>Created At</div>
              <div>
                {dayjs(team?.createdAt ?? new Date()).format(
                  dateFormats.shortDateFormat
                )}
              </div>
            </li>
            <li>
              <div>Updated By</div>
              <div>{team?.updatedByName ?? ""}</div>
            </li>
            <li>
              <div>Updated At</div>
              <div>
                {dayjs(team?.updatedAt ?? new Date()).format(
                  dateFormats.shortDateFormat
                )}
              </div>
            </li>
          </ul>
        </div>

        <div className="w-full bg-white border border-gray-200 rounded-sm px-10 py-10">
          <div className="grid grid-cols-1 md:grid-cols-2 gap-3">
            <div>
              <label className="form-label">Lead</label>
              <div className="form-data">{team?.leadName ?? "N/A"}</div>
            </div>
            <div>
              <label className="form-label">Department</label>
              <div className="form-data">{team?.departmentName ?? "N/A"}</div>
            </div>
            <div>
              <label className="form-label">Status</label>
              <div className="form-data">{team?.status ?? "N/A"}</div>
            </div>
          </div>

          <div className="grid grid-cols-1 gap-5 py-4">
            <div>
              <label className="form-label">Default Response</label>
              <div className="form-data">{team?.note ?? "N/A"}</div>
            </div>
          </div>
        </div>
      </div>
    </PageView>
  );
};

export default ViewTeam;
