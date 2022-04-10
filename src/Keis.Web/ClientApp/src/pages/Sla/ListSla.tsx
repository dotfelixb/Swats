import { Breadcrumb, Button } from "antd";
import React, { FC } from "react";
import { Link, Outlet } from "react-router-dom";
import { DataTable, PageView } from "../../components";

interface IListSla {}

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
      <tr></tr>
    </DataTable>
    <Outlet />
  </PageView>;
};

export default ListSla;
 