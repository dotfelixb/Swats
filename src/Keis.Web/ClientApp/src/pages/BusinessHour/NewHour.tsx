import {
  ClockCircleOutlined,
  CommentOutlined,
  FormOutlined,
} from "@ant-design/icons";
import {
  Alert,
  Breadcrumb,
  Button,
  Form,
  Input,
  Select,
  Timeline,
} from "antd";
import dayjs from "dayjs";
import { FC, useState } from "react";
import { Link, Outlet, useNavigate } from "react-router-dom";
import { OpenHourItem, PageView } from "../../components";
import { useAuth } from "../../context";
import { IOpenHour, ISingleResult } from "../../interfaces";

const { TextArea } = Input;

interface INewHour { }

interface IFormData {
  name: string;
  timezone: string;
  status: string;
  description: string;
}

const initialOpenHours: IOpenHour[] = [
  { id: 1, name: "Monday", enabled: false, fullDay: false, fromTime: undefined, toTime: undefined },
  { id: 2, name: "Tuesday", enabled: false, fullDay: false, fromTime: undefined, toTime: undefined },
  { id: 3, name: "Wednesday", enabled: false, fullDay: false, fromTime: undefined, toTime: undefined },
  { id: 4, name: "Thursday", enabled: false, fullDay: false, fromTime: undefined, toTime: undefined },
  { id: 5, name: "Friday", enabled: false, fullDay: false, fromTime: undefined, toTime: undefined },
  { id: 6, name: "Saturday", enabled: false, fullDay: false, fromTime: undefined, toTime: undefined },
  { id: 7, name: "Sunday", enabled: false, fullDay: false, fromTime: undefined, toTime: undefined },
];

const NewHour: FC<INewHour> = () => {
  const { user } = useAuth();
  const [form] = Form.useForm();
  const navigate = useNavigate();
  const [hoursList, setHoursList] = useState<IOpenHour[]>(initialOpenHours);
  const [hasFormErrors, setHasFormErrors] = useState(false);
  const [formErrors, setFormErrors] = useState<string[]>();

  const onFinish = async (values: IFormData) => {
    const body = new FormData();
    body.append("name", values.name ?? "");
    body.append("timezone", values.timezone ?? "");
    body.append("status", values.status ?? 1);
    body.append("description", values.description ?? "");

    const defaultDate = dayjs("01/01/0001 00:00:00").format().toString();
    for (let index = 0; index < hoursList.length; index++) {
      body.append(`openhours[${index}].name`, hoursList[index].name ?? "");
      body.append(`openhours[${index}].enabled`, hoursList[index].enabled?.toString() ?? "");
      body.append(`openhours[${index}].fullDay`, hoursList[index].fullDay?.toString() ?? "");
      body.append(`openhours[${index}].fromTime`, hoursList[index].fromTime?.toString() ?? defaultDate);
      body.append(`openhours[${index}].toTime`, hoursList[index].toTime?.toString() ?? defaultDate);
    }

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
                {hoursList.sort((a, b) => a.id - b.id).map(o => (
                  <OpenHourItem
                    key={o.name}
                    data={o}
                    hoursList={hoursList}
                    setHoursList={setHoursList} />
                ))}
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
