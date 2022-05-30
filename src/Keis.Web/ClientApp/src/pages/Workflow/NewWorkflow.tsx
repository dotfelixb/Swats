import {
  ClockCircleOutlined,
  CommentOutlined,
  FireOutlined,
  FormOutlined,
} from "@ant-design/icons";
import { Alert, Breadcrumb, Button, Form, Input, Select, Timeline } from "antd";
import React, { FC, useCallback, useEffect, useState } from "react";
import { Link, Outlet, useNavigate } from "react-router-dom";
import { PageView, WorkflowAction, WorkflowCriteria } from "../../components";
import { useApp, useAuth } from "../../context";
import {
  IListResult,
  ISingleResult,
  IWorkflowAction,
  IWorkflowCriteria,
  IWorkflowEvent,
} from "../../interfaces";

const { TextArea } = Input;

interface IFormData {
  name: string;
  events: string[];
  priority: number;
  status: string;
  note: string;
}

interface INewWorkflow {}

const NewWorkflow: FC<INewWorkflow> = () => {
  const { user } = useAuth();
  const { get } = useApp();
  const [form] = Form.useForm();
  const navigate = useNavigate();
  const [criteriaList, setCriteriaList] = useState<IWorkflowCriteria[]>([]);
  const [actionList, setActionList] = useState<IWorkflowAction[]>([]);
  const [workflowEventList, setWorkflowEventList] = useState<IWorkflowEvent[]>(
    []
  );
  const [workflowCriteriaList, setWorkflowCriteriaList] = useState<
    IWorkflowCriteria[]
  >([]);
  const [workflowActionList, setWorkflowActionList] = useState<
    IWorkflowAction[]
  >([]);
  const [hasFormErrors, setHasFormErrors] = useState(false);
  const [formErrors, setFormErrors] = useState<string[]>();

  const loadEvent = useCallback(async () => {
    const g: Response = await get(`methods/workflow.event`);

    if (g != null) {
      const d: IListResult<IWorkflowEvent[]> = await g.json();

      if (g.status === 200 && d.ok) {
        setWorkflowEventList(d.data);
      } else {
        // TODO: display error to user
      }
    }
  }, [get]);
 
  const loadCriteria = useCallback(async () => {
    const g: Response = await get(`methods/workflow.criteria`);

    if (g != null) {
      const d: IListResult<IWorkflowCriteria[]> = await g.json();

      if (g.status === 200 && d.ok) {
        setWorkflowCriteriaList(d.data);
      } else {
        // TODO: display error to user
      }
    }
  }, [get]);
 
  const loadAction = useCallback(async () => {
    const g: Response = await get(`methods/workflow.action`);

    if (g != null) {
      const d: IListResult<IWorkflowAction[]> = await g.json();

      if (g.status === 200 && d.ok) {
        setWorkflowActionList(d.data);
      } else {
        // TODO: display error to user
      }
    }
  }, [get]);

  useEffect(() => {
    if (user != null) {
      // move to promise.all
      loadEvent();
      loadCriteria();
      loadAction();
    }
  }, [user, get, loadEvent, loadCriteria, loadAction]);

  const onFinish = async (values: IFormData) => {
    const body = new FormData();
    body.append("name", values.name ?? "");
    body.append("priority", values.priority.toString() ?? "");
    body.append("status", values.status ?? "");
    body.append("note", values.note ?? "");

    for (let index = 0; index < values.events.length; index++) {
      body.append(`events[${index}]`, values.events[index]);
    }

    for (let index = 0; index < actionList.length; index++) {
      body.append(
        `actions[${index}].name`,
        actionList[index].name?.toString() ?? ""
      );
      body.append(
        `actions[${index}].actionFrom`,
        actionList[index].actionFrom?.toString() ?? ""
      );
      body.append(
        `actions[${index}].actionTo`,
        actionList[index].actionTo ?? ""
      );
    }

    for (let index = 0; index < criteriaList.length; index++) {
      body.append(
        `criteria[${index}].name`,
        criteriaList[index].name?.toString() ?? ""
      );
      body.append(
        `criteria[${index}].criteria`,
        criteriaList[index].criteria?.toString() ?? ""
      );
      body.append(
        `criteria[${index}].condition`,
        criteriaList[index].condition?.toString() ?? ""
      );
      body.append(`criteria[${index}].match`, criteriaList[index].match ?? "");
    }

    const headers = new Headers();
    headers.append("Authorization", `Bearer ${user?.token ?? ""}`);

    const f = await fetch("methods/workflow.create", {
      method: "POST",
      body,
      headers,
    });

    const result: ISingleResult<string> = await f.json();

    if (f.status === 201 && result.ok) {
      navigate(`/admin/workflow/${result.data}`, { replace: true });
    }else{
      setHasFormErrors(true);
      setFormErrors(result?.errors);
    }
  };

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
                <Form.Item name="events" label="Events">
                  <Select showSearch mode="multiple">
                    {workflowEventList.map((e) => (
                      <Select.Option key={e.type} value={e.type}>
                        {e.name}
                      </Select.Option>
                    ))}
                  </Select>
                </Form.Item>
                <Form.Item name="priority" label="Priority">
                  <Select>
                    <Select.Option value="1">Normal</Select.Option>
                    <Select.Option value="2">Medium</Select.Option>
                    <Select.Option value="3">High</Select.Option>
                    <Select.Option value="4">Important!</Select.Option>
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
                dot={<ClockCircleOutlined style={{ fontSize: "16px" }} />}
              >
                <div className="font-bold mb-2">
                  Workflow Criteria matches and conditions
                </div>

                {criteriaList
                  ?.sort((l, r) => l.key - r.key)
                  .map((c) => (
                    <WorkflowCriteria
                      key={c.key}
                      id={c.key}
                      criteriaTags={workflowCriteriaList}
                      criteriaList={criteriaList}
                      setCriteriaList={setCriteriaList}
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
                dot={<FireOutlined style={{ fontSize: "16px" }} />}
              >
                <div className="font-bold mb-2">
                  Action to perform base on the events above
                </div>

                {actionList
                  ?.sort((l, r) => l.key - r.key)
                  .map((c) => (
                    <WorkflowAction
                      key={c.key}
                      id={c.key}
                      actionTags={workflowActionList}
                      actionList={actionList}
                      setActionList={setActionList}
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
