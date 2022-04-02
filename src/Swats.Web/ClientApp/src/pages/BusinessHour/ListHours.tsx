import { Breadcrumb, Button } from "antd";
import React, { FC } from "react";
import { Link, Outlet } from "react-router-dom";
import { PageView } from "../../components";

interface IListHours {}

const ListHours: FC<IListHours> = () => {
  const Buttons: FC = () => (
    <div className="space-x-2">
      <Link to="new">
        <Button type="primary" >New Hour</Button>
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
      <Breadcrumb.Item>Business Hours</Breadcrumb.Item>
    </Breadcrumb>
  );

  return (<PageView title="Business Hours" buttons={<Buttons />} breadcrumbs={<Breadcrumbs />}>


    <Outlet />
  </PageView>)
};

export default ListHours;
