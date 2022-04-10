import { Breadcrumb } from 'antd';
import React, { FC } from 'react';
import { Link } from 'react-router-dom';
import { PageView } from '../../components';

interface IViewSla {}

const ViewSla : FC<IViewSla> = () => {

  const Breadcrumbs: FC = () => (
    <Breadcrumb separator="/">
      <Breadcrumb.Item>
        <Link to="/">Home</Link>
      </Breadcrumb.Item>
      <Breadcrumb.Item>
        <Link to="/admin">Admin</Link>
      </Breadcrumb.Item>
      <Breadcrumb.Item>
        <Link to="/admin/sla">SLA</Link>
      </Breadcrumb.Item>
      <Breadcrumb.Item>View SLA
      </Breadcrumb.Item>
    </Breadcrumb>
  );
  
  return ( <PageView title='View SLA' breadcrumbs={<Breadcrumbs />}>


  </PageView> )
}

export default ViewSla;