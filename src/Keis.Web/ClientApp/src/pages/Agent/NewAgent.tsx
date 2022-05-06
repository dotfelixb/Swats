import {CommentOutlined, CustomerServiceOutlined, PhoneOutlined, UserSwitchOutlined,} from "@ant-design/icons";
import {Alert, Breadcrumb, Button, Form, Input, Select, Timeline} from "antd";
import {FC, useEffect, useState} from "react";
import {Link, Outlet, useNavigate} from "react-router-dom";
import {PageView} from "../../components";
import {useApp, useAuth} from "../../context";
import {IFetchDepartment, IFetchTeam, IFetchType, IListResult, ISingleResult} from "../../interfaces";

const {TextArea} = Input;

interface INew {
}

interface IFormData {
    firstname: string;
    lastname: string;
    email: string;
    mobile: string;
    telephone: string;
    timezone: string;
    department: string;
    team: string;
    type: string;
    mode: string;
    status: string;
    note: string;
}

const New: FC<INew> = () => {
    const {user} = useAuth();
    const [form] = Form.useForm();
    const {get} = useApp();
    const navigate = useNavigate();
    const [departmentList, setDepartmentList] = useState<IFetchDepartment[]>();
    const [teamList, setTeamList] = useState<IFetchTeam[]>();
    const [typeList, setTypeList] = useState<IFetchType[]>();
    const [hasFormErrors, setHasFormErrors] = useState(false);
    const [formErrors, setFormErrors] = useState<string[]>();

    useEffect(() => {
        const loadDepartment = async () => {
            const g: Response = await get(`methods/department.list`);

            if (g != null) {
                const d: IListResult<IFetchDepartment[]> = await g.json();

                if (g.status === 200 && d.ok) {
                    setDepartmentList(d.data);
                } else {
                    // TODO: display error to user
                }
            }
        };

        const loadTeam = async () => {
            const g: Response = await get(`methods/team.list`);

            if (g != null) {
                const d: IListResult<IFetchTeam[]> = await g.json();

                if (g.status === 200 && d.ok) {
                    setTeamList(d.data);
                } else {
                    // TODO: display error to user
                }
            }
        };

        const loadType = async () => {
            const g: Response = await get(`methods/tickettype.list`);

            if (g != null) {
                const d: IListResult<IFetchType[]> = await g.json();

                if (g.status === 200 && d.ok) {
                    setTypeList(d.data);
                } else {
                    // TODO: display error to user
                }
            }
        };

        if (user != null) {
            loadDepartment();
            loadTeam();
            loadType();
        }
    }, [user, get]);

    const onFinish = async ({
                                firstname,
                                lastname,
                                email,
                                mobile,
                                telephone,
                                timezone,
                                department,
                                team,
                                type,
                                mode,
                                status,
                                note,
                            }: IFormData) => {
        const body = new FormData();
        body.append("firstname", firstname ?? "");
        body.append("lastname", lastname ?? "");
        body.append("email", email ?? "");
        body.append("mobile", mobile ?? "");
        body.append("telephone", telephone ?? "");
        body.append("timezone", timezone ?? "");
        body.append("department", department ?? "");
        body.append("team", team ?? "");
        body.append("type", type ?? "");
        body.append("mode", mode ?? "");
        body.append("status", status ?? "");
        body.append("note", note ?? "");

        const headers = new Headers();
        headers.append("Authorization", `Bearer ${user?.token ?? ""}`);

        const f = await fetch("methods/agent.create", {
            method: "POST",
            body,
            headers,
        });

        const result: ISingleResult<string> = await f.json();

        if (f.status === 201 && result.ok) {
            navigate(`/admin/agent/${result.data}`, {replace: true});
        }

        setHasFormErrors(true);
        setFormErrors(result?.errors);
    };

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
            <Breadcrumb.Item>New Agent</Breadcrumb.Item>
        </Breadcrumb>
    );

    return (
        <PageView title="New Agent" breadcrumbs={<Breadcrumbs/>}>
            <div className="grid grid-cols-1 lg:grid-cols-2 gap-3">
                <div>
                    <Form form={form} layout="vertical" onFinish={onFinish}>
                        {hasFormErrors &&
                            formErrors?.map((e) => (
                                <div key={e} className="py-2">
                                    <Alert message={e} type="error" className="py-2"/>
                                </div>
                            ))}
                        <Timeline>
                            <Timeline.Item
                                dot={<CustomerServiceOutlined style={{fontSize: "16px"}}/>}
                            >
                                <div className="font-bold mb-2">Agent info</div>
                                <Form.Item
                                    name="firstname"
                                    label="First Name"
                                    htmlFor="firstname"
                                >
                                    <Input/>
                                </Form.Item>
                                <Form.Item name="lastname" label="Last Name" htmlFor="lastname">
                                    <Input/>
                                </Form.Item>
                                <Form.Item name="status" label="Status">
                                    <Select>
                                        <Select.Option value="1">Active</Select.Option>
                                        <Select.Option value="2">Inactive</Select.Option>
                                    </Select>
                                </Form.Item>
                            </Timeline.Item>
                            <Timeline.Item
                                dot={<PhoneOutlined style={{fontSize: "16px"}}/>}
                            >
                                <div className="font-bold mb-2">Agent contact</div>
                                <Form.Item name="email" label="Email" htmlFor="email">
                                    <Input/>
                                </Form.Item>
                                <Form.Item name="mobile" label="Mobile" htmlFor="mobile">
                                    <Input/>
                                </Form.Item>
                                <Form.Item
                                    name="telephone"
                                    label="Telephone"
                                    htmlFor="telephone"
                                >
                                    <Input/>
                                </Form.Item>
                                <Form.Item name="timezone" label="Timezone">
                                    <Select>
                                        <Select.Option value="">Not Available</Select.Option>
                                    </Select>
                                </Form.Item>
                            </Timeline.Item>
                            <Timeline.Item
                                dot={<UserSwitchOutlined style={{fontSize: "16px"}}/>}
                            >
                                <div className="font-bold mb-2">
                                    Agent department, team and service
                                </div>
                                <Form.Item name="department" label="Department">
                                    <Select>
                                        {departmentList?.map(d => (
                                            <Select.Option key={d.id} value={d.id}>{d.name}</Select.Option>))}
                                    </Select>
                                </Form.Item>
                                <Form.Item name="team" label="Team">
                                    <Select>
                                        {teamList?.map(d => (
                                            <Select.Option key={d.id} value={d.id}>{d.name}</Select.Option>))}
                                    </Select>
                                </Form.Item>
                                <Form.Item name="type" label="Default Ticket Type">
                                    <Select>
                                        {typeList?.map(d => (
                                            <Select.Option key={d.id} value={d.id}>{d.name}</Select.Option>))}
                                    </Select>
                                </Form.Item>
                                <Form.Item name="mode" label="Mode">
                                    <Select>
                                        <Select.Option value="1">Agent</Select.Option>
                                        <Select.Option value="2">User</Select.Option>
                                    </Select>
                                </Form.Item>
                            </Timeline.Item>
                            <Timeline.Item
                                dot={<CommentOutlined style={{fontSize: "16px"}}/>}
                            >
                                <div className="font-bold mb-2">
                                    Email signature for agent (optional)
                                </div>
                                <Form.Item name="note" label="Signature" htmlFor="note">
                                    <TextArea rows={4}/>
                                </Form.Item>
                            </Timeline.Item>
                            <Timeline.Item>
                                <Form.Item>
                                    <Button type="primary" htmlType="submit">
                                        Submit
                                    </Button>
                                </Form.Item>
                            </Timeline.Item>
                        </Timeline>
                    </Form>
                </div>

                <div></div>
            </div>

            <Outlet/>
        </PageView>
    );
};

export default New;