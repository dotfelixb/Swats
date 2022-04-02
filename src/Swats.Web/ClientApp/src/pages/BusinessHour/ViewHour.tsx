import { Breadcrumb } from 'antd';
import React, { FC } from 'react';
import { Link } from 'react-router-dom';
import { PageView } from '../../components';

interface IViewHour {}

const ViewHour : FC<IViewHour> = () => {
  
  const Breadcrumbs: FC = () => (
    <Breadcrumb separator="/">
      <Breadcrumb.Item>
        <Link to="/">Home</Link>
      </Breadcrumb.Item>
      <Breadcrumb.Item>
        <Link to="/admin">Admin</Link>
      </Breadcrumb.Item>
      <Breadcrumb.Item>
        <Link to="/admin/businesshour">Business Hours</Link>
      </Breadcrumb.Item>
      <Breadcrumb.Item>View Hour</Breadcrumb.Item>
    </Breadcrumb>
  );

  return (<PageView title="Viewing Hour" breadcrumbs={<Breadcrumbs />}>


  </PageView>)
}

export default ViewHour;