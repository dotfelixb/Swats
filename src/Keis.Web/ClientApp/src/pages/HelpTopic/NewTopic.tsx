import {CommentOutlined, EyeOutlined, FormOutlined, UserSwitchOutlined,} from "@ant-design/icons";
import {Alert, Breadcrumb, Button, Form, Input, Select, Timeline} from "antd";
import React, {FC, useEffect, useState} from "react";
import {Link, Outlet, useNavigate} from "react-router-dom";
import {PageView} from "../../components";
import {useApp, useAuth} from "../../context";
import {IFetchDepartment, IListResult, ISingleResult} from "../../interfaces";

const {TextArea} = Input;

interface INewTopic {
}

interface IFormData {
    topic: string;
    department: string;
    type: string;
    status: string;
    visibility: string;
    note: string;
}

const NewTopic: FC<INewTopic> = () => {
    const {user} = useAuth();
    const {get} = useApp();
    const [form] = Form.useForm();
    const navigate = useNavigate();
    const [departmentList, setDepartmentList] = useState<IFetchDepartment[]>();
    const [hasFormErrors, setHasFormErrors] = useState(false);
    const [formErrors, setFormErrors] = useState<string[]>();

    useEffect(() => {
        const load = async () => {
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

        if (user != null) {
            load();
        }
    }, [user, get]);

    const onFinish = async ({
                                topic,
                                department,
                                type,
                                status,
                                visibility,
                                note,
                            }: IFormData) => {
        const body = new FormData();
        body.append("topic", topic ?? "");
        body.append("department", department ?? "");
        body.append("type", type ?? 1);
        body.append("status", status ?? 1);
        body.append("visibility", visibility ?? 1);
        body.append("note", note ?? "");

        const headers = new Headers();
        headers.append("Authorization", `Bearer ${user?.token ?? ""}`);

        const f = await fetch("methods/helptopic.create", {
            method: "POST",
            body,
            headers,
        });

        const result: ISingleResult<string> = await f.json();

        if (f.status === 201 && result.ok) {
            navigate(`/admin/helptopic/${result.data}`, {replace: true});
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
                <Link to="/admin/helptopic">Help Topics</Link>
            </Breadcrumb.Item>
            <Breadcrumb.Item>New Topic</Breadcrumb.Item>
        </Breadcrumb>
    );

    return (
        <PageView title="New Topic" breadcrumbs={<Breadcrumbs/>}>
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
                                <div className="font-bold mb-2">Help topic name</div>
                                <Form.Item name="topic" label="Topic" htmlFor="topic">
                                    <Input/>
                                </Form.Item>
                            </Timeline.Item>
                            <Timeline.Item dot={<EyeOutlined style={{fontSize: "16px"}}/>}>
                                <div className="font-bold mb-2">
                                    External visibility and status
                                </div>
                                <Form.Item name="type" label="Visibility">
                                    <Select>
                                        <Select.Option value="1">Public</Select.Option>
                                        <Select.Option value="2">Private</Select.Option>
                                    </Select>
                                </Form.Item>
                                <Form.Item name="status" label="Status">
                                    <Select>
                                        <Select.Option value="1">Active</Select.Option>
                                        <Select.Option value="2">Inactive</Select.Option>
                                    </Select>
                                </Form.Item>
                            </Timeline.Item>
                            <Timeline.Item
                                dot={<UserSwitchOutlined style={{fontSize: "16px"}}/>}
                            >
                                <div className="font-bold mb-2">
                                    Which department is this topic linked to
                                </div>
                                <Form.Item name="department" label="Department" htmlFor="department">
                                    <Select showSearch>
                                        {departmentList?.map(d => (
                                            <Select.Option key={d.id} value={d.id}>{d.name}</Select.Option>))}
                                    </Select>
                                </Form.Item>
                            </Timeline.Item>
                            <Timeline.Item
                                dot={<CommentOutlined style={{fontSize: "16px"}}/>}
                            >
                                <div className="font-bold mb-2">
                                    Additional note for this topic (optional)
                                </div>
                                <Form.Item name="note" label="Note" htmlFor="note">
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

export default NewTopic;