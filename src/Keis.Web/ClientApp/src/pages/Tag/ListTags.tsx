import { Breadcrumb, Button } from "antd";
import dayjs from "dayjs";
import React, { FC, useEffect, useState } from "react";
import { Link, Outlet } from "react-router-dom";
import { DataTable, PageView } from "../../components";
import { useApp, useAuth } from "../../context";
import { IFetchTag, IListResult } from "../../interfaces";

interface IListTags { }

const columns = [
    { key: "name", column: [{ title: "Name" }, { title: "" }] },
    { key: "status", column: [{ title: "Visibility" }, { title: "Status" }] },
    {
        key: "created",
        column: [{ title: "Created By" }, { title: "Created At" }],
    },
    { key: "extra", column: [{ title: "" }] },
];

const ListTags: FC<IListTags> = () => {
    const { user } = useAuth();
    const { get, dateFormats } = useApp();
    const [tagList, setTagList] = useState<IFetchTag[]>();

    useEffect(() => {
        const load = async () => {
            const g: Response = await get("methods/tag.list");

            if (g != null) {
                const d: IListResult<IFetchTag[]> = await g.json();

                if (g.status === 200 && d.ok) {
                    setTagList(d.data);
                } else {
                    // TODO: display error to user
                }
            }
        };

        if (user != null && user.token) {
            load();
        }
    }, [user, get]);

    const Buttons: FC = () => (
        <div className="space-x-2">
            <Link to="new">
                <Button type="primary">New Tag</Button>
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
            <Breadcrumb.Item>Tags</Breadcrumb.Item>
        </Breadcrumb>
    );

    return (
        <PageView title="Tags" buttons={<Buttons />} breadcrumbs={<Breadcrumbs />}>
            <DataTable columns={columns}>
                {tagList?.map((t) => (
                    <tr className="px-10" key={t.id}>
                        <td className="px-3 py-3">
                            <Link to={`/admin/tag/${t.id}`}>
                                <div className="">{t.name}</div>
                            </Link>
                            <div className="text-xs" style={{ color: "#9b9b9b" }}>
                            </div>
                        </td>

                        <td className="px-3 py-3">
                            <div className="">{t.visibility}</div>
                            <div className="text-xs" style={{ color: "#9b9b9b" }}>
                                {t.status}
                            </div>
                        </td>

                        <td className="px-3 py-3">
                            <div className="">{t.createdByName}</div>
                            <div className="text-xs" style={{ color: "#9b9b9b" }}>
                                {dayjs(t.createdAt).format(dateFormats.longDateFormat)}
                            </div>
                        </td>

                        <td>
                            <span className="text-gray-300"></span>
                        </td>
                    </tr>
                ))}
            </DataTable>

            <Outlet />
        </PageView>
    );
};

export default ListTags;