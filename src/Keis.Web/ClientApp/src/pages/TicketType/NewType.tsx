import { CommentOutlined, EyeOutlined, FormOutlined } from '@ant-design/icons';
import { Breadcrumb, Form, Select, Input, Timeline, Button, Alert } from 'antd';
import { FC, useState } from 'react';
import { Link, Outlet, useNavigate } from 'react-router-dom';
import { PageView } from '../../components';
import { useAuth } from '../../context';
import { ISingleResult } from '../../interfaces';

const { TextArea } = Input;

interface INewType { }

interface IFormData {
    name: string;
    color: string;
    status: string;
    visibility: string;
    description: string;
}

const NewType: FC<INewType> = () => {
    const { user } = useAuth();
    const [form] = Form.useForm();
    const navigate = useNavigate();
    const [hasFormErrors, setHasFormErrors] = useState(false);
    const [formErrors, setFormErrors] = useState<string[]>();

    const onFinish = async ({ name, color, status, visibility, description }: IFormData) => {
        const body = new FormData();
        body.append("name", name ?? "");
        body.append("color", color ?? "");
        body.append("status", status ?? "");
        body.append("visibility", visibility ?? "");
        body.append("description", description ?? "");

        const headers = new Headers();
        headers.append("Authorization", `Bearer ${user?.token ?? ""}`);

        const f = await fetch("methods/tickettype.create", {
            method: "POST",
            body,
            headers,
        });

        const result: ISingleResult<string> = await f.json();

        if (f.status === 201 && result.ok) {
            navigate(`/admin/tickettype/${result.data}`, { replace: true });
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
                <Link to="/admin/tickettype">Ticket Types</Link>
            </Breadcrumb.Item>
            <Breadcrumb.Item>New Type</Breadcrumb.Item>
        </Breadcrumb>
    );

    return (<PageView title='New Type' breadcrumbs={<Breadcrumbs />}>
        <div className="grid grid-cols-1 lg:grid-cols-2 gap-3">
            <div>
                <Form form={form} layout="vertical" onFinish={onFinish}>
                    {hasFormErrors &&
                        formErrors?.map((e) => (
                            <div key={e} className="py-2">
                                <Alert message={e} type="error" className="py-2" />
                            </div>
                        ))}
                    <Timeline>
                        <Timeline.Item
                            dot={<FormOutlined style={{ fontSize: "16px" }} />}
                        >
                            <div className="font-bold mb-2">Ticket type name</div>
                            <Form.Item name='name' label="Name" htmlFor="name">
                                <Input />
                            </Form.Item>
                        </Timeline.Item>
                        <Timeline.Item
                            dot={<EyeOutlined style={{ fontSize: "16px" }} />}
                        >
                            <div className="font-bold mb-2">External visibility and status</div>
                            <Form.Item name="visibility" label="Visibility">
                                <Select >
                                    <Select.Option value="1">Public</Select.Option>
                                    <Select.Option value="2">Private</Select.Option>
                                </Select>
                            </Form.Item>
                            <Form.Item name="status" label="Status">
                                <Select >
                                    <Select.Option value="1">Active</Select.Option>
                                    <Select.Option value="2">Inactive</Select.Option>
                                </Select>
                            </Form.Item>
                        </Timeline.Item>
                        <Timeline.Item
                            dot={<CommentOutlined style={{ fontSize: "16px" }} />}
                        >
                            <div className="font-bold mb-2">The description of this type (optional)</div>
                            <Form.Item name='description' label="Description" htmlFor='description'>
                                <TextArea rows={4} />
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
        <Outlet />
    </PageView>)
}

export default NewType;