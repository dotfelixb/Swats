import { Breadcrumb, Button } from "antd";
import dayjs from "dayjs";
import React, { FC, useCallback, useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { DataTable, PageView } from "../../components";
import { useApp, useAuth } from "../../context";
import { IFetchTemplate, IListResult } from "../../interfaces";

interface IListTemplate {}

const columns = [
  { key: "name", column: [{ title: "Name" }, { title: "" }] },
  { key: "status", column: [{ title: "Subject" }, { title: "Status" }] },
  {
    key: "created",
    column: [{ title: "Created By" }, { title: "Created At" }],
  },
  { key: "extra", column: [{ title: "" }] },
];

const ListTemplate: FC<IListTemplate> = () => {
  const { user } = useAuth();
  const { get, dateFormats } = useApp();
  const [templateList, setTempalteList] = useState<IFetchTemplate[]>();
  
  const load = useCallback(async () => {
    const g: Response = await get("methods/template.list");

    if (g != null) {
      const d: IListResult<IFetchTemplate[]> = await g.json();

      if (g.status === 200 && d.ok) {
        setTempalteList(d.data);
      } else {
        // TODO: display error to user
      }
    }
  }, [get]);

  useEffect(() => {

    if (user != null && user.token) {
      load();
    }
  }, [user, get, load]);
  
  const Buttons: FC = () => (
    <div className="space-x-2">
      <Link to="new">
        <Button type="primary">New Template</Button>
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
      <Breadcrumb.Item>Templates</Breadcrumb.Item>
    </Breadcrumb>
  );

  return (
    <PageView
      title="Templates"
      buttons={<Buttons />}
      breadcrumbs={<Breadcrumbs />}
    >
      <DataTable columns={columns}>
      {templateList?.map((t) => (
          <tr className="px-10" key={t.id}>
            <td className="px-3 py-3">
              <Link to={`/admin/template/${t.id}`}>
                <div className="">{t.name}</div>
              </Link>
            </td>

            <td className="px-3 py-3">
              <div className="">{t.subject }</div>
              <div className="text-xs" style={{ color: "#9b9b9b" }}>
                {t.status}
              </div>
            </td>

            <td className="px-3 py-3">
              <div className="">{t.createdByName}</div>
              <div className="text-xs" style={{ color: "#9b9b9b" }}>
                {dayjs(t.createdAt).format(dateFormats.longDateFormat)}
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

export default ListTemplate;
