import {
  ClockCircleOutlined,
  CommentOutlined,
  FormOutlined,
} from "@ant-design/icons";
import { Alert, Breadcrumb, Button, Form, Input, Select, Timeline } from "antd";
import React, { FC, useState } from "react";
import { Link, Outlet, useNavigate } from "react-router-dom";
import { PageView } from "../../components";
import { useAuth } from "../../context";
import { ISingleResult } from "../../interfaces";

const { TextArea } = Input;

interface IFormData {
  name: string;
  color: string;
  status: string;
  visibility: string;
  note: string;
}

interface INewTag {}

const NewTag: FC<INewTag> = () => {
  const { user } = useAuth();
  const [form] = Form.useForm();
  const navigate = useNavigate();
  const [hasFormErrors, setHasFormErrors] = useState(false);
  const [formErrors, setFormErrors] = useState<string[]>();

  const onFinish = async ({ name, color, status, visibility, note }: IFormData) => {
    const body = new FormData();
    body.append("name", name ?? "");
    body.append("color", color ?? "");
    body.append("status", status ?? "");
    body.append("visibility", visibility ?? "");
    body.append("note", note ?? "");

    const headers = new Headers();
    headers.append("Authorization", `Bearer ${user?.token ?? ""}`);

    const f = await fetch("methods/tag.create", {
      method: "POST",
      body,
      headers,
    });

    const result: ISingleResult<string> = await f.json();

    if (f.status === 201 && result.ok) {
      navigate(`/admin/tag/${result.data}`, { replace: true });
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
        <Link to="/admin/tag">Tags</Link>
      </Breadcrumb.Item>
      <Breadcrumb.Item>New Tag</Breadcrumb.Item>
    </Breadcrumb>
  );

  return (
    <PageView title="New Topic" breadcrumbs={<Breadcrumbs />}>
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
                <div className="font-bold mb-2">Tag name</div>
                <Form.Item name="name" label="Name" htmlFor="name">
                  <Input />
                </Form.Item>
              </Timeline.Item>
              <Timeline.Item
                dot={<ClockCircleOutlined style={{ fontSize: "16px" }} />}
              >
                <div className="font-bold mb-2">Visibility and status</div>
                <Form.Item name="visibility" label="Visibility">
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
                dot={<CommentOutlined style={{ fontSize: "16px" }} />}
              >
                <div className="font-bold mb-2">
                  Addition note for this tag (optional)
                </div>
                <Form.Item
                  name="note"
                  label="Note"
                  htmlFor="note"
                >
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

export default NewTag;
