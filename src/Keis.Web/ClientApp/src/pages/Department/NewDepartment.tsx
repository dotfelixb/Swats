import {CommentOutlined, EyeOutlined, FormOutlined} from '@ant-design/icons';
import {Alert, Breadcrumb, Button, Form, Input, Select, Timeline} from 'antd';
import React, {FC, useEffect, useState} from 'react';
import {Link, Outlet, useNavigate} from 'react-router-dom';
import {PageView} from '../../components';
import {useApp, useAuth} from '../../context';
import {IFetchBusinessHour, IFetchDepartment, IListResult, ISingleResult} from '../../interfaces';

const {TextArea} = Input;

interface INewDepartment {
}

interface IFormData {
    name: string;
    type: string;
    agent: string;
    businessHour: string;
    outgoingEmail: string;
    status: string;
    response: string;
}

const NewDepartment: FC<INewDepartment> = () => {
    const {user} = useAuth();
    const {get} = useApp();
    const [form] = Form.useForm();
    const navigate = useNavigate();
    const [departmentList, setDepartmentList] = useState<IFetchDepartment[]>();
    const [hourList, setHourList] = useState<IFetchBusinessHour[]>();
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

        const loadHour = async () => {
            const g: Response = await get(`methods/businesshour.list`);

            if (g != null) {
                const d: IListResult<IFetchBusinessHour[]> = await g.json();

                if (g.status === 200 && d.ok) {
                    setHourList(d.data);
                } else {
                    // TODO: display error to user
                }
            }
        };

        if (user != null) {
            loadDepartment();
            loadHour()
        }
    }, [user, get]);

    const onFinish = async ({name, type, agent, businessHour, outgoingEmail, status, response}: IFormData) => {
        const body = new FormData();
        body.append("name", name ?? "");
        body.append("type", type ?? "");
        body.append("agent", agent ?? "");
        body.append("businessHour", businessHour ?? "");
        body.append("outgoingEmail", outgoingEmail ?? "");
        body.append("status", status ?? 1);
        body.append("response", response ?? "");

        const headers = new Headers();
        headers.append("Authorization", `Bearer ${user?.token ?? ""}`);

        const f = await fetch("methods/department.create", {
            method: "POST",
            body,
            headers,
        });

        const result: ISingleResult<string> = await f.json();

        if (f.status === 201 && result.ok) {
            navigate(`/admin/department/${result.data}`, {replace: true});
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
                <Link to="/admin/department">Departments</Link>
            </Breadcrumb.Item>
            <Breadcrumb.Item>New Team</Breadcrumb.Item>
        </Breadcrumb>
    );

    return (<PageView title='New Department' breadcrumbs={<Breadcrumbs/>}>
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
                            dot={<FormOutlined style={{fontSize: "16px"}}/>}
                        >
                            <div className="font-bold mb-2">Department name and type</div>
                            <Form.Item name='name' label="Name" htmlFor="name">
                                <Input/>
                            </Form.Item>
                            <Form.Item name="type" label="Type">
                                <Select>
                                    <Select.Option value="1">Public</Select.Option>
                                    <Select.Option value="2">Private</Select.Option>
                                </Select>
                            </Form.Item>
                        </Timeline.Item>
                        <Timeline.Item
                            dot={<EyeOutlined style={{fontSize: "16px"}}/>}
                        >
                            <div className="font-bold mb-2">Department lead, working hour and status</div>
                            <Form.Item name="manager" label="Manager">
                                <Select>
                                    {departmentList?.map(d => (
                                        <Select.Option key={d.id} value={d.id}>{d.name}</Select.Option>))}
                                </Select>
                            </Form.Item>
                            <Form.Item name="businessHour" label="Business Hour">
                                <Select>
                                    {hourList?.map(d => (
                                        <Select.Option key={d.id} value={d.id}>{d.name}</Select.Option>))}
                                </Select>
                            </Form.Item>
                            <Form.Item name='outgoingEmail' label="Outgoing Email" htmlFor="outgoingEmail">
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
                            dot={<CommentOutlined style={{fontSize: "16px"}}/>}
                        >
                            <div className="font-bold mb-2">The default response to send in email (optional)</div>
                            <Form.Item name='response' label="Default Response" htmlFor='response'>
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
    </PageView>)
}

export default NewDepartment;