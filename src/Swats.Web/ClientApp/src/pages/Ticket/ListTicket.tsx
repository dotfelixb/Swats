import { Breadcrumb, Button } from "antd";
import React, { FC } from "react";
import { Link, Outlet } from "react-router-dom";
import { PageView } from "../../components";

interface IListTicket { }

const ListTicket: FC<IListTicket> = () => {

  return (<PageView title="Tickets">

    <Outlet />
  </PageView>)
}

export default ListTicket;