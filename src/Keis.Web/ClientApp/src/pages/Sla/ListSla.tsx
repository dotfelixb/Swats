import { Breadcrumb, Button } from "antd";
import dayjs from "dayjs";
import React, { FC, useEffect, useState } from "react";
import { Link, Outlet } from "react-router-dom";
import { DataTable, PageView } from "../../components";
import { useApp, useAuth } from "../../context";
import { IFetchSla, IListResult } from "../../interfaces";

interface IListSla { }

const columns = [
  { key: "name", column: [{ title: "Name" }, { title: "" }] },
  { key: "hour", column: [{ title: "Business Hour" }, { title: "Status" }] },
  {
    key: "created",
    column: [{ title: "Created By" }, { title: "Created At" }],
  },
  { key: "extra", column: [{ title: "" }] },
];

const ListSla: FC<IListSla> = () => {
  const { user } = useAuth();
  const { get, dateFormats } = useApp();
  const [slaList, setSlaList] = useState<IFetchSla[]>();

  useEffect(() => {
    const load = async () => {
      const g: Response = await get("methods/sla.list");

      if (g != null) {
        const d: IListResult<IFetchSla[]> = await g.json();

        if (g.status === 200 && d.ok) {
          setSlaList(d.data);
        } else {
          // TODO: display error to user
        }
      }
    };

    if (user != null && user.token) {
      load();
    }
  }, [user, get]);

  const Buttons: FC = () => (
    <div className="space-x-2">
      <Link to="new">
        <Button type="primary">New SLA</Button>
      </Link>
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
      <Breadcrumb.Item>SLA</Breadcrumb.Item>
    </Breadcrumb>
  );

  return <PageView title="SLA" buttons={<Buttons />} breadcrumbs={<Breadcrumbs />}>
    <DataTable columns={columns}>
      {slaList?.map((s) => (
        <tr className="px-10" key={s.id}>
          <td className="px-3 py-3">
            <Link to={`/admin/sla/${s.id}`}>
              <div className="">{s.name}</div>
            </Link>
            <div className="text-xs" style={{ color: "#9b9b9b" }}>

            </div>
          </td>

          <td className="px-3 py-3">
            <div className="">{s.businessHourName}</div>
            <div className="text-xs" style={{ color: "#9b9b9b" }}>
              {s.status}
            </div>
          </td>

          <td className="px-3 py-3">
            <div className="">{s.createdByName}</div>
            <div className="text-xs" style={{ color: "#9b9b9b" }}>
              {dayjs(s.createdAt).format(dateFormats.longDateFormat)}
            </div>
          </td>

          <td>
            <span className="text-gray-300"></span>
          </td>
        </tr>
      ))}
    </DataTable>
    <Outlet />
  </PageView>;
};

export default ListSla;
