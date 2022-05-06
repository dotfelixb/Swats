import {Breadcrumb, Button} from "antd";
import React, {FC} from "react";
import {Link, Outlet} from "react-router-dom";
import {DataTable, PageView} from "../../components";

interface IListTickets {
}

const columns = [
    {key: "name", column: [{title: "Subject"}, {title: ""}]},
    {key: "requester", column: [{title: "Requester"}, {title: "Assigned To"}]},
    {key: "department", column: [{title: "Department"}, {title: "Team"}]},
    {
        key: "created",
        column: [{title: "Created By"}, {title: "Created At"}],
    },
    {key: "extra", column: [{title: ""}]},
];

const ListTickets: FC<IListTickets> = () => {
    const Buttons: FC = () => (
        <div className="space-x-2">
            <Link to="/ticket/new">
                <Button type="primary">New Ticket</Button>
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

    return (<PageView title="Tickets" buttons={<Buttons/>} breadcrumbs={<Breadcrumbs/>}>
        <DataTable columns={columns}>

        </DataTable>

        <Outlet/>
    </PageView>)
}

export default ListTickets;