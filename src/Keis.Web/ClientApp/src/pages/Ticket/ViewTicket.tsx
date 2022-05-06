import {Breadcrumb, Button} from "antd";
import React, {FC} from "react";
import {Link} from "react-router-dom";

interface IViewTicket {
}

const ViewTicket: FC<IViewTicket> = () => {
    return (<div>
        <Breadcrumb>
            <Breadcrumb.Item>
                <Link to="/">Home</Link>
            </Breadcrumb.Item>
            <Breadcrumb.Item>
                <Link to="/ticket">Tickets</Link>
            </Breadcrumb.Item>
            <Breadcrumb.Item>New Ticket</Breadcrumb.Item>
        </Breadcrumb>

        <span>View</span>

        <Link to="/ticket/new">
            <Button type="primary">New</Button>
        </Link>
    </div>)
}

export default ViewTicket;