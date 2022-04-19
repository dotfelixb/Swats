import { Breadcrumb, Button } from 'antd';
import React, { FC } from 'react';
import { Link } from 'react-router-dom';
import { DataTable, PageView } from '../../components';

interface IListWorkflow { }

const columns = [
    { key: "name", column: [{ title: "Name" }, { title: "" }] },
    { key: "status", column: [{ title: "Status" }, { title: "Order" }] },
    {
        key: "created",
        column: [{ title: "Created By" }, { title: "Created At" }],
    },
    { key: "extra", column: [{ title: "" }] },
];

const ListWorkflow: FC<IListWorkflow> = () => {
    const Buttons: FC = () => {
        return (
            <div className="space-x-2">
                <Link to="new">
                    <Button type="primary">New Workflow</Button>
                </Link>
            </div>
        );
    };

    const Breadcrumbs: FC = () => (
        <Breadcrumb separator="/">
            <Breadcrumb.Item>
                <Link to="/">Home</Link>
            </Breadcrumb.Item>
            <Breadcrumb.Item>
                <Link to="/admin">Admin</Link>
            </Breadcrumb.Item>
            <Breadcrumb.Item>Workflows</Breadcrumb.Item>
        </Breadcrumb>
    );

    return (<PageView title="Workflows" buttons={<Buttons />} breadcrumbs={<Breadcrumbs />}>
        <DataTable columns={columns}>
            <tr></tr>
        </DataTable>
    </PageView>)
}

export default ListWorkflow;