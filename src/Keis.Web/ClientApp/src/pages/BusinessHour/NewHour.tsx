import {
  ClockCircleOutlined,
  CommentOutlined,
  FormOutlined,
} from "@ant-design/icons";
import {
  Alert,
  Breadcrumb,
  Button,
  Checkbox,
  DatePicker,
  Form,
  Input,
  Select,
  Timeline,
} from "antd";
import dayjs from "dayjs";
import { FC, useState } from "react";
import { Link, Outlet, useNavigate } from "react-router-dom";
import { PageView } from "../../components";
import { useAuth } from "../../context";
import { ISingleResult } from "../../interfaces";

const { TextArea } = Input;

interface INewHour { }

interface IOpenHourItem {
  value: string;
  label: string;
}

interface IFormData {
  name: string;
  timezone: string;
  status: string;
  description: string;
}

interface IOpenHour {
  name:string;
  enabled: boolean;
  fullday: boolean;
  fromtime: string;
  totime:string;
}

const OpenHourItem: FC<IOpenHourItem> = ({ value, label }) => {
  const [dayDisabled, setDayDisabled] = useState(true);
  const [dateDisabled, setDateDisabled] = useState(true);

  const onChange = (e: any) => {
    setDayDisabled(!e.target.checked);
    setDateDisabled(!e.target.checked);
  }

  const onDayChange = (e: any) => {
    setDateDisabled(e.target.checked)
  }

  return (
    <div className="grid grid-cols-1 sm:grid-cols-4 gap-5">
      <Form.Item name={value} valuePropName="checked">
        <Checkbox onChange={onChange} >{label}</Checkbox>
      </Form.Item>
      <Form.Item name={`${value}hour`} valuePropName="checked">
        <Checkbox onChange={onDayChange} disabled={dayDisabled}>Open 24 Hours</Checkbox>
      </Form.Item>
      <Form.Item name={`${value}from`} valuePropName="checked">
        <DatePicker placeholder="From" onChange={(t)=> console.log(t)} picker="time" disabled={dateDisabled} />
      </Form.Item>
      <Form.Item name={`${value}to`} valuePropName="checked">
        <DatePicker placeholder="To" picker="time" disabled={dateDisabled} />
      </Form.Item>
    </div>
  );
};

const NewHour: FC<INewHour> = () => {
  const { user } = useAuth();
  const [form] = Form.useForm();
  const navigate = useNavigate();
  const [openHoursList, setOpenHoursList] = useState<IOpenHour[]>([]);
  const [hasFormErrors, setHasFormErrors] = useState(false);
  const [formErrors, setFormErrors] = useState<string[]>();

  const onFinish = async (values: IFormData) => {
    const body = new FormData();
    body.append("name", values.name ?? "");
    body.append("timezone", values.timezone ?? "");
    body.append("status", values.status ?? 1);
    body.append("description", values.description ?? "");

    for (let index = 0; index < openHoursList.length; index++) {
      body.append(`openhours[${index}].name`, openHoursList[index].name ?? "");
      body.append(`openhours[${index}].enabled`, openHoursList[index].enabled?.toString() ?? "");
      body.append(`openhours[${index}].fullday`, openHoursList[index].fullday?.toString() ?? "");
      body.append(`openhours[${index}].fromtime`, openHoursList[index].fromtime?.toString() ?? "");
      body.append(`openhours[${index}].totime`, openHoursList[index].totime?.toString() ?? "");
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
                <OpenHourItem value="monday" label="Monday" />
                <OpenHourItem value="tuesday" label="Tuesday" />
                <OpenHourItem value="wednesday" label="Wednesday" />
                <OpenHourItem value="thursday" label="Thursday" />
                <OpenHourItem value="friday" label="Friday" />
                <OpenHourItem value="saturday" label="Saturday" />
                <OpenHourItem value="sunday" label="Sunday" />
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
