import {
  ClockCircleOutlined,
  CommentOutlined,
  FormOutlined,
} from "@ant-design/icons";
import {
  Alert,
  Breadcrumb,
  Button,
  DatePicker,
  Form,
  Input,
  Select,
  Switch,
  Timeline,
} from "antd";
import { FC, useState } from "react";
import { Link, Outlet, useNavigate } from "react-router-dom";
import { PageView } from "../../components";
import { useAuth } from "../../context";
import { ISingleResult } from "../../interfaces";

const { TextArea } = Input;

interface INewHour {}

interface IOpenHour {
  name: string;
  label:string;
}

interface IFormData {
  name: string;
  timezone: string;
  status: string;
  description: string;
}

const OpenHour: FC<IOpenHour> = ({ name, label }) => {
  return (
    <div className="grid grid-cols-1 sm:grid-cols-4 gap-5">
      <Form.Item name={name} valuePropName="checked">
        <Switch size="small" /> <span>{label}</span>
      </Form.Item>
      <Form.Item name={`${name}_day`} valuePropName="checked">
        <Switch size="small" /> <span>Open 24 Hours</span>
      </Form.Item>
      <Form.Item name={`${name}_from`} valuePropName="checked">
        <DatePicker placeholder="From..." picker="time"/>
      </Form.Item>
      <Form.Item name={`${name}_to`} valuePropName="checked">
      <DatePicker placeholder="To..."  picker="time"/>
      </Form.Item>
    </div>
  );
};

const NewHour: FC<INewHour> = () => {
  const { user } = useAuth();
  const [form] = Form.useForm();
  const navigate = useNavigate();
  const [hasFormErrors, setHasFormErrors] = useState(false);
  const [formErrors, setFormErrors] = useState<string[]>();

  const onFinish = async ({
    name,
    timezone,
    status,
    description,
  }: IFormData) => {
    const body = new FormData();
    body.append("name", name ?? "");
    body.append("timezone", timezone ?? "");
    body.append("status", status ?? 1);
    body.append("description", description ?? "");

    const headers = new Headers();
    headers.append("Authorization", `Bearer ${user?.token ?? ""}`);

    const f = await fetch("methods/businesshour.create", {
      method: "POST",
      body,
      headers,
    });

    const result: ISingleResult<string> = await f.json();

    if (f.status === 201 && result.ok) {
      navigate(`/admin/businesshour/${result.data}`, { replace: true });
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
        <Link to="/admin/businesshour">Business Hours</Link>
      </Breadcrumb.Item>
      <Breadcrumb.Item>New Hour</Breadcrumb.Item>
    </Breadcrumb>
  );

  return (
    <PageView title="New Hour" breadcrumbs={<Breadcrumbs />}>
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
                <div className="font-bold mb-2">Business hour name</div>
                <Form.Item name="name" label="Name" htmlFor="name">
                  <Input />
                </Form.Item>
              </Timeline.Item>
              <Timeline.Item
                dot={<ClockCircleOutlined style={{ fontSize: "16px" }} />}
              >
                <div className="font-bold mb-2">Open hours</div>
                <OpenHour name="monday" label="Monday"/>
                <OpenHour name="tuesday" label="Tuesday"/>
                <OpenHour name="wednesday" label="Wednesday"/>
                <OpenHour name="thursday" label="Thursday"/>
                <OpenHour name="friday" label="Friday"/>
                <OpenHour name="saturday" label="Saturday"/>
                <OpenHour name="sunday" label="Sunday"/>
              </Timeline.Item>
              <Timeline.Item
                dot={<ClockCircleOutlined style={{ fontSize: "16px" }} />}
              >
                <div className="font-bold mb-2">Timezone and status</div>
                <Form.Item name="timezone" label="Timezone">
                  <Select>
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
                  The description of this hour (optional)
                </div>
                <Form.Item
                  name="description"
                  label="Description"
                  htmlFor="description"
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

export default NewHour;
