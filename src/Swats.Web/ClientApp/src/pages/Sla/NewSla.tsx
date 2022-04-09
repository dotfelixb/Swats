import {
  ClockCircleOutlined,
  CommentOutlined,
  FormOutlined,
  ScheduleOutlined,
} from "@ant-design/icons";
import { Alert, Breadcrumb, Button, Form, Input, Select, Timeline } from "antd";
import React, { FC, useEffect, useState } from "react";
import { Link, Outlet, useNavigate } from "react-router-dom";
import { PageView } from "../../components";
import { useApp, useAuth } from "../../context";
import { IFetchBusinessHour, IListResult, ISingleResult } from "../../interfaces";

const { TextArea } = Input;

interface INewSla {}

interface IFormData {
  name: string;
  color: string;
  status: string;
  visibility: string;
  note: string;
}

const NewSla: FC<INewSla> = () => {
  const { user } = useAuth();
  const { get } = useApp();
  const [form] = Form.useForm();
  const navigate = useNavigate();
  const [hourList, setHourList] = useState<IFetchBusinessHour[]>();
  const [hasFormErrors, setHasFormErrors] = useState(false);
  const [formErrors, setFormErrors] = useState<string[]>();

  useEffect(() => {
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
      loadHour()
    }
  }, [user, get]);

  const onFinish = async ({
    name,
    color,
    status,
    visibility,
    note,
  }: IFormData) => {
    const body = new FormData();
    body.append("name", name ?? "");
    body.append("color", color ?? "");
    body.append("status", status ?? "");
    body.append("visibility", visibility ?? "");
    body.append("note", note ?? "");

    const headers = new Headers();
    headers.append("Authorization", `Bearer ${user?.token ?? ""}`);

    const f = await fetch("methods/sla.create", {
      method: "POST",
      body,
      headers,
    });

    const result: ISingleResult<string> = await f.json();

    if (f.status === 201 && result.ok) {
      navigate(`/admin/sla/${result.data}`, { replace: true });
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
        <Link to="/admin/sla">SLA</Link>
      </Breadcrumb.Item>
      <Breadcrumb.Item>New SLA</Breadcrumb.Item>
    </Breadcrumb>
  );

  return (
    <PageView title="New SLA" breadcrumbs={<Breadcrumbs />}>
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
                <div className="font-bold mb-2">Sla info</div>
                <Form.Item name="name" label="Name" htmlFor="name">
                  <Input />
                </Form.Item>
              </Timeline.Item>
              <Timeline.Item
                dot={<ClockCircleOutlined style={{ fontSize: "16px" }} />}
              >
                <div className="font-bold mb-2">Working hours and status</div>
                <Form.Item name="hour" label="Business Hour">
                  <Select>
                  {hourList?.map(d => (<Select.Option key={d.id} value={d.id}>{d.name}</Select.Option>))}
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
                dot={<ScheduleOutlined style={{ fontSize: "16px" }} />}
              >
                <div className="font-bold mb-2">
                  Sla actions and resolution period
                </div>
                <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-4 gap-5">
                  <Form.Item name="responseperiod" label="Response Period">
                    <Input type="number" />
                  </Form.Item>
                  <Form.Item name="responseformat" label="Period Format">
                    <Select>
                      <Select.Option value="1">Min(s)</Select.Option>
                      <Select.Option value="2">Hour(s)</Select.Option>
                      <Select.Option value="3">Days(s)</Select.Option>
                    </Select>
                  </Form.Item>
                  <Form.Item name="responsenotify" label="Inapp Notification">
                    <Select>
                      <Select.Option value="1">Yes</Select.Option>
                      <Select.Option value="2">No</Select.Option>
                    </Select>
                  </Form.Item>
                  <Form.Item name="responseemail" label="Escalation Email">
                    <Select>
                      <Select.Option value="1">Yes</Select.Option>
                      <Select.Option value="2">No</Select.Option>
                    </Select>
                  </Form.Item>
                </div>
                <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-4 gap-5">
                  <Form.Item name="resolveperiod" label="Resolution Period">
                    <Input type="number" />
                  </Form.Item>
                  <Form.Item name="resolveformat" label="Period Format">
                    <Select>
                      <Select.Option value="1">Min(s)</Select.Option>
                      <Select.Option value="2">Hour(s)</Select.Option>
                      <Select.Option value="3">Days(s)</Select.Option>
                    </Select>
                  </Form.Item>
                  <Form.Item name="resolvenotify" label="Inapp Notification">
                    <Select>
                      <Select.Option value="1">Yes</Select.Option>
                      <Select.Option value="2">No</Select.Option>
                    </Select>
                  </Form.Item>
                  <Form.Item name="resolveemail" label="Escalation Email">
                    <Select>
                      <Select.Option value="1">Yes</Select.Option>
                      <Select.Option value="2">No</Select.Option>
                    </Select>
                  </Form.Item>
                </div>
              </Timeline.Item>
              <Timeline.Item
                dot={<CommentOutlined style={{ fontSize: "16px" }} />}
              >
                <div className="font-bold mb-2">
                  Addition note for this SLA (optional)
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

export default NewSla;
