import { Breadcrumb, Button } from "antd";
import React, { FC } from "react";
import { Link, Outlet } from "react-router-dom";

interface IListTicket {}

const ListTicket : FC<IListTicket> = () => {

  return ( <div>
    <Breadcrumb>
      <Breadcrumb.Item >
        <Link to="/">Home</Link>
      </Breadcrumb.Item>
      <Breadcrumb.Item>Tickets</Breadcrumb.Item>
    </Breadcrumb>

    <Link to="new">
      <Button type="primary">New</Button>
    </Link>
    <Link to="sTEhaSsas">
      <Button type="primary">View</Button>
    </Link>

    <Outlet />
  </div> )
}

export default ListTicket;