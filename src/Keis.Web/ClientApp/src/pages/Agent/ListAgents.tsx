import { Breadcrumb, Button } from "antd";
import dayjs from "dayjs";
import React, { FC, useEffect, useState } from "react";
import { Link, Outlet } from "react-router-dom";
import { DataTable, PageView } from "../../components";
import { useApp, useAuth } from "../../context";
import { IFetchAgent, IListResult } from "../../interfaces";

interface IListAgents {}

const columns = [
  { key: "email", column: [{ title: "Name" }, { title: "" }] },
  { key: "name", column: [{ title: "Email" }, { title: "Mobile" }] },
  {
    key: "departmentTeam",
    column: [{ title: "Department" }, { title: "Team" }],
  },
  {
    key: "created",
    column: [{ title: "Created By" }, { title: "Created At" }],
  },
  { key: "extra", column: [{ title: "" }] },
];

const ListAgents: FC<IListAgents> = () => {
  const { user } = useAuth();
  const { get, dateFormats } = useApp();
  const [agentList, setAgentList] = useState<IFetchAgent[]>();

  useEffect(() => {
    const load = async () => {
      const g: Response = await get("methods/agent.list");

      if (g != null) {
        const d: IListResult<IFetchAgent[]> = await g.json();

        if (g.status === 200 && d.ok) {
          setAgentList(d.data);
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
        <Button type="primary">New Agent</Button>
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
      <Breadcrumb.Item>Agent</Breadcrumb.Item>
    </Breadcrumb>
  );

  return (
    <PageView
      title="Agents"
      buttons={<Buttons />}
      breadcrumbs={<Breadcrumbs />}
    >
      <DataTable columns={columns}>
        {agentList?.map((a) => (
          <tr className="px-10" key={a.id}>
            <td className="px-3 py-3">
              <Link to={`/admin/agent/${a.id}`}>
                <div className="">{`${a.firstName} ${a.lastName}`}</div>
              </Link>
              <div className="text-xs" style={{ color: "#9b9b9b" }}></div>
            </td>

            <td className="px-3 py-3">
              <div className="">{a.email}</div>
              <div className="text-xs" style={{ color: "#9b9b9b" }}>
                {a.mobile}
              </div>
            </td>

            <td className="px-3 py-3">
              <div className="">{a.departmentName}</div>
              <div className="text-xs" style={{ color: "#9b9b9b" }}>
                {a.teamName}
              </div>
            </td>

            <td className="px-3 py-3">
              <div className="">{a.createdByName}</div>
              <div className="text-xs" style={{ color: "#9b9b9b" }}>
                {dayjs(a.createdAt).format(dateFormats.longDateFormat)}
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

export default ListAgents;
