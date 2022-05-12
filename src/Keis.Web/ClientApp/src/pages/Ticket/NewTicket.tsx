import { BookOutlined, CommentOutlined, UserOutlined } from "@ant-design/icons";
import { Alert, Breadcrumb, Button, Form, Input, Select, Timeline } from "antd";
import { FC, useEffect, useState } from "react";
import { Link, Outlet, useNavigate } from "react-router-dom";
import { PageView } from "../../components";
import ReactQuill from "react-quill";
import "react-quill/dist/quill.snow.css";
import { useApp, useAuth } from "../../context";
import {
  IFetchAgent,
  IFetchDepartment,
  IFetchTeam,
  IListResult,
  ISingleResult,
} from "../../interfaces";

interface INewTicket {}

interface IFormData {
  requester: string;
  priority: string;
  status: string;
  assignedto: string;
  subject: string;
  department: string;
  team: string;
  note: string;
}

const NewTicket: FC<INewTicket> = () => {
  const { user } = useAuth();
  const { get } = useApp();
  const [form] = Form.useForm();
  const navigate = useNavigate();
  const [note, setNote] = useState("");
  const [agentList, setAgentList] = useState<IFetchAgent[]>([]);
  const [departmentList, setDepartmentList] = useState<IFetchDepartment[]>([]);
  const [teamList, setTeamList] = useState<IFetchTeam[]>([]);
  const [hasFormErrors, setHasFormErrors] = useState(false);
  const [formErrors, setFormErrors] = useState<string[]>();

  useEffect(() => {
    const loadAgent = async () => {
      const g: Response = await get(`methods/agent.list`);

      if (g != null) {
        const d: IListResult<IFetchAgent[]> = await g.json();

        if (g.status === 200 && d.ok) {
          setAgentList(d.data);
        } else {
          // TODO: display error to user
        }
      }
    };

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

    if (user != null) {
      loadAgent();
      loadDepartment();
      loadTeam();
    }
  }, [user, get]);

  const onFinish = async (values: IFormData) => {
    const body = new FormData();
    body.append("requester", values.requester ?? "");
    body.append("priority", values.priority ?? "");
    body.append("status", values.status ?? "");
    body.append("assignedto", values.assignedto ?? "");
    body.append("department", values.department ?? "");
    body.append("team", values.team ?? "");
    body.append("subject", values.subject ?? "");
    body.append("body", note ?? "");

    const headers = new Headers();
    headers.append("Authorization", `Bearer ${user?.token ?? ""}`);

    const f = await fetch("methods/ticket.create", {
      method: "POST",
      body,
      headers,
    });

    const result: ISingleResult<string> = await f.json();

    if (f.status === 201 && result.ok) {
      navigate(`/ticket/${result.data}`, { replace: true });
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
        <Link to="/ticket">Tickets</Link>
      </Breadcrumb.Item>
      <Breadcrumb.Item>New Ticket</Breadcrumb.Item>
    </Breadcrumb>
  );

  return (
    <PageView title="New Ticket" breadcrumbs={<Breadcrumbs />}>
      <Form form={form} layout="vertical" onFinish={onFinish}>
        {hasFormErrors &&
          formErrors?.map((e) => (
            <div key={e} className="py-2">
              <Alert message={e} type="error" className="py-2" />
            </div>
          ))}
        <div className="grid grid-cols-1 lg:grid-cols-2 gap-3">
          <div>
            <Timeline>
              <Timeline.Item
                dot={<BookOutlined style={{ fontSize: "16px" }} />}
              >
                <div className="font-bold mb-2">Where is the ticket from</div>
                <Form.Item
                  name="requester"
                  label="Requester"
                  htmlFor="requester"
                >
                  <Select showSearch>
                    {agentList?.map((a) => (
                      <Select.Option key={a.id} value={a.id}>
                        {a.name}
                      </Select.Option>
                    ))}
                  </Select>
                </Form.Item>
                <Form.Item name="priority" label="Priority">
                  <Select showSearch>
                    <Select.Option value="1">Low</Select.Option>
                    <Select.Option value="2">Medium</Select.Option>
                    <Select.Option value="3">High</Select.Option>
                    <Select.Option value="4">Important!</Select.Option>
                  </Select>
                </Form.Item>
                {/* <Form.Item name="source" label="Source">
                                    <Select defaultValue="1" showSearch>
                                        <Select.Option value="1">App</Select.Option>
                                        <Select.Option value="2">Web</Select.Option>
                                        <Select.Option value="3">Call</Select.Option>
                                        <Select.Option value="4">API</Select.Option>
                                    </Select>
                                </Form.Item> */}
                <Form.Item name="status" label="Status">
                  <Select showSearch>
                    <Select.Option value="1">New</Select.Option>
                    <Select.Option value="2">Open</Select.Option>
                    <Select.Option value="3">Approved</Select.Option>
                    <Select.Option value="4">Assigned</Select.Option>
                    <Select.Option value="5">Pending</Select.Option>
                    <Select.Option value="6">Review</Select.Option>
                    <Select.Option value="7">Close</Select.Option>
                    <Select.Option value="8">Deleted</Select.Option>
                  </Select>
                </Form.Item>
              </Timeline.Item>
              <Timeline.Item
                dot={<UserOutlined style={{ fontSize: "16px" }} />}
              >
                <div className="font-bold mb-2">Who should work on it</div>
                <Form.Item name="assignedto" label="Assigned To">
                  <Select showSearch>
                    {agentList?.map((a) => (
                      <Select.Option key={a.id} value={a.id}>
                        {a.name}
                      </Select.Option>
                    ))}
                  </Select>
                </Form.Item>
                <Form.Item name="department" label="Department">
                  <Select showSearch>
                    {departmentList?.map((a) => (
                      <Select.Option key={a.id} value={a.id}>
                        {a.name}
                      </Select.Option>
                    ))}
                  </Select>
                </Form.Item>
                <Form.Item name="team" label="Team">
                  <Select showSearch>
                    {teamList?.map((a) => (
                      <Select.Option key={a.id} value={a.id}>
                        {a.name}
                      </Select.Option>
                    ))}
                  </Select>
                </Form.Item>
              </Timeline.Item>
              <Timeline.Item
                dot={<CommentOutlined style={{ fontSize: "16px" }} />}
              >
                <div className="font-bold mb-2">What is the issue</div>
                <Form.Item name="subject" label="Subject">
                  <Input />
                </Form.Item>
                <Form.Item label="Description">
                  <ReactQuill
                    theme="snow"
                    value={note}
                    onChange={setNote}
                    style={{ height: "240px" }}
                  />
                  <div className="mb-9"></div>
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
          </div>

          <div></div>
        </div>
      </Form>
      <Outlet />
    </PageView>
  );
};

export default NewTicket;
