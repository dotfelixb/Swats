import {
    ClockCircleOutlined,
    CommentOutlined,
    FormOutlined,
    ScheduleOutlined,
} from "@ant-design/icons";
import { Alert, Breadcrumb, Button, Form, Input, Select, Timeline } from "antd";
import React, { FC, useState } from "react";
import { Link, Outlet, useNavigate } from "react-router-dom";
import { PageView, WorkflowAction, WorkflowCriteria } from "../../components";
import { useApp, useAuth } from "../../context";

const { TextArea } = Input;

interface ICriteria {
    key: number;
}

interface IAction {
    key: number;
}

interface INewWorkflow { }

interface IEventTag {
    key: string;
    value: string;
}

const eventTags: IEventTag[] = [
    { key: "newticket", value: "New Ticket" },
    { key: "changetype", value: "Change Ticket Type" },
    { key: "changedepartment", value: "Change Department" },
    { key: "changeteam", value: "Change Team" },
    { key: "changepriority", value: "Change Ticket Priority" },
    { key: "changestatus", value: "Change Ticket Status" },
    { key: "ticketreply", value: "Ticket Reply" },
    { key: "ticketcomment", value: "Ticket Comment" },
];

const NewWorkflow: FC<INewWorkflow> = () => {
    const { user } = useAuth();
    const { get } = useApp();
    const [form] = Form.useForm();
    const navigate = useNavigate();
    const [criteriaList, setCriteriaList] = useState<ICriteria[]>([]);
    const [actionList, setActionList] = useState<IAction[]>([]);
    const [hasFormErrors, setHasFormErrors] = useState(false);
    const [formErrors, setFormErrors] = useState<string[]>();

    const onFinish = (values: any) => { };

    const onAddCriteria = () => {
        const newKey = +new Date();
        setCriteriaList([...criteriaList, { key: newKey }]);
    };

    const onRemoveCriterial = (key: number) => {
        setCriteriaList(criteriaList.filter((f) => f.key !== key));
    };

    const onAddAction = () => {
        const newKey = +new Date();
        setActionList([...actionList, { key: newKey }]);
    };

    const onRemoveAction = (key: number) => {
        setActionList(actionList.filter((f) => f.key !== key));
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
                <Link to="/admin/workflow">Workflows</Link>
            </Breadcrumb.Item>
            <Breadcrumb.Item>New Workflow</Breadcrumb.Item>
        </Breadcrumb>
    );

    return (
        <PageView title="New Workflow" breadcrumbs={<Breadcrumbs />}>
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
                                <div className="font-bold mb-2">Workflow name and events</div>
                                <Form.Item name="name" label="Name" htmlFor="name">
                                    <Input />
                                </Form.Item>
                                <Form.Item name="event" label="Events">
                                    <Select showSearch mode="multiple">
                                        {eventTags.map((e) => (
                                            <Select.Option key={e.key} value={e.key}>
                                                {e.value}
                                            </Select.Option>
                                        ))}
                                    </Select>
                                </Form.Item>
                            </Timeline.Item>
                            <Timeline.Item
                                dot={<ClockCircleOutlined style={{ fontSize: "16px" }} />}
                            >
                                <div className="font-bold mb-2">
                                    Workflow Criteria matches and conditions
                                </div>

                                {criteriaList?.map((c) => (
                                    <WorkflowCriteria
                                        key={c.key}
                                        id={c.key}
                                        onClick={onRemoveCriterial}
                                    />
                                ))}

                                <Form.Item>
                                    <Button type="default" onClick={onAddCriteria}>
                                        Add Criteria
                                    </Button>
                                </Form.Item>
                            </Timeline.Item>
                            <Timeline.Item
                                dot={<CommentOutlined style={{ fontSize: "16px" }} />}
                            >
                                <div className="font-bold mb-2">
                                    Action to perform base on the events above
                                </div>

                                {actionList?.map((c) => (
                                    <WorkflowAction
                                        key={c.key}
                                        id={c.key}
                                        onClick={onRemoveAction}
                                    />
                                ))}

                                <Form.Item>
                                    <Button type="default" onClick={onAddAction}>
                                        Add Action
                                    </Button>
                                </Form.Item>
                            </Timeline.Item>
                            <Timeline.Item
                                dot={<CommentOutlined style={{ fontSize: "16px" }} />}
                            >
                                <div className="font-bold mb-2">
                                    Addition note for this workflow (optional)
                                </div>
                                <Form.Item name="note" label="Note" htmlFor="note">
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
        </PageView>
    );
};

export default NewWorkflow;