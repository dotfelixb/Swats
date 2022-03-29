import { Breadcrumb, Button } from "antd";
import React, { FC } from "react";
import { Link, Outlet } from "react-router-dom";
import { PageView } from "../../components";

interface IListTicket { }

const ListTicket: FC<IListTicket> = () => {

  const Buttons: FC = () => (
    <div className="space-x-2">
      <Link to="/ticket/new">
        <Button type="primary" >New Ticket</Button>
      </Link>
    </div>
  );

  const Breadcrumbs: FC = () => (
    <Breadcrumb separator="/">
      <Breadcrumb.Item>
        <Link to="/">Home</Link>
      </Breadcrumb.Item>
      <Breadcrumb.Item>Tickets</Breadcrumb.Item>
    </Breadcrumb>
  )

  return (<PageView title="Tickets" buttons={<Buttons />} breadcrumbs={<Breadcrumbs />}>


    <Outlet />
  </PageView>)
}

export default ListTicket;