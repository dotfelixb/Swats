import { Breadcrumb, Button } from "antd";
import dayjs from "dayjs";
import React, { FC, useEffect, useState } from "react";
import { Link, Outlet } from "react-router-dom";
import { DataTable, PageView } from "../../components";
import { useApp, useAuth } from "../../context";
import { IFetchDepartment, IListResult } from "../../interfaces";

interface IListDepartments {}

const columns = [
  { key: "name", column: [{ title: "Name" }, { title: "" }] },
  { key: "manager", column: [{ title: "Manager" }, { title: "Type" }] },
  {
    key: "hour",
    column: [{ title: "Business Hour" }, { title: "Outgoing Email" }],
  },
  {
    key: "created",
    column: [{ title: "Created By" }, { title: "Created At" }],
  },
  { key: "extra", column: [{ title: "" }] },
];

const ListDepartments: FC<IListDepartments> = () => {
  const { user } = useAuth();
  const { get, dateFormats } = useApp();
  const [departmentList, setDepartmentList] = useState<IFetchDepartment[]>();

  useEffect(() => {
    const load = async () => {
      const g: Response = await get("methods/department.list");

      if (g != null) {
        const d: IListResult<IFetchDepartment[]> = await g.json();

        if (g.status === 200 && d.ok) {
          setDepartmentList(d.data);
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
        <Button type="primary">New Department</Button>
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
      <Breadcrumb.Item>Departments</Breadcrumb.Item>
    </Breadcrumb>
  );

  return (
    <PageView
      title="Departments"
      buttons={<Buttons />}
      breadcrumbs={<Breadcrumbs />}
    >
      <DataTable columns={columns}>
        {departmentList?.map((d) => (
          <tr className="px-10" key={d.id}>
            <td className="px-3 py-3">
              <Link to={`/admin/department/${d.id}`}>
                <div className="">{d.name}</div>
              </Link>
              <div className="text-xs" style={{ color: "#9b9b9b" }}></div>
            </td>

            <td className="px-3 py-3">
              <div className="">{d.managerName}</div>
              <div className="text-xs" style={{ color: "#9b9b9b" }}>
                {d.type}
              </div>
            </td>

            <td className="px-3 py-3">
              <div className="">{d.businessHourName}</div>
              <div className="text-xs" style={{ color: "#9b9b9b" }}>
                {d.outgoingEmail}
              </div>
            </td>

            <td className="px-3 py-3">
              <div className="">{d.createdByName}</div>
              <div className="text-xs" style={{ color: "#9b9b9b" }}>
                {dayjs(d.createdAt).format(dateFormats.longDateFormat)}
              </div>
            </td>

            <td>
              <span className="text-gray-300"></span>
            </td>
          </tr>
        ))}
      </DataTable>

      <Outlet />
    </PageView>
  );
};

export default ListDepartments;
