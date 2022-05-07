import { Breadcrumb } from "antd";
import dayjs from "dayjs";
import React, { FC, useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import { PageView } from "../../components";
import { useApp, useAuth } from "../../context";
import { IFetchDepartment, ISingleResult } from "../../interfaces";

interface IViewDepartment {}

const ViewDepartment: FC<IViewDepartment> = () => {
  const { user } = useAuth();
  const { get, dateFormats } = useApp();
  const { id } = useParams();
  const [department, setDepartment] = useState<IFetchDepartment>();

  useEffect(() => {
    const load = async () => {
      const g: Response = await get(`methods/department.get?id=${id}`);
      const d: ISingleResult<IFetchDepartment> = await g.json();

      if (g != null) {
        if (g.status === 200 && d.ok) {
          setDepartment(d.data);
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
        <Link to="/admin/department">Departments</Link>
      </Breadcrumb.Item>
      <Breadcrumb.Item>{department?.name ?? ""}</Breadcrumb.Item>
    </Breadcrumb>
  );

  return (
    <PageView title={department?.name ?? ""} breadcrumbs={<Breadcrumbs />}>
      <div className="w-full flex flex-row">
        <div style={{ width: "220px" }} className="">
          <div className="pr-2">
            <div className="bg-gray-200 rounded-sm w-28 h-28"></div>
          </div>
          <ul className="edit-sidebar py-5">
            <li>
              <div>Created By</div>
              <div>{department?.createdByName ?? ""}</div>
            </li>
            <li>
              <div>Created At</div>
              <div>
                {dayjs(department?.createdAt ?? new Date()).format(
                  dateFormats.shortDateFormat
                )}
              </div>
            </li>
            <li>
              <div>Updated By</div>
              <div>{department?.updatedByName ?? ""}</div>
            </li>
            <li>
              <div>Updated At</div>
              <div>
                {dayjs(department?.updatedAt ?? new Date()).format(
                  dateFormats.shortDateFormat
                )}
              </div>
            </li>
          </ul>
        </div>

        <div className="w-full bg-white border border-gray-200 rounded-sm px-10 py-10">
          <div className="grid grid-cols-1 md:grid-cols-2 gap-3">
            <div>
              <label className="form-label">Name</label>
              <div className="form-data">{department?.name ?? "N/A"}</div>
            </div>
            <div>
              <label className="form-label">Type</label>
              <div className="form-data">{department?.type ?? "N/A"}</div>
            </div>
            <div>
              <label className="form-label">Manager</label>
              <div className="form-data">
                {department?.managerName ?? "N/A"}
              </div>
            </div>
            <div>
              <label className="form-label">Business Hour</label>
              <div className="form-data">
                {department?.businessHourName ?? "N/A"}
              </div>
            </div>
            <div>
              <label className="form-label">Status</label>
              <div className="form-data">{department?.status ?? "N/A"}</div>
            </div>
            <div>
              <label className="form-label">Outgoing Email</label>
              <div className="form-data">
                {department?.outgoingEmail ?? "N/A"}
              </div>
            </div>
          </div>

          <div className="grid grid-cols-1 gap-5 py-4">
            <div>
              <label className="form-label">Default Response</label>
              <div className="form-data">{department?.response ?? "N/A"}</div>
            </div>
          </div>
        </div>
      </div>
    </PageView>
  );
};

export default ViewDepartment;
