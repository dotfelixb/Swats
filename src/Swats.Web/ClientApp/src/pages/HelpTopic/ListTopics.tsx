import { Breadcrumb, Button } from 'antd';
import React, { FC } from 'react';
import { Link, Outlet } from 'react-router-dom';
import { PageView } from '../../components';

interface IListTopics {}

const ListTopics : FC<IListTopics> = () => {
  const Buttons: FC = () => (
    <div className="space-x-2">
      <Link to="new">
        <Button type="primary" >New Topic</Button>
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

  return (<PageView title="Help Topics" buttons={<Buttons />} breadcrumbs={<Breadcrumbs />}>


    <Outlet />
  </PageView>)
}

export default ListTopics;