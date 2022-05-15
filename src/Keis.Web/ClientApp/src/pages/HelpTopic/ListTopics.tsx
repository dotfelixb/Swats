import { Breadcrumb, Button } from "antd";
import dayjs from "dayjs";
import React, { FC, useEffect, useState } from "react";
import { Link, Outlet } from "react-router-dom";
import { DataTable, PageView } from "../../components";
import { useApp, useAuth } from "../../context";
import { IFetchTopic, IListResult } from "../../interfaces";

interface IListTopics {}

const columns = [
  { key: "name", column: [{ title: "Name" }, { title: "" }] },
  { key: "status", column: [{ title: "Type" }, { title: "Status" }] },
  {
    key: "created",
    column: [{ title: "Created By" }, { title: "Created At" }],
  },
  { key: "extra", column: [{ title: "" }] },
];

const ListTopics: FC<IListTopics> = () => {
  const { user } = useAuth();
  const { get, dateFormats } = useApp();
  const [topicList, setTopicList] = useState<IFetchTopic[]>();

  useEffect(() => {
    const load = async () => {
      const g: Response = await get("methods/helptopic.list");

      if (g != null) {
        const d: IListResult<IFetchTopic[]> = await g.json();

        if (g.status === 200 && d.ok) {
          setTopicList(d.data);
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
        <Button type="primary">New Topic</Button>
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
      <Breadcrumb.Item>Help Topics</Breadcrumb.Item>
    </Breadcrumb>
  );

  return (
    <PageView
      title="Help Topics"
      buttons={<Buttons />}
      breadcrumbs={<Breadcrumbs />}
    >
      <DataTable columns={columns}>
        {topicList?.map((t) => (
          <tr className="px-10" key={t.id}>
            <td className="px-3 py-3">
              <Link to={`/admin/helptopic/${t.id}`}>
                <div className="">{t.name}</div>
              </Link>
              <div className="text-xs" style={{ color: "#9b9b9b" }}>
                {t.note}
              </div>
            </td>

            <td className="px-3 py-3">
              <div className="">{t.type}</div>
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

      <Outlet />
    </PageView>
  );
};

export default ListTopics;
