import {Breadcrumb, Button} from "antd";
import dayjs from "dayjs";
import React, {FC, useEffect, useState} from "react";
import {Link} from "react-router-dom";
import {DataTable, PageView} from "../../components";
import {useApp, useAuth} from "../../context";
import {IFetchWorkflow, IListResult} from "../../interfaces";

interface IListWorkflow {
}

const columns = [
    {key: "name", column: [{title: "Name"}, {title: ""}]},
    {key: "status", column: [{title: "Priority"}, {title: "Status"}]},
    {
        key: "created",
        column: [{title: "Created By"}, {title: "Created At"}],
    },
    {key: "extra", column: [{title: ""}]},
];

const ListWorkflow: FC<IListWorkflow> = () => {
    const {user} = useAuth();
    const {get, dateFormats} = useApp();
    const [workflowList, setWorkflowList] = useState<IFetchWorkflow[]>([]);

    useEffect(() => {
        const load = async () => {
            const g: Response = await get("methods/workflow.list");

            if (g != null) {
                const d: IListResult<IFetchWorkflow[]> = await g.json();

                if (g.status === 200 && d.ok) {
                    setWorkflowList(d.data);
                } else {
                    // TODO: display error to user
                }
            }
        };

        if (user != null && user.token) {
            load();
        }
    }, [user, get]);

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

    return (
        <PageView
            title="Workflows"
            buttons={<Buttons/>}
            breadcrumbs={<Breadcrumbs/>}
        >
            <DataTable columns={columns}>
                {workflowList?.map((w) => (
                    <tr className="px-10" key={w.id}>
                        <td className="px-3 py-3">
                            <Link to={`/admin/workflow/${w.id}`}>
                                <div className="">{w.name}</div>
                            </Link>
                            <div className="text-xs" style={{color: "#9b9b9b"}}></div>
                        </td>

                        <td className="px-3 py-3">
                            <div className="">{w.priority}</div>
                            <div className="text-xs" style={{color: "#9b9b9b"}}>
                                {w.status}
                            </div>
                        </td>

                        <td className="px-3 py-3">
                            <div className="">{w.createdByName}</div>
                            <div className="text-xs" style={{color: "#9b9b9b"}}>
                                {dayjs(w.createdAt).format(dateFormats.longDateFormat)}
                            </div>
                        </td>

                        <td>
                            <span className="text-gray-300"></span>
                        </td>
                    </tr>
                ))}
            </DataTable>
        </PageView>
    );
};

export default ListWorkflow;
