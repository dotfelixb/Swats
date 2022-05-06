import {Breadcrumb, Button} from "antd";
import dayjs from "dayjs";
import React, {FC, useEffect, useState} from "react";
import {Link, Outlet} from "react-router-dom";
import {DataTable, PageView} from "../../components";
import {useApp, useAuth} from "../../context";
import {IFetchBusinessHour, IListResult} from "../../interfaces";

interface IListHours {
}

interface IHourListItem {
    data: IFetchBusinessHour;
}

const HourListItem = ({data}: IHourListItem) => {
    const {dateFormats} = useApp();

    return (
        <tr className="px-10" key={data.id}>
            <td className="px-3 py-1">
                <Link to={`/admin/businesshour/${data.id}`}>
                    <div className="">{data.name}</div>
                </Link>
                <div className="text-xs" style={{color: "#9b9b9b"}}>
                    {data.description}
                </div>
            </td>

            <td className="px-3 py-3">
                <div className="">{data.status}</div>
                <div className="text-xs" style={{color: "#9b9b9b"}}>
                    {data.timezone}
                </div>
            </td>
            <td className="px-3 py-3">
                <div className="">{data.createdByName}</div>
                <div className="text-xs" style={{color: "#9b9b9b"}}>
                    {dayjs(data.createdAt).format(dateFormats.longDateFormat)}
                </div>
            </td>
            <td>
                <span className="text-gray-300"></span>
            </td>
        </tr>
    );
};

const columns = [
    {key: "name", column: [{title: "Name"}, {title: "Description"}]},
    {key: "status", column: [{title: "Status"}, {title: "Timezone"}]},
    {
        key: "created",
        column: [{title: "Created By"}, {title: "Created At"}],
    },
    {key: "extra", column: [{title: ""}]},
];

const ListHours: FC<IListHours> = () => {
    const {user} = useAuth();
    const {get} = useApp();
    const [hourList, setHourList] = useState<IFetchBusinessHour[]>();

    useEffect(() => {
        const load = async () => {
            const g: Response = await get("methods/businesshour.list");

            if (g != null) {
                const d: IListResult<IFetchBusinessHour[]> = await g.json();

                if (g.status === 200 && d.ok) {
                    setHourList(d.data);
                } else {
                    // TODO: display error to user
                }
            }
        };

        // because useEffect is not async
        if (user != null && user.token) {
            load();
        }
    }, [user, get]);

    const Buttons: FC = () => (
        <div className="space-x-2">
            <Link to="new">
                <Button type="primary">New Hour</Button>
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
            <Breadcrumb.Item>Business Hours</Breadcrumb.Item>
        </Breadcrumb>
    );

    return (
        <PageView
            title="Business Hours"
            buttons={<Buttons/>}
            breadcrumbs={<Breadcrumbs/>}
        >
            <DataTable columns={columns}>
                {hourList?.map((h) => (
                    <HourListItem key={h.id} data={h}/>
                ))}
            </DataTable>

            <Outlet/>
        </PageView>
    );
};

export default ListHours;