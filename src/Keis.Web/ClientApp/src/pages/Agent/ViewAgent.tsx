import {Breadcrumb} from "antd";
import dayjs from "dayjs";
import React, {FC, useEffect, useState} from "react";
import {Link, useParams} from "react-router-dom";
import {PageView} from "../../components";
import {useApp, useAuth} from "../../context";
import {IFetchAgent, ISingleResult} from "../../interfaces";

interface IViewAgent {
}

const ViewAgent: FC<IViewAgent> = () => {
    const {user} = useAuth();
    const {get, dateFormats} = useApp();
    const {id} = useParams();
    const [agent, setAgent] = useState<IFetchAgent>();

    useEffect(() => {
        const load = async () => {
            const g: Response = await get(`methods/agent.get?id=${id}`);
            const d: ISingleResult<IFetchAgent> = await g.json();

            if (g != null) {
                if (g.status === 200 && d.ok) {
                    setAgent(d.data);
                } else {
                    // TODO: display error to user
                }
            }
        };

        if (user != null && user.token && id) {
            load();
        }
    }, [user, id, get]);

    const Breadcrumbs: FC = () => (
        <Breadcrumb separator="/">
            <Breadcrumb.Item>
                <Link to="/">Home</Link>
            </Breadcrumb.Item>
            <Breadcrumb.Item>
                <Link to="/admin">Admin</Link>
            </Breadcrumb.Item>
            <Breadcrumb.Item>
                <Link to="/admin/agent">Agents</Link>
            </Breadcrumb.Item>
            <Breadcrumb.Item>
                {`${agent?.firstName ?? ""} ${agent?.lastName ?? ""}`}
            </Breadcrumb.Item>
        </Breadcrumb>
    );

    return (
        <PageView
            title={`${agent?.firstName ?? ""} ${agent?.lastName ?? ""}`}
            breadcrumbs={<Breadcrumbs/>}
        >
            <div className="w-full flex flex-row">
                <div style={{width: "220px"}} className="">
                    <div className="pr-2">
                        <div className="bg-gray-200 rounded-sm w-28 h-28"></div>
                    </div>
                    <ul className="edit-sidebar py-5">
                        <li>
                            <div>Created By</div>
                            <div>{agent?.createdByName ?? ""}</div>
                        </li>
                        <li>
                            <div>Created At</div>
                            <div>
                                {dayjs(agent?.createdAt ?? new Date()).format(
                                    dateFormats.shortDateFormat
                                )}
                            </div>
                        </li>
                        <li>
                            <div>Updated By</div>
                            <div>{agent?.updatedByName ?? ""}</div>
                        </li>
                        <li>
                            <div>Updated At</div>
                            <div>
                                {dayjs(agent?.updatedAt ?? new Date()).format(
                                    dateFormats.shortDateFormat
                                )}
                            </div>
                        </li>
                    </ul>
                </div>

                <div className="w-full bg-white border border-gray-200 rounded-sm px-10 py-10">
                    <div className="grid grid-cols-1 md:grid-cols-2 gap-3">
                        <div>
                            <label className="form-label">Email</label>
                            <div className="form-data">{agent?.email ?? "N/A"}</div>
                        </div>
                        <div>
                            <label className="form-label">Mobile</label>
                            <div className="form-data">{agent?.mobile ?? "N/A"}</div>
                        </div>
                        <div>
                            <label className="form-label">First Name</label>
                            <div className="form-data">{agent?.firstName ?? "N/A"}</div>
                        </div>
                        <div>
                            <label className="form-label">Last Name</label>
                            <div className="form-data">{agent?.lastName ?? "N/A"}</div>
                        </div>
                        <div>
                            <label className="form-label">Department</label>
                            <div className="form-data">{agent?.departmentName ?? "N/A"}</div>
                        </div>
                        <div>
                            <label className="form-label">Team</label>
                            <div className="form-data">{agent?.teamName ?? "N/A"}</div>
                        </div>
                        <div>
                            <label className="form-label">Default Ticket Type</label>
                            <div className="form-data">{agent?.typeName ?? "N/A"}</div>
                        </div>
                        <div>
                            <label className="form-label">Timezone</label>
                            <div className="form-data">{agent?.timezone ?? "N/A"}</div>
                        </div>
                        <div>
                            <label className="form-label">Status</label>
                            <div className="form-data">{agent?.status ?? "N/A"}</div>
                        </div>
                    </div>

                    <div className="grid grid-cols-1 gap-5 py-4">
                        <div>
                            <label className="form-label">Default Response</label>
                            <div className="form-data">{agent?.note ?? "N/A"}</div>
                        </div>
                    </div>
                </div>
            </div>
        </PageView>
    );
};

export default ViewAgent;