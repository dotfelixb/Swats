import { Breadcrumb, Button } from "antd";
import dayjs from "dayjs";
import React, { FC, useCallback, useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { DataTable, PageView } from "../../components";
import { useApp, useAuth } from "../../context";
import { IFetchEmail, IListResult } from "../../interfaces";

interface IListEmail {}

const columns = [
  { key: "email", column: [{ title: "Email" }, { title: "" }] },
  { key: "name", column: [{ title: "Display Name" }, { title: "" }] },
  { key: "status", column: [{ title: "Status" }, { title: "" }] },
  {
    key: "created",
    column: [{ title: "Created By" }, { title: "Created At" }],
  },
  { key: "extra", column: [{ title: "" }] },
];

const ListEmail: FC<IListEmail> = () => {
  const { user } = useAuth();
  const { get, dateFormats } = useApp();
  const [emailList, setEmailList] = useState<IFetchEmail[]>();

  const loadEmails = useCallback(async () => {
    const g: Response = await get("methods/email.list");

    if (g != null) {
      const d: IListResult<IFetchEmail[]> = await g.json();

      if (g.status === 200 && d.ok) {
        setEmailList(d.data);
      } else {
        // TODO: display error to user
      }
    }
  }, [get]);

  useEffect(() => {
    if (user != null && user.token) {
      loadEmails();
    }
  }, [user, loadEmails]);

  const Buttons: FC = () => (
    <div className="space-x-2">
      <Link to="new">
        <Button type="primary">New Email Settings</Button>
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
      <Breadcrumb.Item>Email Settings</Breadcrumb.Item>
    </Breadcrumb>
  );

  return (
    <PageView
      title="Email Settings"
      buttons={<Buttons />}
      breadcrumbs={<Breadcrumbs />}
    >
      <DataTable columns={columns}>
        {emailList?.map((e) => (
          <tr className="px-10" key={e.id}>
            <td className="px-3 py-3">
              <Link to={`/admin/email/${e.id}`}>
                <div className="">{e.address}</div>
              </Link>
              <div className="text-xs" style={{ color: "#9b9b9b" }}></div>
            </td>

            <td className="px-3 py-3">
              <div className="">{e.name}</div>
              <div className="text-xs" style={{ color: "#9b9b9b" }}>
              </div>
            </td>

            <td className="px-3 py-3">
              <div className="">{e.status}</div>
              <div className="text-xs" style={{ color: "#9b9b9b" }}>
              </div>
            </td>

            <td className="px-3 py-3">
              <div className="">{e.createdByName}</div>
              <div className="text-xs" style={{ color: "#9b9b9b" }}>
                {dayjs(e.createdAt).format(dateFormats.longDateFormat)}
              </div>
            </td>

            <td>
              <span className="text-gray-300"></span>
            </td>
          </tr>
        ))}
      </DataTable>
    </PageView>
  );
};

export default ListEmail;
