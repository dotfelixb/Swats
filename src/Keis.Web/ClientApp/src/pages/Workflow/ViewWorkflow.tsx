import { Breadcrumb } from 'antd';
import React, { FC } from 'react';
import { Link } from 'react-router-dom';
import { PageView } from '../../components';

interface IViewWorkflow {}

const ViewWorkflow : FC<IViewWorkflow> = () => {
  
  const Breadcrumbs: FC = () => (
    <Breadcrumb separator="/">
      <Breadcrumb.Item>
        <Link to="/">Home</Link>
      </Breadcrumb.Item>
      <Breadcrumb.Item>
        <Link to="/admin">Admin</Link>
      </Breadcrumb.Item>
      <Breadcrumb.Item>
        <Link to="/admin/workflow">Workflows</Link>
      </Breadcrumb.Item>
      <Breadcrumb.Item>View Workflow
      </Breadcrumb.Item>
    </Breadcrumb>
  );
  
  return ( <PageView title='View Workflow' breadcrumbs={<Breadcrumbs />}>


  </PageView> )
}

export default ViewWorkflow;